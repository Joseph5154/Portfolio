using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;

namespace SandStrider
{
    internal class Tile : GameObject
    {
        private bool solid;
        private ObjectDirection orientation;


        public Tile(int x, int y, int width, int height, Texture2D tileTexture, ObjectDirection orientation, bool solid)
            : base(x, y, width, height, tileTexture)
        {
            this.orientation = orientation;
            this.solid = solid;
        }


        public Tile(int x, int y, Texture2D tileTexture, bool solid)
            : base(x, y, 50, 50, tileTexture)
        {
            orientation = ObjectDirection.Up;
            this.solid = solid;
        }


        public ObjectDirection Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }


        public bool Solid
        {
            get { return solid; }
            set { solid = value; }
        }
    }
}
