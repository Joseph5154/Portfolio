using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

// While doing research, I found this website:
// https://www.syncfusion.com/succinctly-free-ebooks/monogame-succinctly/creating-the-first-game
// Seems to cover a lot of the same ground we are, for future reference. - Joe Z.
namespace SandStrider
{
    public enum GameState
    {
        MainMenu,
        Game,
        GameOver,
        Shop,
        PlayerExperience,
        NotLoadedFile
    }

    // Helper classes should be created for drawing sets of sprites
    // during the game, like the enemies and projectiles and whatnot.
    public class Game1 : Game
    {
        // Texture fields.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ContentManager contentManager;

        // Control related fields
        private KeyboardState previousKBState;
        private GameState currentState;

        // Game objects
        private Player playerChar;
        private RoomManager roomManager;
        private ScreenManager screenManager;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.Window.Title = "Sand Strider";
            Window.AllowAltF4 = true;
        }

        protected override void Initialize()
        {
             // Adjust window size to be larger
            _graphics.PreferredBackBufferWidth = 1000; 
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();

            currentState = GameState.MainMenu;
            previousKBState = new KeyboardState();
            contentManager = new ContentManager();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Texture Loading
            contentManager.Add("titleScreenLogo", Content.Load<Texture2D>("Sand Strider Logo"));
            contentManager.Add("pointerTexture", Content.Load<Texture2D>("pointer"));
            contentManager.Add("sliderTexture", Content.Load<Texture2D>("thermometer_transparent"));
            contentManager.Add("sliderOffOne", Content.Load<Texture2D>("thermometer_transparent_offset_two"));
            contentManager.Add("sliderOffTwo", Content.Load<Texture2D>("thermometer_transparent_offset_three"));
            contentManager.Add("sliderOffThree", Content.Load<Texture2D>("thermometer_transparent_offset_four"));
            contentManager.Add("playerBackground", Content.Load<Texture2D>("playerBackground"));
            contentManager.Add("playerBackgroundEmpty", Content.Load<Texture2D>("playerBackgroundEmpty"));
            contentManager.Add("buttonOutline", Content.Load<Texture2D>("button outline"));
            contentManager.Add("buttonOutlineSelected", Content.Load<Texture2D>("button_outline_selected"));
            contentManager.Add("coinTexture", Content.Load<Texture2D>("01coin"));
            contentManager.Add("coinWidthHeight", Content.Load<Texture2D>("coin"));
            contentManager.Add("mummyTexture", Content.Load<Texture2D>("Mummy Sprite Sheet"));
            contentManager.Add("playerTexture", Content.Load<Texture2D>("playerAnimations"));
            contentManager.Add("projectile", Content.Load<Texture2D>("projectile"));
            contentManager.Add("pickupOne", Content.Load<Texture2D>("potion_green"));
            contentManager.Add("pickupTwo", Content.Load<Texture2D>("potion_red"));
            contentManager.Add("pickupThree", Content.Load<Texture2D>("potion_yellow"));
            contentManager.Add("titlePlay", Content.Load<Texture2D>("play"));
            contentManager.Add("titleLoad", Content.Load<Texture2D>("load"));
            contentManager.Add("desertBackground", Content.Load<Texture2D>("desertBackground"));
            contentManager.Add("desertBackgroundEnd", Content.Load<Texture2D>("desertBackgroundEnd"));
            contentManager.Add("redX", Content.Load<Texture2D>("redX"));
            contentManager.Add("checkmark", Content.Load<Texture2D>("checkmark"));

            // Room Related Textures
            contentManager.Add("bannerWallOne", Content.Load<Texture2D>("bannerWallOne"));
            contentManager.Add("bannerWallTwo", Content.Load<Texture2D>("bannerWallTwo"));
            contentManager.Add("bottomLeftCorner", Content.Load<Texture2D>("bottomLeftCorner"));
            contentManager.Add("bottomLeftPassage", Content.Load<Texture2D>("bottomLeftPassage"));
            contentManager.Add("bottomRightCorner", Content.Load<Texture2D>("bottomRightCorner"));
            contentManager.Add("bottomRightPassage", Content.Load<Texture2D>("bottomRightPassage"));
            contentManager.Add("bottomWallOne", Content.Load<Texture2D>("bottomWallOne"));
            contentManager.Add("bottomWallTwo", Content.Load<Texture2D>("bottomWallTwo"));
            contentManager.Add("chest", Content.Load<Texture2D>("chest"));
            contentManager.Add("crackedWallOne", Content.Load<Texture2D>("crackedWallOne"));
            contentManager.Add("crackedWallTwo", Content.Load<Texture2D>("crackedWallTwo"));
            contentManager.Add("debris", Content.Load<Texture2D>("debris"));
            contentManager.Add("emptyTile", Content.Load<Texture2D>("emptyTile"));
            contentManager.Add("floorOne", Content.Load<Texture2D>("floorOne"));
            contentManager.Add("floorTwo", Content.Load<Texture2D>("floorTwo"));
            contentManager.Add("floorThree", Content.Load<Texture2D>("floorThree"));
            contentManager.Add("floorFour", Content.Load<Texture2D>("floorFour"));
            contentManager.Add("leftCornerShadow", Content.Load<Texture2D>("leftCornerShadow"));
            contentManager.Add("leftShadow", Content.Load<Texture2D>("leftShadow"));
            contentManager.Add("leftShadowAlt", Content.Load<Texture2D>("leftShadowAlt"));
            contentManager.Add("leftWall", Content.Load<Texture2D>("leftWall"));
            contentManager.Add("middleShadow", Content.Load<Texture2D>("middleShadow"));
            contentManager.Add("midWall", Content.Load<Texture2D>("midWall"));
            contentManager.Add("rightCornerShadow", Content.Load<Texture2D>("rightCornerShadow"));
            contentManager.Add("rightWall", Content.Load<Texture2D>("rightWall"));
            contentManager.Add("sunWall", Content.Load<Texture2D>("sunWall"));
            contentManager.Add("topLeftCorner", Content.Load<Texture2D>("topLeftCorner"));
            contentManager.Add("topLeftPassage", Content.Load<Texture2D>("topLeftPassage"));
            contentManager.Add("topLeftShadow", Content.Load<Texture2D>("topLeftShadow"));
            contentManager.Add("topRightCorner", Content.Load<Texture2D>("topRightCorner"));
            contentManager.Add("topRightPassage", Content.Load<Texture2D>("topRightPassage"));
            contentManager.Add("topShadow", Content.Load<Texture2D>("topShadow"));
            contentManager.Add("topShadowAlt", Content.Load<Texture2D>("topShadowAlt"));
            contentManager.Add("topWallOne", Content.Load<Texture2D>("topWallOne"));
            contentManager.Add("topWallTwo", Content.Load<Texture2D>("topWallTwo"));
            contentManager.Add("topWallThree", Content.Load<Texture2D>("topWallThree"));
            contentManager.Add("candles", Content.Load<Texture2D>("candlesObstacle"));
            contentManager.Add("stairs", Content.Load<Texture2D>("stairsObstacle"));
            contentManager.Add("pit", Content.Load<Texture2D>("pitObstacle"));
            contentManager.Add("pillarLower", Content.Load<Texture2D>("pillarLower"));
            contentManager.Add("pillarUpper", Content.Load<Texture2D>("pillarUpper"));
            contentManager.Add("sandBottomLeft", Content.Load<Texture2D>("sandBottomLeft"));
            contentManager.Add("sandBottomMid", Content.Load<Texture2D>("sandBottomMid"));
            contentManager.Add("sandBottomRight", Content.Load<Texture2D>("sandBottomRight"));
            contentManager.Add("sandTopLeft", Content.Load<Texture2D>("sandTopLeft"));
            contentManager.Add("sandTopMid", Content.Load<Texture2D>("sandTopMid"));
            contentManager.Add("sandTopRight", Content.Load<Texture2D>("sandTopRight"));
            contentManager.Add("ladder", Content.Load<Texture2D>("ladder"));
            contentManager.Add("camel", Content.Load<Texture2D>("camel"));
            contentManager.Add("shopRock", Content.Load<Texture2D>("shopRock"));
            contentManager.Add("doorExplosion", Content.Load<Texture2D>("doorExplosion"));

            // Health and Soul Bar
            contentManager.Add("health0", Content.Load<Texture2D>("health0"));
            contentManager.Add("health10", Content.Load<Texture2D>("health10"));
            contentManager.Add("health20", Content.Load<Texture2D>("health20"));
            contentManager.Add("health30", Content.Load<Texture2D>("health30"));
            contentManager.Add("health40", Content.Load<Texture2D>("health40"));
            contentManager.Add("health50", Content.Load<Texture2D>("health50"));
            contentManager.Add("health60", Content.Load<Texture2D>("health60"));
            contentManager.Add("health70", Content.Load<Texture2D>("health70"));
            contentManager.Add("health80", Content.Load<Texture2D>("health80"));
            contentManager.Add("health90", Content.Load<Texture2D>("health90"));
            contentManager.Add("health100", Content.Load<Texture2D>("health100"));
            contentManager.Add("soul10", Content.Load<Texture2D>("soul10"));
            contentManager.Add("soul20", Content.Load<Texture2D>("soul20"));
            contentManager.Add("soul30", Content.Load<Texture2D>("soul30"));
            contentManager.Add("soul40", Content.Load<Texture2D>("soul40"));
            contentManager.Add("soul50", Content.Load<Texture2D>("soul50"));
            contentManager.Add("soul60", Content.Load<Texture2D>("soul60"));
            contentManager.Add("soul70", Content.Load<Texture2D>("soul70"));
            contentManager.Add("soul80", Content.Load<Texture2D>("soul80"));
            contentManager.Add("soul90", Content.Load<Texture2D>("soul90"));
            contentManager.Add("soul100", Content.Load<Texture2D>("soul100"));
            contentManager.Add("enemyDeath", Content.Load<Texture2D>("enemyDeath"));

            // Pickups
            contentManager.Add("ring", Content.Load<Texture2D>("ringPickup"));
            contentManager.Add("sword", Content.Load<Texture2D>("swordPickup"));
            contentManager.Add("necklace", Content.Load<Texture2D>("necklacePickup"));
            contentManager.Add("potion", Content.Load<Texture2D>("potionPickup"));
            contentManager.Add("plant", Content.Load<Texture2D>("plantPickup"));
            contentManager.Add("arrow", Content.Load<Texture2D>("arrow"));
            contentManager.Add("powerArrow", Content.Load<Texture2D>("powerArrow"));
            contentManager.Add("armGuard", Content.Load<Texture2D>("armGuard"));
            contentManager.Add("feather", Content.Load<Texture2D>("feather"));
            contentManager.Add("herb", Content.Load<Texture2D>("herb"));
            contentManager.Add("ankh", Content.Load<Texture2D>("ankh"));
            contentManager.Add("leatherArmor", Content.Load<Texture2D>("leatherArmor"));
            contentManager.Add("ironArmor", Content.Load<Texture2D>("ironArmor"));
            contentManager.Add("quiver", Content.Load<Texture2D>("quiver"));
            contentManager.Add("book", Content.Load<Texture2D>("book"));
            contentManager.Add("boots", Content.Load<Texture2D>("boots"));
            contentManager.Add("crossbow", Content.Load<Texture2D>("crossbow"));
            contentManager.Add("bow", Content.Load<Texture2D>("bow"));


            contentManager.Font = Content.Load<SpriteFont>("TempFont"); 


            playerChar = new Player(350, 225, contentManager);
            screenManager = new ScreenManager(playerChar, contentManager);
            roomManager = new RoomManager(contentManager, playerChar, screenManager);
        }

