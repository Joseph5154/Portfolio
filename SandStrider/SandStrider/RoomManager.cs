using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandStrider
{
    public enum DoorDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    // Luciano Balsamo
    // This class is designed to manage the creation and saving
    // of all the rooms the player has/will explore in the dungeon.
    internal class RoomManager
    {
        private Room[,] rooms;
        private int roomCount;

        private Room currentRoom;
        private int currentRoomX;
        private int currentRoomY;

        private ContentManager contentManager;
        private ScreenManager screenManager;
        private Player player;
        private Random random;
        private CustomItem customItem;

        private Vector2 lastShop;


        public RoomManager(ContentManager contentManager, Player player, ScreenManager screenManager)
        {
            rooms = new Room[2000, 2000];
            roomCount = 1;

            this.contentManager = contentManager;
            this.screenManager = screenManager;
            this.player = player;
            random = new Random();
            customItem = null;

            Rooms[1000, 1000] = new Room(RoomType.Start, FillTiles(), player.RoomCount / 10, 1000, 1000, player, contentManager, screenManager);
            currentRoom = Rooms[1000, 1000];
            currentRoomX = 1000;
            currentRoomY = 1000;

            lastShop = new Vector2(1000, 1000);
            currentRoom.RoomGeneration(customItem);
        }


        public Room[,] Rooms
        {
            get { return rooms; }
            set { rooms = value; }
        }


        public Room CurrentRoom
        {
            get { return currentRoom; }
            set { currentRoom = value; }
        }


        public void Update(GameTime gameTime, KeyboardState kb)
        {
            CurrentRoom.RoomUpdate(gameTime);
            CheckForRoomSwap();

            // Teleports player to last discovered shop if they have the book unlocked.
            if (kb.IsKeyDown(Keys.T) && player.BookUnlocked)
            {
                currentRoom = rooms[(int)lastShop.X, (int)lastShop.Y];
                currentRoomX = (int)lastShop.X;
                currentRoomY = (int)lastShop.Y;
            }
        }


        public void Reset()
        {
            player.X = 350;
            player.Y = 225;

            player.LoadStats();
            for (int i = 0; i < player.TempStats.Length; i++)
            {
                player.TempStats[i] = 0;
            }

            player.Health = player.MaxHealth;
            player.Dead = false;
            player.HasExtracted = false;

            rooms = new Room[2000, 2000];
            roomCount = 1;

            Rooms[1000, 1000] = new Room(RoomType.Start, FillTiles(), player.RoomCount / 10, 1000, 1000, player, contentManager, screenManager);
            currentRoom = Rooms[1000, 1000];
            currentRoom.RoomGeneration(customItem);
            currentRoomX = 1000;
            currentRoomY = 1000;
        }


        public void CheckForRoomSwap()
        {
            // If room is clear and player goes through doorway, move to next room.
            if (currentRoom.Clear)
            {
                if (player.X > 740) NextRoom(DoorDirection.Right);
                if (player.X < -40) NextRoom(DoorDirection.Left);
                if (player.Y > 440) NextRoom(DoorDirection.Down);
                if (player.Y < -50) NextRoom(DoorDirection.Up);
            }
            else
            {
                // Else, teleport to center of room.
                if (player.X > 740) { player.X = 350; player.Y = 225; }
                if (player.X < -50) { player.X = 350; player.Y = 225; }
                if (player.Y > 440) { player.X = 350; player.Y = 225; }
                if (player.Y < -40) { player.X = 350; player.Y = 225; }
            }
        }


        public void NextRoom(DoorDirection door)
        {
            switch (door)
            {
                case DoorDirection.Up:
                    currentRoomY++;
                    player.Y = 375;
                    break;

                case DoorDirection.Down:
                    currentRoomY--;
                    player.Y = 100;
                    break;

                case DoorDirection.Left:
                    currentRoomX--;
                    player.X = 625;
                    break;

                case DoorDirection.Right:
                    currentRoomX++;
                    player.X = 75;
                    break;
            }

            if (rooms[currentRoomX, currentRoomY] == null)
            {
                int roomRoll = random.Next(1, 11);
                roomCount++;
                player.TempStats[8]++;

                if (roomRoll == 1)
                {
                    rooms[currentRoomX, currentRoomY] = new Room(RoomType.Passive, FillTiles(), (player.RoomCount + roomCount) / 10, currentRoomX, currentRoomY, player, contentManager, screenManager);
                }
                else if (roomRoll == 2)
                {
                    rooms[currentRoomX, currentRoomY] = new Room(RoomType.Extract, FillTiles(), (player.RoomCount + roomCount) / 10, currentRoomX, currentRoomY, player, contentManager, screenManager);
                }
                else if(roomRoll == 3)
                {
                    rooms[currentRoomX, currentRoomY] = new Room(RoomType.Shop, FillTiles(), (player.RoomCount + roomCount) / 10, currentRoomX, currentRoomY, player, contentManager, screenManager);
                    lastShop.X = currentRoomX;
                    lastShop.Y = currentRoomY;
                }
                else
                {
                    rooms[currentRoomX, currentRoomY] = new Room(RoomType.Hostile, FillTiles(), (player.RoomCount + roomCount) / 10, currentRoomX, currentRoomY, player, contentManager, screenManager);
                }

                rooms[currentRoomX, currentRoomY].RoomGeneration(customItem);
            }

            player.PlayerProjectiles.Clear();
            currentRoom.EnemyProjectiles.Clear();

            currentRoom = rooms[currentRoomX, currentRoomY];
        }


        public Tile[,] FillTiles()
        {
            const int tileSize = 50;
            int luckyDucky = 0;
            Tile[,] tileArray = new Tile[15, 10];

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    luckyDucky = random.Next(1, 51);

                    if (luckyDucky == 50)
                        tileArray[i, j] = new Tile(i * tileSize, j * tileSize, contentManager.Textures["floorFour"], false);
                    else if (luckyDucky == 49)
                        tileArray[i, j] = new Tile(i * tileSize, j * tileSize, contentManager.Textures["floorThree"], false);
                    else if (luckyDucky == 48)
                        tileArray[i, j] = new Tile(i * tileSize, j * tileSize, contentManager.Textures["floorTwo"], false);
                    else
                        tileArray[i, j] = new Tile(i * tileSize, j * tileSize, contentManager.Textures["floorOne"], false);
                }
            }

            tileArray[0, 0] = new Tile(0, 0, contentManager.Textures["topLeftCorner"], true);
            tileArray[14, 0] = new Tile(700, 0, contentManager.Textures["topRightCorner"], true);
            tileArray[14, 9] = new Tile(700, 450, contentManager.Textures["bottomRightCorner"], true);
            tileArray[0, 9] = new Tile(0, 450, contentManager.Textures["bottomLeftCorner"], true);

            tileArray[7, 1] = new Tile(350, 50, contentManager.Textures["middleShadow"], false);
            tileArray[7, 2] = new Tile(350, 100, contentManager.Textures["leftCornerShadow"], false);
            tileArray[1, 2] = new Tile(50, 100, contentManager.Textures["topLeftShadow"], false);

            tileArray[6, 0] = new Tile(300, 0, contentManager.Textures["bottomRightPassage"], true);
            tileArray[8, 0] = new Tile(400, 0, contentManager.Textures["bottomLeftPassage"], true);
            tileArray[6, 9] = new Tile(300, 450, contentManager.Textures["topRightPassage"], true);
            tileArray[8, 9] = new Tile(400, 450, contentManager.Textures["topLeftPassage"], true);
            tileArray[0, 3] = new Tile(0, 150, contentManager.Textures["bottomRightPassage"], true);
            tileArray[0, 6] = new Tile(0, 300, contentManager.Textures["topRightPassage"], true);
            tileArray[14, 3] = new Tile(700, 150, contentManager.Textures["bottomLeftPassage"], true);
            tileArray[14, 6] = new Tile(700, 300, contentManager.Textures["topLeftPassage"], true);

            for (int i = 1; i < 14; i++)
            {
                if (i == 7)
                    continue; // Doorways

                luckyDucky = random.Next(1, 21);

                if (luckyDucky == 20)
                    tileArray[i, 1] = new Tile(i * 50, 50, contentManager.Textures["sunWall"], true);
                else if (luckyDucky == 19)
                    tileArray[i, 1] = new Tile(i * 50, 50, contentManager.Textures["bannerWallOne"], true);
                else if (luckyDucky == 18)
                    tileArray[i, 1] = new Tile(i * 50, 50, contentManager.Textures["bannerWallTwo"], true);
                else if (luckyDucky >= 14)
                    tileArray[i, 1] = new Tile(i * 50, 50, contentManager.Textures["crackedWallOne"], true);
                else if (luckyDucky >= 10)
                    tileArray[i, 1] = new Tile(i * 50, 50, contentManager.Textures["crackedWallTwo"], true);
                else
                    tileArray[i, 1] = new Tile(i * 50, 50, contentManager.Textures["midWall"], true);
            }

            luckyDucky = random.Next(1, 21);

            if (luckyDucky == 20)
                tileArray[0, 4] = new Tile(0 * 50, 200, contentManager.Textures["sunWall"], true);
            else if (luckyDucky == 19)
                tileArray[0, 4] = new Tile(0 * 50, 200, contentManager.Textures["bannerWallOne"], true);
            else if (luckyDucky == 18)
                tileArray[0, 4] = new Tile(0 * 50, 200, contentManager.Textures["bannerWallTwo"], true);
            else if (luckyDucky >= 14)
                tileArray[0, 4] = new Tile(0 * 50, 200, contentManager.Textures["crackedWallOne"], true);
            else if (luckyDucky >= 10)
                tileArray[0, 4] = new Tile(0 * 50, 200, contentManager.Textures["crackedWallTwo"], true);
            else
                tileArray[0, 4] = new Tile(0 * 50, 200, contentManager.Textures["midWall"], true);

            luckyDucky = random.Next(1, 21);

            if (luckyDucky == 20)
                tileArray[14, 4] = new Tile(14 * 50, 200, contentManager.Textures["sunWall"], true);
            else if (luckyDucky == 19)
                tileArray[14, 4] = new Tile(14 * 50, 200, contentManager.Textures["bannerWallOne"], true);
            else if (luckyDucky == 18)
                tileArray[14, 4] = new Tile(14 * 50, 200, contentManager.Textures["bannerWallTwo"], true);
            else if (luckyDucky >= 14)
                tileArray[14, 4] = new Tile(14 * 50, 200, contentManager.Textures["crackedWallOne"], true);
            else if (luckyDucky >= 10)
                tileArray[14, 4] = new Tile(14 * 50, 200, contentManager.Textures["crackedWallTwo"], true);
            else
                tileArray[14, 4] = new Tile(14 * 50, 200, contentManager.Textures["midWall"], true);

            for (int i = 1; i < 14; i++)
            {
                if (i == 6)
                    i = 9;

                luckyDucky = random.Next(1, 21);

                if (luckyDucky >= 14)
                    tileArray[i, 0] = new Tile(i * 50, 0, contentManager.Textures["topWallThree"], true);
                else if (luckyDucky >= 8)
                    tileArray[i, 0] = new Tile(i * 50, 0, contentManager.Textures["topWallTwo"], true);
                else
                    tileArray[i, 0] = new Tile(i * 50, 0, contentManager.Textures["topWallOne"], true);
            }


            for (int i = 1; i < 14; i++)
            {
                if (i == 6)
                    i = 9;

                luckyDucky = random.Next(1, 21);

                if (luckyDucky >= 11)
                    tileArray[i, 9] = new Tile(i * 50, 450, contentManager.Textures["bottomWallOne"], true);
                else
                    tileArray[i, 9] = new Tile(i * 50, 450, contentManager.Textures["bottomWallTwo"], true);
            }


            for (int i = 2; i < 14; i++)
            {
                if (i == 7)
                    i = 8;

                luckyDucky = random.Next(1, 21);

                if (luckyDucky >= 14)
                    tileArray[i, 2] = new Tile(i * 50, 100, contentManager.Textures["topShadowAlt"], false);
                else
                    tileArray[i, 2] = new Tile(i * 50, 100, contentManager.Textures["topShadow"], false);
            }


            for (int i = 3; i < 9; i++)
            {
                if (i == 5)
                    i = 7;

                luckyDucky = random.Next(1, 21);

                if (luckyDucky >= 14)
                    tileArray[1, i] = new Tile(50, i * 50, contentManager.Textures["leftShadowAlt"], false);
                else
                    tileArray[1, i] = new Tile(50, i * 50, contentManager.Textures["leftShadow"], false);
            }

            luckyDucky = random.Next(1, 21);

            if (luckyDucky >= 14)
                tileArray[0, 5] = new Tile(0, 250, contentManager.Textures["topShadowAlt"], false);
            else
                tileArray[0, 5] = new Tile(0, 250, contentManager.Textures["topShadow"], false);

            tileArray[1, 5] = new Tile(50, 250, contentManager.Textures["leftCornerShadow"], false);
            tileArray[1, 6] = new Tile(50, 300, contentManager.Textures["middleShadow"], false);
            tileArray[14, 5] = new Tile(700, 250, contentManager.Textures["rightCornerShadow"], false);
            tileArray[7, 9] = new Tile(350, 450, contentManager.Textures["middleShadow"], false);

            tileArray[0, 1] = new Tile(0, 50, contentManager.Textures["leftWall"], true);
            tileArray[0, 2] = new Tile(0, 100, contentManager.Textures["leftWall"], true);
            tileArray[0, 7] = new Tile(0, 350, contentManager.Textures["leftWall"], true);
            tileArray[0, 8] = new Tile(0, 400, contentManager.Textures["leftWall"], true);
            tileArray[14, 1] = new Tile(700, 50, contentManager.Textures["rightWall"], true);
            tileArray[14, 2] = new Tile(700, 100, contentManager.Textures["rightWall"], true);
            tileArray[14, 7] = new Tile(700, 350, contentManager.Textures["rightWall"], true);
            tileArray[14, 8] = new Tile(700, 400, contentManager.Textures["rightWall"], true);


            return tileArray;
        }

        public void LoadCustomItem()
        {
            customItem = new CustomItem();
            customItem.LoadFile();
        }

        public void UnloadCustomItem()
        {
            customItem = null;
        }

        public string GetPathToTexture()
        {
            return customItem.CustomTexturePath;
        }
    }
}