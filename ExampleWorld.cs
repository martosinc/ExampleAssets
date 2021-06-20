using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using System.Collections.Generic;
using Examples.Tiles;
using Microsoft.Xna.Framework;


namespace Exampleu {
    public class ExampleWorld : ModWorld {
        public int width;
        public int height;
        public int smoothCycles=5;

        private int[,] cavePoints;
        public int randFillPercent=50;
        public int tile = ModContent.TileType<CrystalSand>();
        public int wall = 172;
        public int threshold=4;
        public int treasureHouses = 0;
        static readonly byte[,] TreasureHouseTiles =
        {
            {2,2,2,1,1,2,2,2},
            {2,2,1,1,1,1,2,2},
            {2,1,1,1,1,1,1,2},
            {1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,0},
            {0,1,0,0,0,0,1,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,3,0,0,0},
            {0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1},
            {2,1,1,1,1,1,1,2},
            {2,2,1,1,1,1,2,2},
            {2,1,1,1,1,1,1,2},
            {2,2,2,1,1,2,2,2},
        };
        static readonly byte[,] TreasureHouseWalls =
        {
            {0,0,0,0,0,0,0,0},
            {2,2,0,1,1,0,2,2},
            {2,0,1,1,1,1,1,2},
            {0,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,0,0},
            {0,0,1,1,1,1,0,0},
            {0,0,1,1,1,1,0,0},
            {0,0,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,0,0},
            {0,0,0,1,1,0,0,0},
            {0,0,1,1,1,1,0,0},
            {0,0,0,0,0,0,0,0},
        };
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
            int DesertificationIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Planting Trees"));
            // int DesertificationIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Full Desert"));
            if (DesertificationIndex != -1) {
                tasks.Insert(DesertificationIndex + 1, new PassLegacy("YES", ExampleGen));            
            }
        }
        private void ExampleGen(GenerationProgress progress) {
            // Biome Shape
            progress.Message = "SUUUUUUUUCK";
            width = (int)(Main.rand.Next(150,250)*0.8f);
            height = width;
            // var XPos = WorldGen.genRand.Next((int)Main.maxTilesX/4,Main.maxTilesX*3/4);
            var YPos = WorldGen.genRand.Next((int)WorldGen.rockLayer+250, Main.maxTilesY-350);
            var XPos = (int)Main.maxTilesX/2 + Main.rand.Next(-100,100);
            // var YPos = 150;
            int startX = XPos-width/2;
            int startY = YPos-height/2;
            WorldGen.TileRunner(
                XPos,
                YPos,
                width,
                100,
                tile,
                true,
                0f,
                0f,
                false,
                true
            );
            GenerateCave();
            PlaceGrid(startX,startY);
            // Chests
            // SpawnTreasure(XPos,YPos);
            // Trees
            for (int j = 0; j < width; j++) {
                for (int k = 0; k < height; k++) {
                    if (cavePoints[j,k] == 0) {
                        int height = GetHeight(j,k);
                        
                        if (height>=25 && Main.tile[startX+j,startY+k+1].type == tile && Main.rand.Next(15)==0) {  
                                Main.tile[startX+j,startY+k+1].type = 2; 
                                Main.tile[startX+j+1,startY+k+1].type = 2; 
                                Main.tile[startX+j-1,startY+k+1].type = 2; 
                                Main.tile[startX+j,startY+k+1].type = 2; 
                                // Main.tile[startX+j+2,startY+k+1].type = 2; 
                                // Main.tile[startX+j-2,startY+k+1].type = 2; 
                                PreparePlant(startX+j,startY+k,height);
                                WorldGen.PlaceObject(startX+j,startY+k,20);
                                WorldGen.KillWall(startX+j,startY+k);
                                WorldGen.GrowTree(startX+j,startY+k);
                        }
                    }
                }
            }
        }
        private void GenerateCave()
        {
            cavePoints = new int[width, height];
            int seed = Main.rand.Next(0, 1000000);
            System.Random randChoice = new System.Random(seed.GetHashCode());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        cavePoints[x, y] = 1;
                    }else if (randChoice.Next(0, 100) < randFillPercent)
                    {
                        cavePoints[x, y] = 1;
                    }else
                    {
                        cavePoints[x, y] = 0;
                    }
                }
            }
            for (int i = 0; i < smoothCycles; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int neighboringWalls = GetNeighbors(x, y);

                        if (neighboringWalls > threshold)
                        {
                            cavePoints[x, y] = 1;
                        }else if (neighboringWalls < threshold)
                        {
                            cavePoints[x, y] = 0;
                        }
                    }
                }
            }
        }
        private int GetNeighbors(int pointX, int pointY)
        {

            int wallNeighbors = 0;

            for (int x = pointX - 1; x <= pointX + 1; x++)
            {
                for (int y = pointY - 1; y <= pointY + 1; y++)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        if (x != pointX || y!= pointY)
                        {
                            if (cavePoints[x,y] == 1)
                            {
                                wallNeighbors++;
                            }
                        }
                    }
                    else
                    {
                        wallNeighbors++;
                    }
                }
            }

            return wallNeighbors;
        }
        private void PlaceGrid(float startX, float startY)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Main.tile[(int)(startX+x), (int)(startY+y)].type == tile) {
                        WorldGen.PlaceWall((int)(startX+x), (int)(startY+y),wall);
                    }
                    if (cavePoints[x,y] == 0 && Main.tile[(int)(startX+x), (int)(startY+y)].type == tile)
                    {
                        WorldGen.KillTile((int)(startX+x), (int)(startY+y));
                    }
                    if (cavePoints[x,y] == 1) {
                    }
                    if (treasureHouses < 1 && Main.tile[(int)(startX+x), (int)(startY+y)].type == tile && Main.rand.Next(3000)==0) {
                        SpawnTreasure((int)(startX+x), (int)(startY+y));
                        treasureHouses++;
                    }
                }
            }
        }  
        private int GetHeight(int pointX, int pointY) {
            var x = pointX;
            var y = pointY;

            int height = 0;
            for (int i = 1; i <= 25; i++) {
                if (cavePoints[x,y-i] == 0) {
                    height = i;
                } else {
                    break;
                }
            }

            return height;
        }
        private void PreparePlant(int pointX, int pointY, int height) {
            int x = pointX;
            int y = pointY;

            // WorldGen.KillTile(x+1,y+1);
            // WorldGen.PlaceTile(x+1,y+1,plantTile);

            // WorldGen.KillTile(x,y+1);
            // WorldGen.PlaceTile(x,y+1,plantTile);

            // WorldGen.KillTile(x-1,y+1);
            // WorldGen.PlaceTile(x-1,y+1,plantTile);

            for (int i = 0; i < height; i++) {
                WorldGen.KillTile(x+1,y-i);

                WorldGen.KillTile(x,y-i);

                WorldGen.KillTile(x-1,y-i);

                WorldGen.KillTile(x+2,y-i);

                WorldGen.KillTile(x-2,y-i);
            }
        }
        private void SpawnTreasure(int pointX, int pointY) {
            var tile1 = ModLoader.GetMod("RothurMod").TileType("HardenedCrystalSand");
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 15; y++) {
                    if (x==7 && y == 13) {break;}
                    var tile = Framing.GetTileSafely(pointX + x, pointY - y);
                    // file.WriteLine((x,y).ToString());
                    var X = pointX + x;
                    var Y = pointY - y;
                    switch (TreasureHouseTiles[y,x]) {
                        case 0:
                            WorldGen.KillTile(X, Y);
                            break;
                        case 1:
                            tile.type = TileID.SilverBrick;
                            tile.active(true);
                            break;
                        case 2:
                            break;
                        case 3:
                            WorldGen.KillTile(X, Y);
                            // tile.type = 197;
                            // file.WriteLine((x,y).ToString());
                            // file.Close();
                            break;
                    }
                    // tile.slope(0);
                    switch (TreasureHouseWalls[y,x]) {
                        case 0:
                            break;
                        case 1:
                            WorldGen.KillWall((int)(pointX+x), (int)(pointY-y));
                            WorldGen.PlaceWall((int)(pointX+x), (int)(pointY-y), WallID.SilverBrick);
                            break;
                    }
                    WorldGen.PlaceChest(pointX + 3, pointY - 6, 21, true, 1);
                    var chestIndex = Chest.FindChest((int)(pointX + 3), (int)(pointY - 7));
                    int[] items = new int[6];
                    // StreamWriter file = new StreamWriter("/home/martos/.local/share/Terraria/ModLoader/Mod Sources/Examples/test.txt");
                    // file.WriteLine(chestIndex.ToString());
                    // file.Close();   

                    items[0] = ItemID.AlphabetStatueP;
                    items[1] = ItemID.AlphabetStatueO;
                    items[2] = ItemID.AlphabetStatueS;
                    items[3] = ItemID.AlphabetStatueO;
                    items[4] = ItemID.AlphabetStatueS;
                    items[5] = ItemID.AlphabetStatueI;
                    if (chestIndex != -1) {
                        for (int i = 0; i < 6; i++) {
                            Main.chest[chestIndex].item[i].SetDefaults(items[i]);
                        }
                    }
                }
            }
        }
    }
} 