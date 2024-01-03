using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{
    public enum RoomType
    {
        Start,
        Passive,
        Shop,
        Extract,
        Hostile,
        Boss
    }

    // Luciano Balsamo
    // This class is used to create a new object for each individual dungeon room.
    // Within each object the data of the room is saved.
    internal class Room
    {
        private RoomType roomType;
        private double roomLevel;
        private Random random;
        private Player player;
        private bool clear;

        private int roomX;
        private int roomY;

        // Shop fields.
        private bool leatherBought = false;
        private bool ironBought = false;
        private bool boltBought = false;

        private List<Enemy> enemyList;
        private List<Coin> coinList;
        private List<Projectile> enemyProjectiles;
        private List<Loot> lootList;

        private Tile[,] tileArray;
        private List<Tile> debris;

        private ContentManager contentManager;
        private ScreenManager screenManager;


        public Room(RoomType roomType, Tile[,] tileArray, double roomLevel, int roomX, int roomY, Player player, ContentManager contentManager, ScreenManager screenManager)
        {
            this.roomType = roomType;
            this.tileArray = tileArray;
            this.roomLevel = roomLevel;
            this.player = player;

            random = new Random();
            clear = false;
            debris = new List<Tile>();

            this.roomX = roomX;
            this.roomY = roomY;

            enemyList = new List<Enemy>();
            coinList = new List<Coin>();
            enemyProjectiles = new List<Projectile>();
            lootList = new List<Loot>();

            this.contentManager = contentManager;
            this.screenManager = screenManager;
        }

        /// <summary>
        /// Gets the room type, which determines what spawns/exists in the room.
        /// Cannot be changed after room creation.
        /// </summary>
        public RoomType RoomType { get { return roomType; } }

        /// <summary>
        /// Gets the difficulty level of the room, which determines the stats/amount of enemies.
        /// Cannot be changed after room creation.
        /// </summary>
        public double RoomLevel { get { return roomLevel; } }

        /// <summary>
        /// Gets the room's grid X location for use in the RoomManager.
        /// Cannot be changed after room creation.
        /// </summary>
        public int RoomX { get { return roomX; } }

        /// <summary>
        /// Gets the room's grid Y location for use in the RoomManager.
        /// Cannot be changed after room creation.
        /// </summary>
        public int RoomY { get { return roomY; } }

        /// <summary>
        /// Gets the list of enemies to be passed into Game1
        /// </summary>
        public List<Enemy> EnemyList { get { return enemyList; } }

        /// <summary>
        /// Getter/setter for if the room has been cleared of enemies.
        /// </summary>
        public bool Clear
        {
            get { return clear; }
            set { clear = value; }
        }


        public List<Projectile> EnemyProjectiles
        {
            get { return enemyProjectiles; }
            set { enemyProjectiles = value; }
        }

        // Tracks if the player has bought a specific item in a specific shop.
        public bool LeatherBought { get { return leatherBought; } set {  leatherBought = value; } }
        public bool IronBought { get { return ironBought; } set {  ironBought = value; } }
        public bool BoltBought { get { return boltBought; } set {  boltBought = value; } }


        public void RoomGeneration(CustomItem customItem)
        {
            switch (roomType)
            {
                case RoomType.Start:
                    clear = true;
                    if (customItem != null)
                    {
                        lootList.Add(new Loot(250, 150, 50, 50, contentManager.Textures["customItem"], 5));
                    }
                    break;

                case RoomType.Passive:
                    // Summon health potion
                    lootList.Add(new Loot(350, 245, 50, 50, contentManager.Textures["potion"], 100));
                    clear = true;
                    break;

                case RoomType.Shop:
                    clear = true;
                    debris.Add(new Tile(325, 220, 100, 100, contentManager.Textures["camel"], ObjectDirection.Up, false));
                    debris.Add(new Tile(253, 151, 40, 40, contentManager.Textures["shopRock"], ObjectDirection.Up, false));
                    debris.Add(new Tile(453, 151, 40, 40, contentManager.Textures["shopRock"], ObjectDirection.Up, false));
                    debris.Add(new Tile(203, 251, 40, 40, contentManager.Textures["shopRock"], ObjectDirection.Up, false));
                    debris.Add(new Tile(503, 251, 40, 40, contentManager.Textures["shopRock"], ObjectDirection.Up, false));
                    debris.Add(new Tile(253, 351, 40, 40, contentManager.Textures["shopRock"], ObjectDirection.Up, false));
                    debris.Add(new Tile(453, 351, 40, 40, contentManager.Textures["shopRock"], ObjectDirection.Up, false));
                    break;

                case RoomType.Hostile:
                    for (int i = 0; i < (int)(1 + roomLevel); i++)
                        enemyList.Add(new Enemy(random.Next(100, 700), random.Next(100, 400), roomLevel, random.Next(0, 3), contentManager));

                    debris.Add(new Tile(350, 50, contentManager.Textures["debris"], true));
                    debris.Add(new Tile(350, 450, contentManager.Textures["debris"], true));
                    debris.Add(new Tile(700, 250, contentManager.Textures["debris"], true));
                    debris.Add(new Tile(0, 250, contentManager.Textures["debris"], true));
                    break;

                case RoomType.Boss:

                    break;
                case RoomType.Extract:
                    clear = true;

                    debris.Add(new Tile(100, 0, 50, 150, contentManager.Textures["ladder"], ObjectDirection.Up, false));
                    debris.Add(new Tile(600, 0, 50, 150, contentManager.Textures["ladder"], ObjectDirection.Up, false));

                    tileArray[1, 2].ObjectTexture = contentManager.Textures["sandTopLeft"];
                    tileArray[11, 2].ObjectTexture = contentManager.Textures["sandTopLeft"];
                    tileArray[2, 2].ObjectTexture = contentManager.Textures["sandTopMid"];
                    tileArray[12, 2].ObjectTexture = contentManager.Textures["sandTopMid"];
                    tileArray[3, 2].ObjectTexture = contentManager.Textures["sandTopRight"];
                    tileArray[13, 2].ObjectTexture = contentManager.Textures["sandTopRight"];

                    tileArray[1, 3].ObjectTexture = contentManager.Textures["sandBottomLeft"];
                    tileArray[11, 3].ObjectTexture = contentManager.Textures["sandBottomLeft"];
                    tileArray[2, 3].ObjectTexture = contentManager.Textures["sandBottomMid"];
                    tileArray[12, 3].ObjectTexture = contentManager.Textures["sandBottomMid"];
                    tileArray[3, 3].ObjectTexture = contentManager.Textures["sandBottomRight"];
                    tileArray[13, 3].ObjectTexture = contentManager.Textures["sandBottomRight"];

                    tileArray[10, 2].ObjectTexture = contentManager.Textures["leftCornerShadow"];
                    tileArray[4, 2].ObjectTexture = contentManager.Textures["rightCornerShadow"];
                    tileArray[1, 4].ObjectTexture = contentManager.Textures["middleShadow"];

                    break;
            }
        }

        public void RoomDraw(SpriteBatch sb)
        {
            //  Tile Drawing
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tileArray[i, j].Draw(sb, tileArray[i, j].Orientation);
                }
            }

            // Debris
            for (int i = 0; i < debris.Count; i++)
            {
                debris[i].Draw(sb, debris[i].Orientation);
            }

            if (roomType == RoomType.Shop)
            {
                screenManager.DrawShop(sb, this);
            }

            // Enemy Drawing
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].Active)
                {
                    Vector2 enemyStatPos = new Vector2(enemyList[i].X, enemyList[i].Y + 50);
                    enemyList[i].Draw(sb, contentManager.Textures["mummyTexture"], enemyList[i].GetEnemyPosition(),
                        contentManager.MummyRectangle[enemyList[i].MummyAnimationIndex], Color.White, 2);

                    // Enemy stats drawing.
                    //sb.DrawString(contentManager.Font, $"Health: {enemyList[i].Health}\nAttack Damage: {enemyList[i].AttackDamage}" +
                        //$"\nEnemy Type: {enemyList[i].Type}", enemyStatPos, Color.Black);
                }
            }

            //Drawing projectiles
            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                if (enemyProjectiles[i].Active)
                {
                    enemyProjectiles[i].Draw(sb, Color.White);
                }
            }



            // Drawing Coins
            for (int i = 0; i < coinList.Count; i++)
            {
                coinList[i].Draw(sb, contentManager.Textures["coinTexture"], coinList[i].GetCoinPosition(),
                        contentManager.CoinRectangle[coinList[i].CoinAnimationIndex], Color.White, (float)0.2f);
            }

            for (int i = 0; i < lootList.Count; i++)
            {
                if (lootList[i].Active)
                {
                    lootList[i].Draw(sb, Color.White);
                }
            }
        }


        public void RoomUpdate(GameTime gameTime)
        {
            if (roomType == RoomType.Extract)
            {
                if (new Rectangle(100, 100, 50, 20).Intersects(player.CollisionBox) || new Rectangle(600, 100, 50, 20).Intersects(player.CollisionBox))
                {
                    player.Extract();
                }
            }

            if (roomType == RoomType.Shop)
            {
                screenManager.UpdateShop(this);
            }

            //for each enemy in the enemy list, have each enemy move if they are active
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].Active)
                {
                    enemyList[i].Move(player, gameTime, contentManager.Textures["projectile"], enemyProjectiles);
                    enemyList[i].Update(gameTime);
                }
            }

            for (int i = 0; i < coinList.Count; i++)
            {
                if (coinList[i].Active)
                {
                    coinList[i].Update(gameTime);
                }
            }

            //Move the projectiles and have them attack
            for (int i = 0; i < player.PlayerProjectiles.Count; i++)
            {
                if (player.PlayerProjectiles[i].Active)
                {
                    player.PlayerProjectiles[i].Move();
                    for (int j = enemyList.Count - 1; j >= 0; j--)
                    {
                        player.PlayerProjectiles[i].Attack(enemyList[j]);
                    }
                }
            }

            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                if (enemyProjectiles[i].Active)
                {
                    enemyProjectiles[i].Move();
                    enemyProjectiles[i].Attack(player);
                }
            }

            // Check if the sword is dealing damage
            if (player.SwingingSword)
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    if (Math.Abs(enemyList[i].X - player.X) < 50 && Math.Abs(enemyList[i].Y - player.Y) < 50)
                    {
                        enemyList[i].Health -= player.AttackDamage * 7;
                    }
                }
            }


            //Check if the player is collecting loot
            for (int i = lootList.Count - 1; i >= 0; i--)
            {
                if (lootList[i].Active)
                {
                    lootList[i].Collect(player);
                }
            }

            //Check if the player is collecting the coins
            for (int i = coinList.Count - 1; i >= 0; i--)
            {
                if (coinList[i].Active)
                {
                    coinList[i].Collect(player);
                }
            }

            // Kills enemies and drops coins at the body.
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].Health <= 0)
                {
                    int numCoins = random.Next(1, 6);
                    for (int j = 0; j < numCoins; j++)
                    {
                        Coin tempCoin = new Coin(enemyList[i].X + random.Next(-50, 50), enemyList[i].Y + random.Next(-50, 50),
                            contentManager.Textures["coinWidthHeight"].Width, contentManager.Textures["coinWidthHeight"].Height, 
                            contentManager.Textures["coinTexture"], 1);
                        coinList.Add(tempCoin);
                    }

                    if (player.AnkhUnlocked && player.Souls < 25)
                        player.Souls++;

                    enemyList.RemoveAt(i);
                }
            }

            if (enemyList.Count == 0 && !clear)
            {
                RoomClear();
            }

            ResolveCollisions();

        }


        public void RoomClear()
        {
            clear = true;
            int randomLoot;
            bool lootSpawned = false;

            // Once all enemies are dead, chooses a random item to boost a stat
            // from whatever items the player hasn't already maxed out.

            while (!lootSpawned)
            {
                randomLoot = random.Next(1, 51);

                // Plant increases max health.
                if (randomLoot > 35)
                {
                    lootList.Add(new Loot(370, 245, 50, 50, contentManager.Textures["herb"], 5));
                    lootSpawned = true;
                }

                // Quiver increases damage.
                else if (randomLoot > 20)
                {
                    lootList.Add(new Loot(370, 245, 50, 50, contentManager.Textures["quiver"], 0));
                    lootSpawned = true;
                }

                // Ring increases attack speed. Capped at 10 rings.
                else if (randomLoot > 15)
                {
                    if (player.TempStats[2] + player.PermStats[2] < 10)
                    {
                        lootList.Add(new Loot(370, 245, 50, 50, contentManager.Textures["ring"], 0));
                        lootSpawned = true;
                    }
                }

                // Necklace increases movement speed. Capped at 5 necklaces.
                else if (randomLoot > 13)
                {
                    if (player.TempStats[3] + player.PermStats[3] < 5)
                    {
                        lootList.Add(new Loot(370, 245, 50, 50, contentManager.Textures["necklace"], 0));
                        lootSpawned = true;
                    }
                }

                // Bracer increases critical damage. Capped at 35 bracers.
                else if (randomLoot > 5)
                {
                    if (player.TempStats[4] + player.PermStats[4] < 35)
                    {
                        lootList.Add(new Loot(370, 245, 50, 50, contentManager.Textures["armGuard"], 0));
                        lootSpawned = true;
                    }
                }

                // Feather increases arrow speed. Capped at 10 feathers.
                else
                {
                    if (player.TempStats[5] + player.PermStats[5] < 10)
                    {
                        lootList.Add(new Loot(370, 245, 50, 50, contentManager.Textures["feather"], 0));
                        lootSpawned = true;
                    }
                }
            }

            // A health potion also always spawns.
            lootList.Add(new Loot(330, 245, 50, 50, contentManager.Textures["potion"], 10));


            // Clears debris from doorways so that player can move to next rooms.
            // If the player is at the edge of the map, does not clear debris.
            int failedRemovals = 0;

            if (roomY < 2000) debris.RemoveAt(0);
            else failedRemovals++;

            if (roomY > 0) debris.RemoveAt(failedRemovals);
            else failedRemovals++;

            if (roomX < 2000) debris.RemoveAt(failedRemovals);
            else failedRemovals++;

            if (roomX > 0) debris.RemoveAt(failedRemovals);
        }


        public bool Contact(Rectangle collider, GameObject collidee)
        {
            return collider.Intersects(collidee.ObjectBox);
        }


        public void ResolveCollisions()
        {
            List<GameObject> collisionList = new List<GameObject>();
            Tile currentTile;

            // Wall tiles
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    currentTile = tileArray[i, j];

                    if (currentTile.Solid)
                        if (Contact(player.CollisionBox, currentTile))
                            collisionList.Add(currentTile);
                }
            }

            // Obstacle tiles
            for (int i = 0; i < debris.Count; i++)
            {
                currentTile = debris[i];

                if (currentTile.Solid)
                    if (Contact(player.CollisionBox, debris[i]))
                        collisionList.Add(currentTile);
            }

            for (int i = 0; i < collisionList.Count; i++)
            {
                Rectangle overlap = Rectangle.Intersect(player.CollisionBox, collisionList[i].ObjectBox);

                if(overlap.Height >= overlap.Width)
                {
                    if (collisionList[i].X > player.X)
                        player.X -= overlap.Width;
                    else
                        player.X += overlap.Width;
                }
            }

            for (int i = 0; i < collisionList.Count; i++)
            {
                Rectangle overlap = Rectangle.Intersect(player.CollisionBox, collisionList[i].ObjectBox);

                if (overlap.Width > overlap.Height)
                {
                    if (collisionList[i].Y > player.CollisionBox.Y)
                        player.Y -= overlap.Height;
                    else
                        player.Y += overlap.Height;
                }
            }
        }
    }
}
