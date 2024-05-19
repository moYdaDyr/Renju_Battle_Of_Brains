using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GameAI
{
    
    public static (int x, int y) BotsMove()
    {
        
        int[,] weights = new int[15, 15];
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                weights[i, j] = 1;
            }
        }
        if (GameData.gameType == 1 && GameData.currentMove == 3)
        {
            for (int i=4; i < 11; i++)
            {
                weights[i, 4] = 5;
                weights[i, 10] = 5;
            }

            for (int i = 4; i < 11; i++)
            {
                weights[4, i] = 5;
                weights[10, i] = 5;
            }
        }

        Debug.Log("aggressivness: "+GameData.aggressivness+" enemy move: "+ GameData.playerMove+" current move: "+ (GameData.currentMove));

        Perebor(ref weights, GameData.aggressivness, GameData.currentMove + 1);
        Perebor(ref weights, 1, GameData.currentMove);

        
        List<Move> moves = new List<Move>();
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (!GameData.IsCorrect(i,j))
                {
                    weights[i, j] *= 0;
                }
                if (weights[i, j] > 0)
                {
                    moves.Add(new Move(i, j, weights[i, j]));
                }
            }
        }

        moves.Sort();
        moves.Reverse();

        Debug.Log("number of moves: " + moves.Count);
        Debug.Log("field center: " + moves.Count);

        int n = 0;
        if (GameData.difficulty == 0)
        {
            while (moves[n].weight > moves[0].weight * 0.5 )
                n++;

            n = UnityEngine.Random.Range(0, n + 1);
        }
        else if (GameData.difficulty == 1)
        {
            while (moves[n].weight > moves[0].weight * 0.75 )
                n++;

            n = UnityEngine.Random.Range(0, n + 1);
        }
        else
        {
            n = 0;
        }
        
        (int x, int y) nextMove = (moves[n].x, moves[n].y);

        return nextMove;
    }

    static int Otsenka(int counterValue, double aggressive)
    {
        if (counterValue >= 5 && GameData.gameType == 0)
        {
            return (int)Math.Round(100000 * aggressive);
        }
        else if (counterValue >= 5 && GameData.gameType != 0)
        {
            return 1;
        }
        else if (counterValue == 4)
        {
            return (int)Math.Round(100000 * aggressive);
        }
        else return (int)Math.Round(100 * Math.Pow(4, counterValue - 1) * aggressive);
    }

    static void Perebor(ref int[,] weigths, double aggressivness, int move)
    {
        int counter = 0;

        int[,] field = GameData.field;

        // горизонтали

        for (int i = 0; i < weigths.GetLength(0); i++)
        {
            for (int j = 0; j < weigths.GetLength(1); j++)
            {
                if ((move % 2 == 0 && field[i, j] == -1) || (move % 2 == 1 && field[i, j] == 1) || (field[i, j] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[i, j] += Otsenka(counter, aggressivness);
                    counter = 0;
                }

            }
            counter = 0;
        }

        for (int i = weigths.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = weigths.GetLength(1) - 1; j >= 0; j--)
            {
                if ((move % 2 == 0 && field[i, j] == -1) || (move % 2 == 1 && field[i, j] == 1) || (field[i, j] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[i, j] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        // вертикали

        for (int i = 0; i < weigths.GetLength(0); i++)
        {
            for (int j = 0; j < weigths.GetLength(1); j++)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j,i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }

            }
            counter = 0;
        }

        for (int i = weigths.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = weigths.GetLength(1) - 1; j >= 0; j--)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j,i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        // диагональ главная

        counter = 0;

        for (int w = 1; w < 15; w++)
        {
            for (int i = w, j = 0; i < 15; i++, j++)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        counter = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = 0, j = w; j < 15; i++, j++)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        counter = 0;

        for (int w = 1; w < 15; w++)
        {
            for (int i = 14-w, j = 14; i >=0 && j>=0; i--, j--)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        counter = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = 14, j = 14-w; i >= 0 && j >= 0; i--, j--)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        // побочная диагональ

        counter = 0;

        for (int w = 1; w < 15; w++)
        {
            for (int i = w, j = 14; i < 15 && j >= 0; i++, j--)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        counter = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = 0, j = 14 - w; i < 15 && j >= 0; i++, j--)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        counter = 0;

        for (int w = 1; w < 15; w++)
        {
            for (int i = 14-w, j = 0; i >=0 && j <15; i--, j++)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }

        counter = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = 14, j = w; i >=0 && j <15; i--, j++)
            {
                if ((move % 2 == 0 && field[j, i] == -1) || (move % 2 == 1 && field[j, i] == 1) || (field[j, i] == 2))
                {
                    counter++;
                }
                else if (counter != 0)
                {
                    weigths[j, i] += Otsenka(counter, aggressivness);
                    counter = 0;
                }
            }
            counter = 0;
        }
    }


    public class Move : IComparable
    {
        public int x;
        public int y;
        public int weight;

        public Move(int x, int y, int weight)
        {
            this.y = y;
            this.x = x;
            this.weight = weight;
        }
        public int CompareTo(object m2)
        {
            if (this.weight > (m2 as Move).weight)
            {
                return 1;
            }
            else if (this.weight < (m2 as Move).weight)
            {
                return -1;
            }
            return 0;
        }

        override public string ToString()
        {
            return "x: " + x + " y: " + y + " weight: " + weight;
        }
    }
}