        protected override void Update(GameTime gameTime)
        {
            // Esc exit functionality, commented out.
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            KeyboardState kb = Keyboard.GetState();

            screenManager.CurrentMouseState = Mouse.GetState();
            screenManager.UpdateButtonHovering();

            // FSM
            switch (currentState)
            {
                case GameState.Game:
                    roomManager.Update(gameTime, kb);
                    playerChar.Update(gameTime, kb);

                    if (playerChar.Dead || playerChar.HasExtracted)
                    {
                        currentState = GameState.GameOver;
                    }

                    // Escape the dungeon with esc key (still lose items).
                    if (kb.IsKeyDown(Keys.Escape) && previousKBState.GetPressedKeyCount() == 0)
                    {
                        currentState = GameState.GameOver;
                    }

                    break;

                case GameState.MainMenu:

                    if (screenManager.IsHoveringPlay && screenManager.SingleClick)
                    {
                        currentState = GameState.PlayerExperience;
                        roomManager.Reset();
                    }

                    // Escape the game
                    if (kb.IsKeyDown(Keys.Escape) && previousKBState.GetPressedKeyCount() == 0)
                    {
                        Exit();
                    }

                    if (screenManager.IsHoveringLoad && screenManager.SingleClick)
                    {
                        try
                        {
                            roomManager.LoadCustomItem();
                            string path = roomManager.GetPathToTexture();
                            FileStream fileStream = new FileStream(path, FileMode.Open);
                            Texture2D customItemTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
                            fileStream.Dispose();

                            contentManager.Add("customItem", customItemTexture);
                            screenManager.FileLoaded = true;
                        }
                        catch
                        {
                            currentState = GameState.NotLoadedFile;
                            roomManager.UnloadCustomItem();
                        }
                    }

                    // Hotkey to test GameOver screen.
                    //if (kb.IsKeyDown(Keys.Space) && previousKBState.IsKeyUp(Keys.Space))
                        //currentState = GameState.GameOver;

                    IsMouseVisible = true;
                    break;

                case GameState.PlayerExperience:
                    
                    if (kb.IsKeyDown(Keys.Escape) && previousKBState.GetPressedKeyCount() == 0)
                    {
                        currentState = GameState.MainMenu;
                    }
                    else if (kb.GetPressedKeyCount() > 0 && previousKBState.GetPressedKeyCount() == 0)
                    {
                        currentState = GameState.Game;
                    }

                    if(screenManager.IsHoveringReset && screenManager.SingleClick)
                            playerChar.ResetStats();

                    break;

                case GameState.NotLoadedFile:
                    if (kb.GetPressedKeyCount() > 0 && previousKBState.GetPressedKeyCount() == 0)
                        currentState = GameState.MainMenu;
                    break;

                case GameState.GameOver:
                    if (kb.GetPressedKeyCount() > 0 && previousKBState.GetPressedKeyCount() == 0)
                    {
                        currentState = GameState.MainMenu;
                        roomManager.Reset();
                    }

                    break;
            }

            // So actions do not repeat every frame
            previousKBState = kb;
            screenManager.PreviousMouseState = screenManager.CurrentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // The only thing changed here from default is the sampler state, which has been set to better
            // support pixel scaling.
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            // GameState FSM
            if (currentState == GameState.MainMenu)
            {
                screenManager.DrawMainMenu(_spriteBatch);
            }

            else if (currentState == GameState.PlayerExperience)
            {
                screenManager.DrawPlayerExperience(_spriteBatch);
            }

            else if (currentState == GameState.GameOver)
            {
                _spriteBatch.Draw(contentManager.Textures["desertBackgroundEnd"], new Rectangle(0, 0, 1000, 500), Color.White);

                // Color and text of game over screen depends on if the player died or escaped.
                if (playerChar.Dead)
                    _spriteBatch.DrawString(contentManager.Font, "GAME OVER - You Died",
                        new Vector2(50, 150), Color.Red, 0, new Vector2(0, 0), 4, SpriteEffects.None, 1);
                else if (playerChar.HasExtracted)
                    _spriteBatch.DrawString(contentManager.Font, "GAME OVER - You Extracted",
                        new Vector2(90, 150), Color.Green, 0, new Vector2(0, 0), 3, SpriteEffects.None, 1);
                else
                    _spriteBatch.DrawString(contentManager.Font, "GAME OVER - You Forfeited",
                        new Vector2(90, 150), Color.Yellow, 0, new Vector2(0, 0), 3, SpriteEffects.None, 1);

                _spriteBatch.DrawString(contentManager.Font, "Press Any Key to Return to Main Menu",
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - 200, 300), Color.White);
            }
            else if (currentState == GameState.Game)
            {
                roomManager.CurrentRoom.RoomDraw(_spriteBatch);
                playerChar.Draw(_spriteBatch);
            }
            else if (currentState == GameState.NotLoadedFile)
            {
                _spriteBatch.DrawString(contentManager.Font, "You have not created a Custom Item",
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - 150, 150), Color.Red);

                _spriteBatch.DrawString(contentManager.Font, "Press Any Key to Return to Main Menu",
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - 200, 300), Color.Red);
            }
            else
            {
                // I don't know how the player would access outside of these 4 options,
                // but exit the application if somehow that happens.
                this.Exit();
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}