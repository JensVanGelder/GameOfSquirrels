﻿using _GameOfSquirrels.Tiles;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _GameOfSquirrels
{
    public class Game
    {
        private Dice dice = new Dice();
        private Board board;
        public List<IPawn> Playerlist;
        public List<ITile> BoardTiles;
        public int BoardWidth;
        public int BoardHeight;
        public bool PlayerFinishedMoving;
        public int RoundCounter { get; set; }
        public int LastNumberRolled { get; set; }

        public Grid GridGame { get; set; }

        public int CurrentPlayer { get; set; }

        public Game(Grid gridGame)
        {
            BoardWidth = 8;
            BoardHeight = 8;
            GridGame = gridGame;
            CurrentPlayer = 0;
        }

        public void GenerateBoard()
        {
            board = new Board(GridGame, BoardHeight, BoardWidth);
            BitmapImage img = new BitmapImage(new Uri(@"https://cdn.discordapp.com/attachments/809042663969652756/809745831732314152/TileGrass.png"));
            ImageBrush image = new ImageBrush();
            image.ImageSource = img;
            GridGame.Background = image;
            GenerateTiles();
            GeneratePawns();
        }

        private void GeneratePawns()
        {
            PawnFactory pawnFactory = new PawnFactory();
            Playerlist = pawnFactory.CreatePawns(4);
            foreach (var item in Playerlist)
            {
                GridGame.Children.Add(item.Ellipse);
                Grid.SetColumn(item.Ellipse, item.LocationX);
                Grid.SetRow(item.Ellipse, item.LocationY);
            }
        }

        private void GenerateTiles()
        {
            TileFactory tileFactory = new TileFactory();
            BoardTiles = tileFactory.CreateTiles(BoardHeight, BoardWidth);
            foreach (ITile tile in BoardTiles)
            {
                GridGame.Children.Add(tile.TileBorder);
                Grid.SetColumn(tile.TileBorder, tile.LocationX);
                Grid.SetRow(tile.TileBorder, tile.LocationY);
            }
        }

        public void MovePawn(int move)
        {
            if (Playerlist[CurrentPlayer].LocationX == BoardWidth - 1)
            {
                SetPlayerNewRow();
            }
            else
            {
                Playerlist[CurrentPlayer].Move(move);
            }

            InteractWithTile();
        }

        private void SetPlayerNewRow()
        {
            Playerlist[CurrentPlayer].LocationY += 1;
            Playerlist[CurrentPlayer].LocationX = 0;
            Grid.SetRow(Playerlist[CurrentPlayer].Ellipse, Playerlist[CurrentPlayer].LocationY);
            Grid.SetColumn(Playerlist[CurrentPlayer].Ellipse, Playerlist[CurrentPlayer].LocationX);
        }

        private void InteractWithTile()
        {
            foreach (ITile tile in BoardTiles)
            {
                if (Playerlist[CurrentPlayer].LocationX == tile.LocationX && Playerlist[CurrentPlayer].LocationY == tile.LocationY)
                {
                    if (tile is SquirrelTile)
                    {
                        tile.GetInteraction();
                        MovePawn(LastNumberRolled);
                    }
                    else if (tile is FireTile)
                    {
                        tile.GetInteraction();
                        Grid.SetColumn(Playerlist[CurrentPlayer].Ellipse, 0);
                        Playerlist[CurrentPlayer].LocationX = 0;
                    }
                    else if (tile is NormalTile)
                    {
                    }
                    else
                    {
                        MovePawn(tile.GetInteraction());
                    }
                }
            }

            if (Playerlist[CurrentPlayer].LocationX >= BoardWidth - 1) //end of the line
            {
            }
        }

        private void NextTurn()
        {
            CurrentPlayer++;
            if (CurrentPlayer > Playerlist.Count - 1)
            {
                CurrentPlayer = 0;
                RoundCounter++;
            }
        }

        public void DoTurn()
        {
            PlayerFinishedMoving = false;
            Dice dice = new Dice();
            int roll = dice.RollDice(1, 7);
            LastNumberRolled = roll;
            int movesavailable = LastNumberRolled;

            for (int i = 0; i < movesavailable; i++)
            {
                if (i == movesavailable - 1)
                {
                    PlayerFinishedMoving = true;
                    MovePawn(1);
                    
                }
                else
                {
                    MovePawn(1);
                }
            }

            NextTurn();
        }

        /// <summary>
        /// </summary>
        public List<ITile> tiles;

        public int maxCatapult = 5;
        public int maxFire = 0;
        public int maxBridge = 5;
        public int howManyNormal = 30;

        private int _normalTileCount;
        private int _catapultTileCount;
        private int _fireTileCount;
        private int _bridgeTileCount;
        private int _tileToGenerate = 0;
        private int _resetHowManyWhite;

        private void LoadMap()
        {
            _bridgeTileCount = 0;
            _catapultTileCount = 0;
            _fireTileCount = 0;
            _normalTileCount = 0;

            for (int i = 0; i < board.MaxBoardHeight; i++)
            {
                for (int j = 0; j < board.MaxBoardWidth; j++)
                {
                    _tileToGenerate = 1;
                    CheckTile(_tileToGenerate);

                    //if (x >= 0 && x < 3 && y >= 0 && y < 3)
                    //{
                    //    tileToGenerate = 1;
                    //    CheckTile(tileToGenerate);
                    //    GameObject obj;
                    //    obj = Instantiate(tiles[tileToGenerate], new Vector3(x, 0, y), Quaternion.identity);
                    //    obj.transform.parent = transform;
                    //}
                }
            }
        }

        private void CheckTile(int randomTile)
        {
            switch (randomTile)
            {
                case 1:
                    if (_catapultTileCount >= maxCatapult - 1)
                    {
                        _tileToGenerate = 1;
                        _normalTileCount++;
                        howManyNormal--;
                        break;
                    }
                    else
                    {
                        if (howManyNormal <= 0)
                        {
                            _catapultTileCount++;
                            howManyNormal = _resetHowManyWhite;
                            break;
                        }
                        else
                        {
                            _tileToGenerate = 1;
                            _normalTileCount++;
                            howManyNormal--;
                            break;
                        }
                    }

                case 2:
                    _normalTileCount++;
                    howManyNormal--;
                    break;

                case 3:
                    if (_bridgeTileCount >= maxBridge)
                    {
                        _tileToGenerate = 1;
                        _normalTileCount++;
                        howManyNormal--;
                        break;
                    }
                    else
                    {
                        _bridgeTileCount++;
                        break;
                    }

                default:
                    _normalTileCount++;
                    howManyNormal--;
                    break;
            }
        }
    }
}