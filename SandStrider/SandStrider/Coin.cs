using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{
    /// <summary>
    /// Class that represents coins
    /// </summary>
    internal class Coin : GameObject
    {
        //fields
        private int coinAmount;
        private bool active;

        // Animation fields
        private const int thresholdToAnimateCoin = 250;
        private sbyte currentAnimationIndexCoin = 1;
        private float animationTimerCoin = 0;

        /// <summary>
        /// Constructor that initializes all fields
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="coinTexture"></param>
        /// <param name="coinAmount"></param>
        public Coin(int x, int y, int width, int height, Texture2D coinTexture, int coinAmount) : base(x, y, width, height, coinTexture)
        {
            this.coinAmount = coinAmount;
            active = true;
        }

        /// <summary>
        /// Gets or sets the coin amount
        /// </summary>
        public int CoinAmount
        {
            get { return coinAmount; }
            set { coinAmount = value; }
        }

        /// <summary>
        /// Gets or sets the value of active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public Vector2 GetCoinPosition()
        {
            return new Vector2((float)X, (float)Y);
        }

        /// <summary>
        /// A method that has the player collect the coin
        /// </summary>
        /// <param name="player">
        /// The player collecting the object
        /// </param>
        public void Collect(Player player)
        {
            //Add the values of the powerup to the player's stats if they are colliding with the loot
            if (this.CheckIntersect(player))
            {
                player.CoinCount += coinAmount;
                player.TempStats[9]+= coinAmount;

                //Make this object inactive
                this.active = false;
            }
        }

        /// <summary>
        /// Only draws the coin if it is active
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

        public override void Draw(SpriteBatch sb, Texture2D texture, Vector2 coinPos, Rectangle source, Color tint, Single scale)
        {
            if (active)
                base.Draw(sb, texture, coinPos, source, tint, scale);
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                if (animationTimerCoin > thresholdToAnimateCoin)
                {
                    if (currentAnimationIndexCoin >= 7)
                        currentAnimationIndexCoin = -1;

                    currentAnimationIndexCoin++;
                    animationTimerCoin = 0;
                }
                else
                    animationTimerCoin += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }


        public sbyte CoinAnimationIndex
        {
            get { return currentAnimationIndexCoin; }
            set { currentAnimationIndexCoin = value; }
        }
    }
}
