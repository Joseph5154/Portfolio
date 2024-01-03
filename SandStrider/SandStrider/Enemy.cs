using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{
    /// <summary>
    /// Enum that represents what the enemy is
    /// </summary>
    public enum EnemyType
    {
        Scorpion,
        Mummy,
        Pharaoh
    }

    /// <summary>
    /// Object that represents the enemies of the game
    /// </summary>
    internal class Enemy : GameObject
    {
        //fields
        protected Random random;

        protected EnemyType type;        
        protected double level;
        protected bool active;
        protected int maxHealth;
        protected int health;
        protected int attackDamage;
        protected double timer;
        protected double cooldown;
        protected ObjectDirection currentDirection;

        private const int thresholdToAnimateMummy = 250;
        private byte currentAnimationIndexMummy = 1;
        private byte previousAnimationIndexMummy = 2;
        private float animationTimerMummy = 0;

        protected ContentManager contentManager;


        public Enemy(int x, int y, double level, int enemyType, ContentManager contentManager)
            : base (x, y)
        {
            random = new Random();
            type = GenerateEnemyType(enemyType);
            timer = 0;
            cooldown = 0;
            currentDirection = ObjectDirection.Right;
            this.contentManager = contentManager;

            this.level = level;
            if (level < 0) { level = 0; }

            switch (type)
            {
                case EnemyType.Scorpion:
                    maxHealth = (int)(level * 2) + 5;
                    attackDamage = (int)(level) + 1;
                    health = maxHealth;
                    break;

                case EnemyType.Mummy:
                    maxHealth = (int)(level * random.Next(3, 6)) + 5;
                    attackDamage = (int)(level * 2.5) + 1;
                    health = maxHealth;
                    break;

                case EnemyType.Pharaoh:
                    maxHealth = (int)(level * random.Next(3, 4)) + 5;
                    attackDamage = (int)(level * 5) + random.Next(1, 4);
                    health = maxHealth;
                    break;
            }
            active = true;
        }


        public EnemyType Type
        {
            get {  return type; }
            set { type = value; }
        }


        /// <summary>
        /// Gets or sets the enemy's health value
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// Gets or sets the enemy's Maximum health value
        /// </summary>
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// Gets or sets the enemy's attack damage
        /// </summary>
        public int AttackDamage
        {
            get { return attackDamage; }
            set { attackDamage = value; }
        }


        public byte MummyAnimationIndex
        {
            get { return currentAnimationIndexMummy; }
            set { currentAnimationIndexMummy = value; }
        }


        public Vector2 GetEnemyPosition()
        {
            return new Vector2((float)X, (float)Y);
        }


        public EnemyType GenerateEnemyType(int num)
        {
            if (num == 0)
                return EnemyType.Mummy;
            else if (num == 1)
                return EnemyType.Scorpion;
            else
                return EnemyType.Pharaoh;
        }

        /// <summary>
        /// Has the enemy move and shoot based on their type
        /// </summary>
        public void Move(GameObject player, GameTime gameTime, Texture2D texture, List<Projectile> projectiles)
        {
            //If the enemy is a mummy, then have it move for 3 seconds, pause, and shoot for two seconds
            if(type == EnemyType.Pharaoh)
            {
                //If it has not yet been three seconds, then have the enemy move towards the player
                if (timer <= 3)
                {
                    //If the x value of the enemy is greater than the player's, then have it move left
                    if (this.X > player.X && X > 50)
                    {
                        this.X -= 2;
                        this.currentDirection = ObjectDirection.Left;
                    }
                    //If the x value of the enemy is less than the player's, then have it move right
                    if (this.X < player.X && X < 675)
                    {
                        this.X += 2;
                        this.currentDirection = ObjectDirection.Right;
                    }
                    //If the y value of the enemy is greater than the player's, then have it move up
                    if (this.Y > player.Y && Y > 75)
                    {
                        this.Y -= 2;
                        this.currentDirection = ObjectDirection.Up;
                    }
                    //If the y value of the enemy is less than the player's, then have it move down
                    if (this.Y < player.Y && Y < 410)
                    {
                        this.Y += 2;
                        this.CurrentDirection = ObjectDirection.Down;
                    }
                }
                //Otherwise, have the enemy pause (will later be the time at which the enemy attacks)
                else
                {
                    //subtract the game time from the cooldown, and whenever cooldown is less than or equal to 0, have the enemy shoot
                    cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

                    if (cooldown <= 0)
                    {
                        Shoot(texture, projectiles, currentDirection);
                        //Reset the cooldown to 1
                        cooldown = 1;
                    }

                    //If 5 seconds have passed, then reset the timer
                    if (timer >= 5)
                    {
                        timer = 0;
                    }
                }
            }
            //If the enemy is a scorpion, then have it move up and down and shoot left or right every second
            else if(type == EnemyType.Scorpion)
            {
                //Move the enemy up or down towards the player
                if(this.Y < player.Y && Y < 410)
                {
                    this.Y += 4;
                }
                if (this.Y > player.Y && Y > 75)
                {
                    this.Y -= 4;
                }
                //Change the direction of the enemy based on the player's x
                if (this.X < player.X)
                {
                    currentDirection = ObjectDirection.Right;
                }
                if(this.X > player.X)
                {
                    currentDirection = ObjectDirection.Left;
                }
                if(cooldown <= 0)
                {
                    Shoot(texture, projectiles, currentDirection);
                    cooldown = 0.5;
                }
            }
            //Otherwise, do the inverse of what the scorpion does (We can change this AI later, I just couldn't come up with anything better)
            else
            {
                //Move the enemy up or down towards the player
                if (this.X < player.X && X < 675)
                {
                    this.X += 4;
                }
                if (this.X > player.X && X > 50)
                {
                    this.X -= 4;
                }
                //Change the direction of the enemy based on the player's x
                if (this.Y < player.Y)
                {
                    currentDirection = ObjectDirection.Down;
                }
                if (this.Y > player.Y)
                {
                    currentDirection = ObjectDirection.Up;
                }
                if (cooldown <= 0)
                {
                    Shoot(texture, projectiles, currentDirection);
                    cooldown = 0.5;
                }
            }
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }


        public ObjectDirection CurrentDirection
        {
            get { return currentDirection; }
            set { currentDirection = value; }
        }


        /// <summary>
        /// Creates a new projectile with the specified texture and add it to a list
        /// </summary>
        /// <param name="texture">
        /// The texture of the projectile
        /// </param>
        /// <param name="projectiles">
        /// The list that the projectile will be added to
        /// </param>
        public void Shoot(Texture2D texture, List<Projectile> projectiles, ObjectDirection face)
        {
            Projectile proj;
            if (face == ObjectDirection.Left)
                proj = new Projectile(X, Y, 20, 20, texture, new Vector2(-2, 0), attackDamage, this, Color.White);
            else if(face == ObjectDirection.Up)
            {
                proj = new Projectile(X, Y, 20, 20, texture, new Vector2(0, -2), attackDamage, this, Color.White);
            }
            else if(face == ObjectDirection.Down)
            {
                proj = new Projectile(X, Y, 20, 20, texture, new Vector2(0, 2), attackDamage, this, Color.White);
            }
            else
                proj = new Projectile(X, Y, 20, 20, texture, new Vector2(2, 0), attackDamage, this, Color.White);
            projectiles.Add(proj);
        }

        /// <summary>
        /// Gets or sets the value of active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Sets the active parameter to false in case of being killed
        /// </summary>
        public void Die()
        {
            active = false;
        }

        /// <summary>
        /// Only draws the loot if it is active
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public override void Draw(SpriteBatch sb, Color tint)
        {
            if (active)
            {
                base.Draw(sb, tint);
            }
        }

        public override void Draw(SpriteBatch sb, Texture2D texture, Vector2 enemyPos, Rectangle source, Color tint, Single scale)
        {
            if (active)
                base.Draw(sb, texture, enemyPos, source, tint, scale);
        }

        public void Update(GameTime gameTime)
        {
            // Animation Logic
            if (animationTimerMummy > thresholdToAnimateMummy)
            {
                if (currentAnimationIndexMummy == 1)
                {
                    if (previousAnimationIndexMummy == 0)
                        currentAnimationIndexMummy = 2;
                    else
                        currentAnimationIndexMummy = 0;
                    previousAnimationIndexMummy = currentAnimationIndexMummy;
                }
                else
                    currentAnimationIndexMummy = 1;
                animationTimerMummy = 0;
            }
            else
                animationTimerMummy += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}