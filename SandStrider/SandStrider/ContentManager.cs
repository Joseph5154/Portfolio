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
    // Luciano Balsamo
    // Class used to load texture files for each individual class.
    internal class ContentManager
    {
        private Dictionary<string, Texture2D> textures;
        private SpriteFont font;

        private Rectangle[] mummyRectangle;
        private Rectangle[] coinRectangle;
        private Rectangle[] playerRectangle;

        public Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
            set { textures = value; }
        }


        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }


        public Rectangle[] MummyRectangle
        {
            get { return mummyRectangle; }
            set { mummyRectangle = value; }
        }

        public Rectangle[] CoinRectangle
        {
            get { return coinRectangle; }
            set { coinRectangle = value; }
        }


        public Rectangle[] PlayerRectangle
        {
            get { return playerRectangle; }
            set { playerRectangle = value; }
        }


        public ContentManager()
        {
            textures = new Dictionary<string, Texture2D>();

            // Mummy movement animation
            mummyRectangle = new Rectangle[5];
            mummyRectangle[0] = new Rectangle(11, 45, 16, 20);
            mummyRectangle[1] = new Rectangle(42, 45, 16, 20);
            mummyRectangle[2] = new Rectangle(76, 45, 16, 20);
            mummyRectangle[3] = new Rectangle(109, 45, 16, 20);
            mummyRectangle[4] = new Rectangle(140, 45, 16, 20);

            // Coin Spinning Animation
            coinRectangle = new Rectangle[8];
            coinRectangle[0] = new Rectangle(21, 16, 80, 88);
            coinRectangle[1] = new Rectangle(144, 16, 72, 88);
            coinRectangle[2] = new Rectangle(268, 16, 64, 88);
            coinRectangle[3] = new Rectangle(400, 16, 40, 88);
            coinRectangle[4] = new Rectangle(528, 16, 24, 88);
            coinRectangle[5] = new Rectangle(640, 16, 40, 88);
            coinRectangle[6] = new Rectangle(748, 16, 64, 88);
            coinRectangle[7] = new Rectangle(864, 16, 72, 88);

            // Player animations
            playerRectangle = new Rectangle[16];
            playerRectangle[0] = new Rectangle(0, 96, 32, 32);
            playerRectangle[1] = new Rectangle(32, 96, 32, 32);
            playerRectangle[2] = new Rectangle(64, 96, 32, 32);
            playerRectangle[3] = new Rectangle(96, 96, 32, 32);
            playerRectangle[4] = new Rectangle(128, 96, 32, 32);
            playerRectangle[5] = new Rectangle(160, 96, 32, 32);
            playerRectangle[6] = new Rectangle(192, 96, 32, 32);
            playerRectangle[7] = new Rectangle(224, 96, 32, 32);
            playerRectangle[8] = new Rectangle(0, 0, 32, 32);
            playerRectangle[9] = new Rectangle(32, 0, 32, 32);
        }


        public void Add(string name, Texture2D image)
        {
            textures[name] = image;
        }
    }
}
