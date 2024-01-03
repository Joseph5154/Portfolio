using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{
    /// <summary>
    /// Class that represents a projectile that will be used for enemy and player projectiles
    /// </summary>
    internal class Projectile : GameObject
    {
        //fields
        private Vector2 velocity;
        private int attackDamage;
        private GameObject owner;
        private bool active;
        private Color tint;
        private ObjectDirection direction;

        /// <summary>
        /// Gets the projectile's velocity
        /// </summary>
        public Vector2 Velocity { get { return velocity; } }


        public ObjectDirection Direction 
        { 
            get { return direction; } 
            set { direction = value; }
        }

        /// <summary>
        /// Property that gets or sets the x value of the velocity
        /// </summary>
        public int VelocityX
        {
            get
            {
                return (int)velocity.X;
            }
            set
            {
                velocity.X = value;
            }
        }

        /// <summary>
        /// Property that gets or sets the y value of the velocity
        /// </summary>
        public int VelocityY
        {
            get
            {
                return (int)velocity.Y;
            }
            set
            {
                velocity.Y = value;
            }
        }

        /// <summary>
        /// Gets the projectile's attack
        /// </summary>
        public int AttackDamage { get { return attackDamage; } }

        /// <summary>
        /// Gets the owner of the projectile
        /// </summary>
        public GameObject Owner { get { return owner; } }


        /// <summary>
        /// Gets or sets the value of active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Constructor that sets all of the fields of the projectile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="objectTexture"></param>
        /// <param name="velocity"></param>
        /// <param name="attack"></param>
        public Projectile(int x, int y, int width, int height, Texture2D objectTexture, Vector2 velocity, 
            int attackDamage, GameObject owner, Color tint) : base(x, y, width, height, objectTexture)
        {
            this.velocity = velocity;
            this.attackDamage = attackDamage;
            this.owner = owner;
            active = true;
            this.tint = tint;
            direction = ObjectDirection.Idle;
        }

        /// <summary>
        /// Has the projectile move based on its current velocity
        /// </summary>
        public void Move()
        {
            this.X += (int)this.Velocity.X;
            this.Y += (int)this.Velocity.Y;

            // Disables projectiles if they go out of bounds.
            if (X > 750 || X < 0 || Y > 500 || Y < 0)
                Active = false;
        }

        /// <summary>
        /// Checks if the projectile is colliding with an object, and determines what should happen
        /// </summary>
        /// <param name="check">
        /// The object being checked against the projectile
        /// </param>
        public void Attack(GameObject check)
        {
            //Check is the object is intersecting with the object
            if(this.CheckIntersect(check))
            {
                //If the projectile is colliding with the owner of the projectile
                if(check == owner)
                {
                    return;
                }
                //Otherwise, if the object is a player, downcast the object and deal damage to the player and deactivate the projectile
                else if(check is Player)
                {
                    Player player = (Player)check;
                    Random random = new Random();

                    // The player has a chance to dodge.
                    if (random.Next(1, 101) > player.DodgeChance)
                    {
                        // Player's armor reduces damage;
                        attackDamage = (int)((double)attackDamage * (1.0 - player.DamageReduction));
                        player.Health -= attackDamage;
                    }

                    this.active = false;
                }
                //Do the same thing if the object is an enemy
                else if(check is Enemy)
                {
                    Enemy enemy = (Enemy)check;
                    enemy.Health -= attackDamage;
                    this.Active = false;
                }
            }
        }

        /// <summary>
        /// Only draws the projectile if it is active
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public override void Draw(SpriteBatch sb, Color tint)
        {
            if (active)
            {
                if (direction == ObjectDirection.Idle)
                    base.Draw(sb, this.tint);
                else
                    Draw(sb, direction);
            }
        }
    }
}