using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SandStrider
{
    // Luciano Balsamo
    // Class designed to help simplify main/Game1 by doing the dirty drawing work.
    internal class ScreenManager
    {
        private Player player;
        private ContentManager contentManager;

        // Button Hovering Logic
        private bool isHoveringPlay = false;
        private bool isHoveringLoad = false;
        private bool isHoveringReset = false;
        private bool isHoveringShopTopLeft = false;
        private bool isHoveringShopTopRight = false;
        private bool isHoveringShopMidLeft = false;
        private bool isHoveringShopMidRight = false;
        private bool isHoveringShopBottomLeft = false;
        private bool isHoveringShopBottomRight = false;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private bool fileLoaded = false;




        public MouseState CurrentMouseState
        {
            get { return currentMouseState; } 
            set {  currentMouseState = value; }
        }


        public MouseState PreviousMouseState
        {
            get { return previousMouseState; }
            set { previousMouseState = value; }
        }


        public bool FileLoaded
        {
            get { return fileLoaded; }
            set { fileLoaded = value; }
        }


        public ScreenManager(Player player, ContentManager contentManager)
        {
            this.player = player;
            this.contentManager = contentManager;
        }


        public bool SingleClick
        {
            get
            {
                if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                    return true;

                return false;
            }
        }


        public bool IsHoveringPlay { get { return isHoveringPlay; } }
        public bool IsHoveringLoad { get {  return isHoveringLoad; } }
        public bool IsHoveringReset { get {  return isHoveringReset; } }


        public void UpdateButtonHovering()
        {
            if (currentMouseState.X > 338 && currentMouseState.X < 635 && currentMouseState.Y > 222 && currentMouseState.Y < 310)
            {
                isHoveringPlay = true;
            }
            else
                isHoveringPlay = false;

            if (currentMouseState.X > 338 && currentMouseState.X < 635 && currentMouseState.Y > 347 && currentMouseState.Y < 435)
            {
                isHoveringLoad = true;
            }
            else
                isHoveringLoad = false;

            if (currentMouseState.X > 770 && currentMouseState.X < 935 && currentMouseState.Y > 395 && currentMouseState.Y < 455)
                isHoveringReset = true;
            else
                isHoveringReset = false;

            if (currentMouseState.X > 253 && currentMouseState.X < 293 && currentMouseState.Y > 151 && currentMouseState.Y < 191)
                isHoveringShopTopLeft = true;
            else
                isHoveringShopTopLeft = false;

            if (currentMouseState.X > 453 && currentMouseState.X < 493 && currentMouseState.Y > 151 && currentMouseState.Y < 191)
                isHoveringShopTopRight = true;
            else
                isHoveringShopTopRight = false;

            if (currentMouseState.X > 203 && currentMouseState.X < 243 && currentMouseState.Y > 251 && currentMouseState.Y < 291)
                isHoveringShopMidLeft = true;
            else
                isHoveringShopMidLeft = false;

            if (currentMouseState.X > 503 && currentMouseState.X < 543 && currentMouseState.Y > 251 && currentMouseState.Y < 291)
                isHoveringShopMidRight = true;
            else
                isHoveringShopMidRight = false;

            if (currentMouseState.X > 253 && currentMouseState.X < 293 && currentMouseState.Y > 351 && currentMouseState.Y < 391)
                isHoveringShopBottomLeft = true;
            else
                isHoveringShopBottomLeft = false;

            if (currentMouseState.X > 453 && currentMouseState.X < 493 && currentMouseState.Y > 351 && currentMouseState.Y < 391)
                isHoveringShopBottomRight = true;
            else
                isHoveringShopBottomRight = false;
        }


        public void DrawShop(SpriteBatch sb, Room shop)
        {
            if (player.SwordUnlocked)
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(248, 121, 50, 50), Color.White);
            else
            {
                sb.Draw(contentManager.Textures["sword"], new Rectangle(248, 121, 50, 50), Color.White);
                sb.DrawString(contentManager.Font, "100 Coins", new Vector2(228, 191), Color.White);
            }

            if (player.BookUnlocked)
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(448, 121, 50, 50), Color.White);
            else
            {
                sb.Draw(contentManager.Textures["book"], new Rectangle(448, 121, 50, 50), Color.White);
                sb.DrawString(contentManager.Font, "50 Coins", new Vector2(428, 191), Color.White);
            }

            if (player.TempStats[6] + player.PermStats[6] >= 15 || shop.LeatherBought)
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(198, 221, 50, 50), Color.White);
            else
            {
                sb.Draw(contentManager.Textures["leatherArmor"], new Rectangle(198, 221, 50, 50), Color.White);
                sb.DrawString(contentManager.Font, "25 Coins", new Vector2(178, 291), Color.White);
            }

            if (player.TempStats[7] + player.PermStats[7] >= 15 || shop.IronBought)
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(498, 221, 50, 50), Color.White);
            else
            {
                sb.Draw(contentManager.Textures["ironArmor"], new Rectangle(498, 221, 50, 50), Color.White);
                sb.DrawString(contentManager.Font, "25 Coins", new Vector2(478, 291), Color.White);
            }

            if (player.Bolts >= 7 || shop.BoltBought)
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(248, 321, 50, 50), Color.White);
            else
            {
                sb.Draw(contentManager.Textures["crossbow"], new Rectangle(248, 321, 50, 50), Color.White);
                sb.DrawString(contentManager.Font, "50 Coins", new Vector2(228, 391), Color.White);
            }

            if (player.AnkhUnlocked)
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(448, 321, 50, 50), Color.White);
            else
            {
                sb.Draw(contentManager.Textures["ankh"], new Rectangle(448, 321, 50, 50), Color.White);
                sb.DrawString(contentManager.Font, "125 Coins", new Vector2(428, 391), Color.White);
            }
        }


        public void UpdateShop(Room shop)
        {
            int wallet = player.TempStats[9] + player.PermStats[9];
            int cost = 0;

            if (isHoveringShopTopLeft && SingleClick && wallet >= 100 && !player.SwordUnlocked)
            {
                cost = 100;
                player.SwordUnlocked = true;
            }

            else if (isHoveringShopTopRight && SingleClick && wallet >= 50 && !player.BookUnlocked)
            {
                cost = 50;
                player.BookUnlocked = true;
            }

            else if (isHoveringShopMidLeft && SingleClick && wallet >= 25 && player.TempStats[6] + player.PermStats[6] < 15 && !shop.LeatherBought)
            {
                cost = 25;
                player.TempStats[6]++;
                player.DodgeChance += 5;
                shop.LeatherBought = true;
            }

            else if (isHoveringShopMidRight && SingleClick && wallet >= 25 && player.TempStats[7] + player.PermStats[7] < 15 && !shop.IronBought)
            {
                cost = 25;
                player.TempStats[7]++;
                player.DamageReduction += 5;
                shop.IronBought = true;
            }

            else if (isHoveringShopBottomLeft && SingleClick && wallet >= 50 && player.Bolts < 7 && !shop.BoltBought)
            {
                cost = 50;
                player.Bolts++;
                shop.BoltBought = true;
            }

            else if (isHoveringShopBottomRight && SingleClick && wallet >= 100 && !player.AnkhUnlocked)
            {
                cost = 125;
                player.AnkhUnlocked = true;
            }

            player.TempStats[9] -= cost;
        }


        public void DrawMainMenu(SpriteBatch sb)
        {
            sb.Draw(contentManager.Textures["desertBackground"], new Rectangle(0, 0, 1000, 500), Color.White);

            sb.Draw(contentManager.Textures["playerBackgroundEmpty"], new Rectangle(65, 30, 880, 150), Color.White);

            sb.Draw(contentManager.Textures["titleScreenLogo"], new Vector2(100, 50),
                null, Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);

            if (!isHoveringPlay)
                sb.Draw(contentManager.Textures["buttonOutline"], new Vector2(315, 200),
               null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            else
                sb.Draw(contentManager.Textures["buttonOutlineSelected"], new Vector2(315, 200),
               null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);

            if (!isHoveringLoad)
                sb.Draw(contentManager.Textures["buttonOutline"], new Vector2(315, 325),
                null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            else
                sb.Draw(contentManager.Textures["buttonOutlineSelected"], new Vector2(315, 325),
                null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["titlePlay"], new Vector2(410, 230),
                null, Color.White, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 1);

            if (!fileLoaded)
            {
                sb.Draw(contentManager.Textures["titleLoad"], new Vector2(410, 365),
                    null, Color.White, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 1);
            }
            else
            {
                sb.Draw(contentManager.Textures["titleLoad"], new Vector2(360, 365),
                    null, Color.White, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 1);
                sb.Draw(contentManager.Textures["checkmark"], new Rectangle(480, 275, 200, 200), Color.White);
            }
        }


        public void DrawPlayerExperience(SpriteBatch sb)
        {
            sb.Draw(contentManager.Textures["desertBackground"], new Rectangle(0, 0, 1000, 500), Color.White);
            sb.Draw(contentManager.Textures["playerBackgroundEmpty"], new Rectangle(20, 10, 960, 480), Color.White);

            sb.DrawString(contentManager.Font, "INVENTORY", new Vector2(100, 50), Color.OrangeRed, 0, new Vector2(0, 0), 2, SpriteEffects.None, 1);
            sb.DrawString(contentManager.Font, "STATS", new Vector2(430, 50), Color.OrangeRed, 0, new Vector2(0, 0), 2, SpriteEffects.None, 1);
            sb.DrawString(contentManager.Font, "INFO", new Vector2(750, 50), Color.OrangeRed, 0, new Vector2(0, 0), 2, SpriteEffects.None, 1);


            sb.DrawString(contentManager.Font, "Press Any Key to Start Dungeon",
            new Vector2(340, 425), Color.OrangeRed);


            sb.Draw(contentManager.Textures["herb"], new Rectangle(100, 120, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[0], new Vector2(150, 130), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["quiver"], new Rectangle(200, 120, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[1], new Vector2(250, 130), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                
            sb.Draw(contentManager.Textures["ring"], new Rectangle(100, 180, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[2], new Vector2(150, 190), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["necklace"], new Rectangle(200, 180, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[3], new Vector2(250, 190), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["armGuard"], new Rectangle(100, 240, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[4], new Vector2(150, 250), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["feather"], new Rectangle(200, 240, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[5], new Vector2(250, 250), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["leatherArmor"], new Rectangle(100, 300, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[6], new Vector2(150, 310), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["ironArmor"], new Rectangle(200, 300, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[7], new Vector2(250, 310), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["crossbow"], new Rectangle(100, 360, 35, 35), Color.White);
            sb.DrawString(contentManager.Font, "X" + player.PermStats[10], new Vector2(150, 370), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["sword"], new Rectangle(200, 360, 35, 35), Color.White);
            if (!player.SwordUnlocked)
                sb.Draw(contentManager.Textures["redX"], new Rectangle(230, 350, 50, 50), Color.White);
            else sb.Draw(contentManager.Textures["checkmark"], new Rectangle(230, 350, 50, 50), Color.White);

            sb.Draw(contentManager.Textures["ankh"], new Rectangle(100, 420, 35, 35), Color.White);
            if (!player.AnkhUnlocked)
                sb.Draw(contentManager.Textures["redX"], new Rectangle(130, 410, 50, 50), Color.White);
            else sb.Draw(contentManager.Textures["checkmark"], new Rectangle(130, 410, 50, 50), Color.White);

            sb.Draw(contentManager.Textures["book"], new Rectangle(200, 420, 35, 35), Color.White);
            if (!player.BookUnlocked)
                sb.Draw(contentManager.Textures["redX"], new Rectangle(230, 410, 50, 50), Color.White);
            else sb.Draw(contentManager.Textures["checkmark"], new Rectangle(230, 410, 50, 50), Color.White);


            sb.Draw(contentManager.Textures["herb"], new Rectangle(345, 120, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Max Health: " + player.MaxHealth, new Vector2(375, 117), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["quiver"], new Rectangle(345, 140, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Attack Dmg: " + player.AttackDamage, new Vector2(375, 137), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["ring"], new Rectangle(345, 160, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Attack Speed: " + player.AttackSpeed, new Vector2(375, 157), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["necklace"], new Rectangle(345, 180, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Movement Speed: " + player.MovementSpeed, new Vector2(375, 177), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["armGuard"], new Rectangle(345, 200, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Crit Dmg Multiply: " + player.CriticalDamage + "x", new Vector2(375, 197), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["feather"], new Rectangle(345, 220, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Arrow Speed: " + player.ArrowSpeed, new Vector2(375, 217), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["leatherArmor"], new Rectangle(345, 240, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Dodge Chance: " + player.DodgeChance + "%", new Vector2(375, 237), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["ironArmor"], new Rectangle(345, 260, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Dmg Reduction: " + player.DamageReduction + "%", new Vector2(375, 257), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["crossbow"], new Rectangle(345, 280, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Arrows per Shot: " + player.Bolts, new Vector2(375, 277), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["coinTexture"], new Rectangle(345, 300, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Coins: " + player.PermStats[9], new Vector2(375, 297), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["debris"], new Rectangle(345, 320, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Difficulty Level: " + player.RoomCount, new Vector2(375, 317), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["boots"], new Rectangle(645, 120, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "WASD to Move", new Vector2(675, 117), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["bow"], new Rectangle(645, 140, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "LCTRL or E to Shoot", new Vector2(675, 137), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["potion"], new Rectangle(645, 160, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Potions replenish health", new Vector2(675, 157), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["camel"], new Rectangle(645, 180, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Spend coins at shop", new Vector2(675, 177), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["ladder"], new Rectangle(645, 200, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "Touch ladder to extract", new Vector2(675, 197), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["sword"], new Rectangle(645, 220, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "LSHIFT or Q to Swing", new Vector2(675, 217), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);

            sb.Draw(contentManager.Textures["book"], new Rectangle(645, 240, 15, 15), Color.White);
            sb.DrawString(contentManager.Font, "T to recall to shop", new Vector2(675, 237), Color.OrangeRed, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);


            if (!isHoveringReset)
            {
                sb.Draw(contentManager.Textures["buttonOutline"], new Vector2(770, 395),
                    null, Color.White, 0, new Vector2(0, 0), 0.2f, SpriteEffects.None, 1);
                sb.DrawString(contentManager.Font, "Reset Stats?", new Vector2(792, 420), Color.White);
            }
            else
            {
                sb.Draw(contentManager.Textures["buttonOutlineSelected"], new Vector2(770, 395),
                    null, Color.White, 0, new Vector2(0, 0), 0.2f, SpriteEffects.None, 1);
                sb.DrawString(contentManager.Font, "Reset Stats!", new Vector2(792, 420), Color.Yellow);
            }
        }
    }
}
