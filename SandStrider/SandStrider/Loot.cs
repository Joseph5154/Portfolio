using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{

    /// <summary>
    /// Represents the items picked up that either restore health or buff the player's stats
    /// </summary>
    internal class Loot : GameObject
    {
        //fields
        private int heal;
        private bool active;
        private CustomItem custom;

        /// <summary>
        /// Constructor that calls the base GameObject constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="objectTexture"></param>
        public Loot(int x, int y, int width, int height, Texture2D objectTexture, int heal) 
            : base(x, y, width, height, objectTexture)
        {
            this.heal = heal;
            active = true;
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
        /// A method that has the player collect the object
        /// </summary>
        /// <param name="player">
        /// The player collecting the object
        /// </param>
        public void Collect(Player player)
        {
            //Add the values of the powerup to the player's stats if they are colliding with the loot
            if(this.CheckIntersect(player))
            {
                if (objectTexture.Name == "herb")
                {
                    player.TempStats[0]++;
                    player.MaxHealth += 5;
                }
                else if (objectTexture.Name == "quiver")
                {
                    player.TempStats[1]++;
                    player.AttackDamage++;
                }
                else if (objectTexture.Name == "ringPickup")
                {
                    player.TempStats[2]++;
                    player.AttackSpeed++;
                }
                else if (objectTexture.Name == "necklacePickup")
                {
                    player.TempStats[3]++;
                    player.MovementSpeed++;
                }
                else if (objectTexture.Name == "armGuard")
                {
                    player.TempStats[4]++;
                    player.CriticalDamage += 0.1;
                }
                else if (objectTexture.Name == "feather")
                {
                    player.TempStats[5]++;
                    player.ArrowSpeed++;
                }
                else if (objectTexture.Name == "crossbow")
                {
                    player.PermStats[10]++;
                    player.Bolts++;
                }
                else if (objectTexture.Name == "ankh")
                {
                    player.PermStats[12]++;
                    player.AnkhUnlocked = true;
                }
                // Custom item
                else if (objectTexture.Name != "potionPickup")
                {
                    custom = new CustomItem();
                    custom.LoadFile();

                    player.MaxHealth += custom.CustomStats[0];
                    player.Health += custom.CustomStats[0];
                    player.AttackDamage += custom.CustomStats[1];

                    player.AttackSpeed += custom.CustomStats[2];
                    if (player.AttackSpeed > 10) player.AttackSpeed = 10;

                    player.MovementSpeed += custom.CustomStats[3];
                    if (player.MovementSpeed > 8) player.MovementSpeed = 8;

                    player.CriticalDamage += (double)custom.CustomStats[4] / 10;
                    if (player.CriticalDamage > 5) player.CriticalDamage = 5;

                    player.ArrowSpeed += custom.CustomStats[5];
                    if (player.ArrowSpeed > 13) player.ArrowSpeed = 13;

                    player.DodgeChance += custom.CustomStats[6];
                    if (player.DodgeChance > 75) player.DodgeChance = 75;

                    player.DamageReduction += custom.CustomStats[7];
                    if (player.DamageReduction > 75) player.DamageReduction = 75;

                    player.RoomCount += custom.CustomStats[8];
                    player.TempStats[9] += custom.CustomStats[9];

                    player.Bolts += custom.CustomStats[10];
                    if (player.Bolts > 7) player.Bolts = 7;

                    if (custom.CustomStats[11] == 1) player.SwordUnlocked = true;
                    if (custom.CustomStats[12] == 1) player.AnkhUnlocked = true;
                    if (custom.CustomStats[13] == 1) player.BookUnlocked = true;
                }


                //If the player's health goes above its max, then set the players health back to max.
                player.Health += heal;
                if (player.Health > player.MaxHealth)
                {
                    player.Health = player.MaxHealth;
                }

                //Make this object inactive
                this.active = false;
            }
        }

        /// <summary>
        /// Only draws the loot if it is active
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public override void Draw(SpriteBatch sb, Color tint)
        {
            if(active)
            {
                base.Draw(sb, tint);
            }
        }
    }
}
