using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{
    public enum ObjectDirection
    {
        Up,
        Down,
        Left,
        Right,
        Idle
    }
    /// <summary>
    /// Class that represents most objects within any given level
    /// </summary>
    internal class GameObject
    {
        protected Rectangle objectBox;
        protected Texture2D objectTexture;

        /// <summary>
        /// Constructor that initializes all values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="objectTexture"></param>
        public GameObject(int x, int y, int width, int height, Texture2D objectTexture)
        {
            this.objectBox = new Rectangle(x, y, width, height);
            this.objectTexture = objectTexture;
        }


        public GameObject(int x, int y)
        {
            this.objectBox = new Rectangle(x, y, 50, 50);
        }


        /// <summary>
        /// Gets or sets the ObjectBox
        /// </summary>
        public Rectangle ObjectBox
        {
            get { return objectBox; }
            set { objectBox = value; }
        }

        /// <summary>
        /// Gets or sets the x value of the object box
        /// </summary>
        public int X
        {
            get { return objectBox.X; }
            set { objectBox.X = value; }
        }

        /// <summary>
        /// Gets or sets the y value of the object box
        /// </summary>
        public int Y
        {
            get { return objectBox.Y; }
            set { objectBox.Y = value; }
        }

        /// <summary>
        /// Gets or sets the texture of the GameObject
        /// </summary>
        public Texture2D ObjectTexture
        {
            get { return objectTexture; }
            set { objectTexture = value; }
        }

        /// <summary>
        /// Draws the sprite in the object rectangle
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public virtual void Draw(SpriteBatch sb, ObjectDirection rotationValue)
        {
            float angleToDrawObjectAs = 0;
            if (rotationValue == ObjectDirection.Up)
                angleToDrawObjectAs = 0;
            if (rotationValue == ObjectDirection.Right)
                angleToDrawObjectAs = 1.5708f;
            if (rotationValue == ObjectDirection.Down)
                angleToDrawObjectAs = 3.14159f;
            if (rotationValue == ObjectDirection.Left)
                angleToDrawObjectAs = 4.71239f;
            
            sb.Draw(objectTexture, objectBox, null, Color.White, angleToDrawObjectAs, new Vector2(0, 0), SpriteEffects.None, 1);
        }

        public virtual void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(objectTexture, ObjectBox, tint);
        }

        /// <summary>
        /// Draws a single frame in an animation
        /// </summary>
        /// <param name="texture">Sprite Sheet as a texture</param>
        /// <param name="position">Current Position of Object</param>
        /// <param name="sourceRectangle">Index of the animation</param>
        /// <param name="tint">Color to draw it as</param>
        /// <param name="scale">Amount to increase or decrease size of portion of texture we are drawing</param>
        public virtual void Draw(SpriteBatch sb, Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color tint, Single scale)
        {
            sb.Draw(texture, position, sourceRectangle, tint, 0, new Vector2(0,0), scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a single frame in an animation with the option for sprite effects
        /// </summary>
        /// <param name="texture">Sprite Sheet as a texture</param>
        /// <param name="position">Current Position of Object</param>
        /// <param name="sourceRectangle">Index of the animation</param>
        /// <param name="effect">effect to apply to object</param>
        /// <param name="tint">Color to draw it as</param>
        /// <param name="scale">Amount to increase or decrease size of portion of texture we are drawing</param>
        public virtual void Draw(SpriteBatch sb, Texture2D texture, Vector2 position, Rectangle sourceRectangle, SpriteEffects effect, Color tint, Single scale)
        {
            sb.Draw(texture, position, sourceRectangle, tint, 0, new Vector2(0, 0), scale, effect, 0);
        }

        /// <summary>
        /// Checks if this object's rectangle intersects another object's rectangle
        /// </summary>
        /// <param name="check">
        /// The other object being checked
        /// </param>
        /// <returns>
        /// Whether or not the two rectangles intersect
        /// </returns>
        public virtual bool CheckIntersect(GameObject check)
        {
            return objectBox.Intersects(check.ObjectBox);
        }
    }
}
