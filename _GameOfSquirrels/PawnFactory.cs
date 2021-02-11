﻿using System.Collections.Generic;
using System.Windows.Media;

namespace _GameOfSquirrels
{
    internal class PawnFactory
    {
        public List<IPawn> CreatePawns(int amount)
        {
            List<IPawn> playerList = new List<IPawn>();
            for (int i = 0; i < amount; i++)
            {
                playerList.Add(new Pawn(1, 1));

                switch (i)
                {
                    case 0:
                        playerList[0].Ellipse.Fill = Brushes.Red;
                        break;
                    case 1:
                        playerList[1].Ellipse.Fill = Brushes.Blue;
                        break;
                    case 2:
                        playerList[2].Ellipse.Fill = Brushes.Green;
                        break;
                    case 3:
                        playerList[3].Ellipse.Fill = Brushes.Yellow;
                        break;
                    default:
                        break;
                }
            }
            return playerList;
        }
    }
}