using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandStrider
{
    internal class CustomItem
    {
        private int[] customStats;
        private string customTexturePath;
        private bool hasBeenLoaded;


        public CustomItem()
        {
            customStats = new int[14];
            customTexturePath = "";
            hasBeenLoaded = false;
        }

        public int[] CustomStats
        {
            get { return customStats; }
            set { customStats = value; }
        }

        public string CustomTexturePath
        {
            get { return customTexturePath; }
            set { customTexturePath = value; }
        }

        public bool HasBeenLoaded
        {
            get { return hasBeenLoaded; }
            set { hasBeenLoaded = value; }
        }


        public void LoadFile()
        {
            hasBeenLoaded = true;
            StreamReader sr = new StreamReader("../../../customStarterItem.ssi");
            
            CustomTexturePath = sr.ReadLine();
            for (int i = 0; i < 14; i++)
            {
                customStats[i] = Convert.ToInt32(sr.ReadLine());
            }
            
        }
    }
}
