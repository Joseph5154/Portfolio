using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace SandStrider
{
    public enum PlayerState
    {
        walk,
        attack,
        idle
    }

    /// <summary>
    /// Class that represents the player of the game.
    /// Also deals with the slider bar.
    /// </summary>
    internal class Player : GameObject
    {
        // Loot-related stats
        private int maxHealth;
        private int attackDamage;
        private int attackSpeed;
        private int movementSpeed;
        private double criticalDamage;
        private int arrowSpeed;
        private int dodgeChance;
        private int damageReduction;

        private int roomCount;
        private int coinCount;

        private int bolts;
        private bool swordUnlocked;
        private bool ankhUnlocked;
        private bool bookUnlocked;

        private int[] permStats;
        private int[] tempStats;

        // Essential stats
        private int health;
        private double healthRatio;
        private int souls;
        private double soulRatio;
        private float attackCooldown;
        private float attackRecharge;
        private bool swingingSword;
        private bool hasExtracted;
        private bool dead;

        // Misc
        private Rectangle collisionBox;
        private ObjectDirection playerRotation;
        private ContentManager contentManager;
        private List<Projectile> playerProjectiles;
        private string[] statsDescriptionArray;

        // Slider related Fields
        private Vector2 pointerVelocity;
        private Rectangle pointerPosition;

        // Animation fields
        private sbyte playerAnimationIndex;
        private float animationTimerPlayer;
        private PlayerState playerState;
        private float swordAnimationTimer;

        /// <summary>
        /// Constructor that initializes all fields
        /// </summary>
        /// <param name="x">Player X Pos, pixels from left of screen</param>
        /// <param name="y">Player Y Pos, pixels from top of screen</param>
        public Player(int x, int y, ContentManager contentManager) : base(x, y)
        {
            maxHealth = 30;
            attackDamage = 3;
            attackSpeed = 0;
            movementSpeed = 3;
            criticalDamage = 1.5;
            arrowSpeed = 3;
            dodgeChance = 0;
            damageReduction = 0;

            roomCount = 0;
            coinCount = 0;

            bolts = 0;
            swordUnlocked = false;
            ankhUnlocked = false;
            bookUnlocked = false;

            tempStats = new int[10];
            permStats = new int[14];
            LoadStats();

            health = maxHealth;
            healthRatio = (double)health / maxHealth;
            souls = 0;
            soulRatio = (double)souls / (double)25;
            attackCooldown = 1500 - (attackSpeed * 100);
            attackRecharge = 0;
            swingingSword = false;
            hasExtracted = false;
            dead = false;

            collisionBox = new Rectangle(objectBox.X, objectBox.Y + (objectBox.Height / 2), objectBox.Width - 20, 20);
            playerRotation = ObjectDirection.Up;
            this.contentManager = contentManager;
            playerProjectiles = new List<Projectile>();
            FillStatsDescriptionArray();

            // How fast the pointer moves up and down
            pointerVelocity = new Vector2(0, 5);
            pointerPosition = new Rectangle(900, 350, contentManager.Textures["pointerTexture"].Width,
                contentManager.Textures["pointerTexture"].Height);

            playerAnimationIndex = 0;
            animationTimerPlayer = 0;
            playerState = PlayerState.idle;
            swordAnimationTimer = 0;
        }

        // Geters and setters for all player stats.

        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
        public int AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
        public int MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
        public double CriticalDamage { get { return criticalDamage; } set { criticalDamage = value; } }
        public int ArrowSpeed { get { return arrowSpeed; } set { arrowSpeed = value; } }
        public int DodgeChance { get { return dodgeChance; } set { dodgeChance = value; } }
        public int DamageReduction { get { return damageReduction; } set { damageReduction = value; } }
        public int RoomCount { get { return roomCount; } set { roomCount = value; } }
        public int CoinCount { get { return coinCount; } set { coinCount = value; } }
        public int Bolts { get { return bolts; } set { bolts = value; } }
        public bool SwordUnlocked { get { return swordUnlocked; } set { swordUnlocked = value; } }
        public bool AnkhUnlocked { get { return ankhUnlocked; } set { ankhUnlocked = value; } }
        public bool BookUnlocked { get { return bookUnlocked; } set { bookUnlocked = value; } }
        public int[] TempStats { get { return tempStats; } set { tempStats = value; } }
        public int[] PermStats { get { return permStats; } set { permStats = value; } }
        public int Health { get { return health; } set { health = value; } }
        public int Souls { get { return souls; } set { souls = value; } }
        public bool SwingingSword { get { return swingingSword; } set { swingingSword = value; } }
        public bool HasExtracted { get { return hasExtracted; } set { hasExtracted = value; } }
        public bool Dead { get { return dead; } set { dead = value; } }
        public Rectangle CollisionBox { get { return collisionBox; } set { collisionBox = value; } }
        public ObjectDirection PlayerRotation { get { return playerRotation; } set {playerRotation = value; } }
        public List<Projectile> PlayerProjectiles { get { return playerProjectiles; } set { playerProjectiles = value; } }


        /// <summary>
        /// Creates a new projectile with the specified texture and add it to a list
        /// </summary>
        /// <param name="texture">
        /// The texture of the projectile
        /// </param>
        /// <param name="projectiles">
        /// The list that the projectile will be added to
        /// </param>
        public void Shoot(Texture2D texture, List<Projectile> projectiles, int sliderModifier, Color tint)
        {
            //Create a projectile object with no velocity
            Projectile proj = new Projectile(X, Y, 40, 20, texture, new Vector2(0, 0), attackDamage + sliderModifier, this, tint);
            
            // Tracks which way the current projectiles are going for use with the bolts stat.
            bool[] projectileDirections = new bool[8]
            {
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false
            };

            //Change the object's velocity to move at the player's attack speed in the direction that the player is facing
            switch (playerRotation)
            {
                case ObjectDirection.Left:
                    proj.VelocityX -= arrowSpeed;
                    proj.Direction = ObjectDirection.Down;
                    proj.Y += 30;
                    proj.X += 20;
                    projectileDirections[0] = true;
                    break;

                case ObjectDirection.Right:
                    proj.VelocityX += arrowSpeed;
                    proj.Direction = ObjectDirection.Up;
                    proj.Y += 10;
                    projectileDirections[2] = true;
                    break;

                case ObjectDirection.Up:
                    proj.VelocityY -= arrowSpeed;
                    proj.Direction = ObjectDirection.Left;
                    proj.X += 20;
                    projectileDirections[4] = true;
                    break;

                case ObjectDirection.Down:
                    proj.VelocityY += arrowSpeed;
                    proj.Direction = ObjectDirection.Right;
                    proj.X += 40;
                    projectileDirections[6] = true;
                    break;
            }

            // If the player has the bolts stat, they fire more projectiles at once.
            List<Projectile> boltArrows = new List<Projectile>(bolts);
            Random random = new Random();

            for (int i = 0; i < bolts; i++)
            {
                int arrowDirection = random.Next(0, 8);
                while (projectileDirections[arrowDirection])
                    arrowDirection = random.Next(0, 8);
                boltArrows.Add(new Projectile(X, Y, 40, 20, texture, new Vector2(0, 0), attackDamage + sliderModifier, this, tint));

                switch (arrowDirection)
                {
                    case 0:
                        boltArrows[i].VelocityX -= arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Down;
                        boltArrows[i].Y += 30;
                        boltArrows[i].X += 20;
                        projectileDirections[0] = true;
                        break;

                    case 1:
                        boltArrows[i].VelocityY += arrowSpeed;
                        boltArrows[i].VelocityX -= arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Right;
                        boltArrows[i].X += 40;
                        projectileDirections[1] = true;
                        break;

                    case 2:
                        boltArrows[i].VelocityX += arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Up;
                        boltArrows[i].Y += 10;
                        projectileDirections[2] = true;
                        break;

                    case 3:
                        boltArrows[i].VelocityY -= arrowSpeed;
                        boltArrows[i].VelocityX += arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Left;
                        boltArrows[i].X += 40;
                        projectileDirections[3] = true;
                        break;

                    case 4:
                        boltArrows[i].VelocityY -= arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Left;
                        boltArrows[i].X += 20;
                        projectileDirections[4] = true;
                        break;

                    case 5:
                        boltArrows[i].VelocityY -= arrowSpeed;
                        boltArrows[i].VelocityX -= arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Left;
                        boltArrows[i].X += 20;
                        projectileDirections[5] = true;
                        break;

                    case 6:
                        boltArrows[i].VelocityY += arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Right;
                        boltArrows[i].X += 40;
                        projectileDirections[6] = true;
                        break;

                    case 7:
                        boltArrows[i].VelocityY += arrowSpeed;
                        boltArrows[i].VelocityX += arrowSpeed;
                        boltArrows[i].Direction = ObjectDirection.Right;
                        boltArrows[i].X += 40;
                        projectileDirections[7] = true;
                        break;
                }
            }


            //Add the projectile to the list
            projectiles.Add(proj);

            if (boltArrows.Count > 0)
                projectiles.AddRange(boltArrows);
        }


        public void Update(GameTime gameTime, KeyboardState kb)
        {
            
                if (kb.IsKeyDown(Keys.A))
                    X -= movementSpeed;

            
                if (kb.IsKeyDown(Keys.D))
                    X += movementSpeed;

            
                if (kb.IsKeyDown(Keys.W))
                    Y -= movementSpeed;

            
                if (kb.IsKeyDown(Keys.S))
                    Y += movementSpeed;
            

            if (kb.IsKeyDown(Keys.Down))
                PlayerRotation = ObjectDirection.Down;
            if (kb.IsKeyDown(Keys.Up))
                PlayerRotation = ObjectDirection.Up;
            if (kb.IsKeyDown(Keys.Left))
                PlayerRotation = ObjectDirection.Left;
            if (kb.IsKeyDown(Keys.Right))
                PlayerRotation = ObjectDirection.Right;
            if (kb.IsKeyDown(Keys.A))
                playerRotation = ObjectDirection.Left;
            if (kb.IsKeyDown(Keys.D))
                playerRotation = ObjectDirection.Right;
            if (kb.IsKeyDown(Keys.W))
                playerRotation = ObjectDirection.Up;
            else if (kb.IsKeyDown(Keys.S))
                playerRotation = ObjectDirection.Down;


            // Keeps track of if the player is moving for animation.
            if ((kb.IsKeyUp(Keys.W) && kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.S) && kb.IsKeyUp(Keys.D)))
                playerState = PlayerState.idle;
            else
                playerState = PlayerState.walk;

            // Uses player state to animate the player.
            if (playerState == PlayerState.walk)
            {
                if (animationTimerPlayer > 150)
                {
                    if (playerAnimationIndex >= 7)
                        playerAnimationIndex = -1;

                    playerAnimationIndex++;
                    animationTimerPlayer = 0;
                }
                else
                    animationTimerPlayer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (playerState == PlayerState.idle)
            {
                if (animationTimerPlayer > 500)
                {
                    if (playerAnimationIndex == 9)
                        playerAnimationIndex = 8;
                    else
                        playerAnimationIndex = 9;

                    animationTimerPlayer = 0;
                }
                else
                    animationTimerPlayer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

                // Shooting attack
                if ((kb.IsKeyDown(Keys.LeftControl) || kb.IsKeyDown(Keys.E)) && attackRecharge >= attackCooldown)
                {
                    //Depending on the postion the point, change the slider modifer during the shoot method
                    if (pointerPosition.Y < 180 || pointerPosition.Y > 390)
                    {
                        Shoot(contentManager.Textures["arrow"], playerProjectiles, 0, Color.White);
                    }
                    else if (pointerPosition.Y < 250 || pointerPosition.Y > 325)
                    {
                        Shoot(contentManager.Textures["arrow"], playerProjectiles, attackDamage / 4, Color.White);
                    }
                    else
                    {
                        Shoot(contentManager.Textures["powerArrow"], playerProjectiles, (int)(attackDamage * (criticalDamage - 1)), Color.White);
                    }
                    attackRecharge = 0;
                }

            // Sword attack
            if (swordUnlocked && (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.Q)) && attackRecharge >= attackCooldown)
            {
                swingingSword = true;
                attackRecharge = 0;
            }

            if (swingingSword)
                swordAnimationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (swordAnimationTimer > 500)
            {
                swingingSword = false;
                swordAnimationTimer = 0;
            }

            // Increment the attack cooldown by the elapsed time since last frame
            if (attackRecharge < attackCooldown)
                attackRecharge += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Switch directions of pointer, in here so position is saved while not in this state.
            // First part: Above Slider, Second Part: Below Texture;
            if (pointerPosition.Y <= 100 || pointerPosition.Y >= 425)
                pointerVelocity *= -1;

            pointerPosition.Y += (int)pointerVelocity.Y;

            collisionBox.X = X + 20;
            collisionBox.Y = Y + 50;

            healthRatio = (double)health / maxHealth;
            soulRatio = (double)souls / (double)25;
            coinCount = tempStats[9] + permStats[9];
            attackCooldown = 1500 - (attackSpeed * 100);

            if (health <= 0)
            {
                if (souls >= 25)
                {
                    health = maxHealth;
                    souls = 0;
                }
                else
                    dead = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            // Player Drawing 
            Vector2 playerStatPosition = new Vector2(X, Y + 75); // Pos needs revision
            SpriteEffects shouldFlip = SpriteEffects.None;

            if (playerRotation == ObjectDirection.Left)
                shouldFlip = SpriteEffects.FlipHorizontally;
            else if (playerRotation == ObjectDirection.Right)
                shouldFlip = SpriteEffects.None;

            Draw(sb, contentManager.Textures["playerTexture"], new Vector2(X, Y),
                        contentManager.PlayerRectangle[playerAnimationIndex], shouldFlip, Color.White, 2);

            // Drawing of stats text under player.
            //sb.DrawString(contentManager.Font, $"Health: {Health}\nAttack Damage: {AttackDamage}" +
            //    $"\nMovement Speed: {MovementSpeed}\n Attack Speed: {AttackSpeed} \n Coin Count: {coinCount}", playerStatPosition,
            //    Color.Black);

            // Projectiles
            for (int i = 0; i < playerProjectiles.Count; i++)
            {
                if (playerProjectiles[i].Active)
                {
                    playerProjectiles[i].Draw(sb, Color.White);
                }
            }

            // Sword
            if (swingingSword)
            {
                sb.Draw(contentManager.Textures["sword"], new Rectangle(collisionBox.X + 10, collisionBox.Y - 10, 50, 50),
                    null, Color.White, (float)(animationTimerPlayer / (20 * Math.PI)), new Vector2(0, 0), SpriteEffects.None, 1);
            }

            // Player stats and background
            sb.Draw(contentManager.Textures["playerBackground"], new Rectangle(750, 0, 250, 500), Color.White);

            if (tempStats[8] < 1)
            {
                // Slider damage values, only drawn in first room.
                sb.DrawString(contentManager.Font, "Damage modifers:", new Vector2(790, 100), Color.White);
                sb.DrawString(contentManager.Font, "1x", new Vector2(820, 150), Color.White);
                sb.DrawString(contentManager.Font, "1x", new Vector2(820, 410), Color.White);
                sb.DrawString(contentManager.Font, "1.25x", new Vector2(790, 215), Color.White);
                sb.DrawString(contentManager.Font, "1.25x", new Vector2(790, 355), Color.White);
                sb.DrawString(contentManager.Font, criticalDamage + "x", new Vector2(800, 285), Color.White);
            }

            sb.DrawString(contentManager.Font, $"{coinCount}", new Vector2(892, 77), Color.White);

            // Health bar area
            Rectangle healthBar = new Rectangle(842, 33, contentManager.Textures["health100"].Width * 3 - 14, contentManager.Textures["health100"].Height * 3 + 1);

            // Health bar drawing
            if (healthRatio >= 1) sb.Draw(contentManager.Textures["health100"], healthBar, Color.White);
            else if (healthRatio >= 0.9) sb.Draw(contentManager.Textures["health90"], healthBar, Color.White);
            else if (healthRatio >= 0.8) sb.Draw(contentManager.Textures["health80"], healthBar, Color.White);
            else if (healthRatio >= 0.7) sb.Draw(contentManager.Textures["health70"], healthBar, Color.White);
            else if (healthRatio >= 0.6) sb.Draw(contentManager.Textures["health60"], healthBar, Color.White);
            else if (healthRatio >= 0.5) sb.Draw(contentManager.Textures["health50"], healthBar, Color.White);
            else if (healthRatio >= 0.4) sb.Draw(contentManager.Textures["health40"], healthBar, Color.White);
            else if (healthRatio >= 0.3) sb.Draw(contentManager.Textures["health30"], healthBar, Color.White);
            else if (healthRatio >= 0.2) sb.Draw(contentManager.Textures["health20"], healthBar, Color.White);
            else if (healthRatio >= 0.1) sb.Draw(contentManager.Textures["health10"], healthBar, Color.White);
            else if (healthRatio > 0) { }
            else if (healthRatio <= 0) sb.Draw(contentManager.Textures["health0"], healthBar, Color.White);

            // Soul bar area
            Rectangle soulBar = new Rectangle(842, 53, contentManager.Textures["soul100"].Width * 3 - 14, contentManager.Textures["soul100"].Height * 3 + 1);

            // Soul bar drawing
            if (soulRatio >= 1) sb.Draw(contentManager.Textures["soul100"], soulBar, Color.White);
            else if (soulRatio >= 0.9) sb.Draw(contentManager.Textures["soul90"], soulBar, Color.White);
            else if (soulRatio >= 0.8) sb.Draw(contentManager.Textures["soul80"], soulBar, Color.White);
            else if (soulRatio >= 0.7) sb.Draw(contentManager.Textures["soul70"], soulBar, Color.White);
            else if (soulRatio >= 0.6) sb.Draw(contentManager.Textures["soul60"], soulBar, Color.White);
            else if (soulRatio >= 0.5) sb.Draw(contentManager.Textures["soul50"], soulBar, Color.White);
            else if (soulRatio >= 0.4) sb.Draw(contentManager.Textures["soul40"], soulBar, Color.White);
            else if (soulRatio >= 0.3) sb.Draw(contentManager.Textures["soul30"], soulBar, Color.White);
            else if (soulRatio >= 0.2) sb.Draw(contentManager.Textures["soul20"], soulBar, Color.White);
            else if (soulRatio >= 0.1) sb.Draw(contentManager.Textures["soul10"], soulBar, Color.White);

            // Sliding Bar Mechanic
            sb.Draw(contentManager.Textures["sliderTexture"],
                new Vector2(850, 125),
                Color.White);
            sb.Draw(contentManager.Textures["pointerTexture"],
                pointerPosition,
                Color.White);
        }


        public void Extract()
        {
            permStats[10] = bolts;

            if (swordUnlocked)
                permStats[11] = 1;

            if (ankhUnlocked)
                permStats[12] = 1;

            if (bookUnlocked)
                permStats[13] = 1;

            for (int i = 0; i < tempStats.Length; i++)
            {
                permStats[i] += tempStats[i];
                tempStats[i] = 0;
            }

            SaveStats();
            hasExtracted = true;
        }


        public void SaveStats()
        {
            StreamWriter sw = new StreamWriter("../../../playerStats.txt");
            sw.WriteLine(statsDescriptionArray[0]);
            sw.WriteLine(permStats[0]);

            for (int i = 1; i < permStats.Length; i++)
            {
                sw.WriteLine();
                sw.WriteLine(statsDescriptionArray[i]);
                sw.WriteLine(permStats[i]);
            }

            sw.Close();
        }

        public void LoadStats()
        {
            StreamReader sr = new StreamReader("../../../playerStats.txt");
            sr.ReadLine();
            permStats[0] = Convert.ToInt32(sr.ReadLine());


            for (int i = 1; i < permStats.Length; i++)
            {
                sr.ReadLine();
                sr.ReadLine();
                permStats[i] = Convert.ToInt32(sr.ReadLine());
            }

            maxHealth = 30 + (permStats[0] * 5);
            attackDamage = 3 + permStats[1];
            attackSpeed = permStats[2];
            movementSpeed = 3 + permStats[3];
            criticalDamage = 1.5 + ((double)permStats[4] / 10);
            arrowSpeed = 3 + permStats[5];
            dodgeChance = permStats[6] * 5;
            damageReduction = permStats[7] * 5;
            roomCount = permStats[8];
            coinCount = permStats[9];
            bolts = permStats[10];

            if (permStats[11] > 0) swordUnlocked = true;
            else swordUnlocked = false;

            if (permStats[12] > 0) ankhUnlocked = true;
            else ankhUnlocked = false;

            if (permStats[13] > 0) bookUnlocked = true;
            else bookUnlocked = false;

            sr.Close();
        }

        public void ResetStats()
        {
            StreamWriter sw = new StreamWriter("../../../playerStats.txt");
            sw.WriteLine(statsDescriptionArray[0]);
            sw.WriteLine(0);

            for (int i = 1; i < permStats.Length; i++)
            {
                sw.WriteLine();
                sw.WriteLine(statsDescriptionArray[i]);
                sw.WriteLine(0);
            }

            sw.Close();
            LoadStats();
        }

        public void FillStatsDescriptionArray()
        {
            statsDescriptionArray = new string[14]
            {
                "// Herbs [+5 to Max Health]",
                "// Quivers [+1 to Attack Damage]",
                "// Rings [-0.1 Seconds to Attack Cooldown; Max = 10]",
                "// Necklaces [+1 to Player Movement Speed; Max = 5]",
                "// Bracers [+0.1x to Critical Damage Multiplier; Max = 35]",
                "// Feathers [+1 to Arrow Movement Speed; Max = 10]",
                "// LeatherArmor [+5% to Dodge Chance; Max = 15]",
                "// IronArmor [+5% to Damage Reduction; Max = 15]",
                "// RoomCount [Difficulty Scaling]",
                "// CointCount []",
                "// Bolts [+1 to Arrows Fired per Attack; Max 7]",
                "// SwordUnlocked [Unlocks Melee Attack with 'e'; Max 1]",
                "// AnkhUnlocked [Prevents Death if Soul Bar is Full; Max 1]",
                "// BookUnlocked [Teleports Player to Recent Shop with 't'; Max 1]",
            };
        }
    }
}