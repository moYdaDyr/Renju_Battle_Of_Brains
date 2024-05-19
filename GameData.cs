using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GameData
{
    public static int[,] field;
    public static int gameMode; // o - single, 1 - multi
    public static int decorativeGameMode; // o - single, 1 - multi
    public static int gameType; // 0 - five-in-row, 1 - sapronovka, 2 - bolgarka
    public static int currentMove; // %2 == 0 - белые, %2 == 1 - чёрные
    public static DateTime date;
    public static int difficulty; // 0 - лёгкий, 2 - средний, 3 - сложный
    public static int playerMove; // 0 - белый, 1 - чёрный
    public static double aggressivness; // 1.4 - пассивный, 0.9 - сбалансированный, 0.4 - агрессивный

    public static System.Random rnd = new System.Random();

    public static string currentDirectory;

    public static bool changePlayerMove = true;

    public static bool isSavedRecently = false;

    // x - чёрные, o - белые, @ - серые

    public static void Init(int type = 0, int mode = -1)
    {
        //rnd = 
        field = new int[15, 15];
        for (int i = 0; i < 15; i++)
        {
            for (int i1 = 0; i1 < 15; i1++)
            {
                field[i, i1] = 0;
            }
        }
        field[7, 7] = 1;

        if (type != -1)
            gameType = type;

        if (mode != -1)
        {
            Debug.Log("mode changed " + gameMode);
            decorativeGameMode = mode;
            gameMode = mode;
        }

        currentMove = 2;
        if (gameType == 2)
        {
            field[7, 7] = 2;
            currentMove = 3;
        }

        date = new DateTime();

        Debug.Log(changePlayerMove);

        if (changePlayerMove)
        {
            playerMove = 0;
            aggressivness = 0;
            difficulty = 0;
        }
        else
        {
            changePlayerMove = true;
        }

        isSavedRecently = true;

        Debug.Log("current game mode init " + gameMode);
    }

    public static bool IsCorrect(int x, int y)
    {
        if (gameType == 1 && currentMove == 3 && x <= 9 && x >= 5 && y <= 9 && y >= 5)
        {
            return false;
        }
        if (field[x, y] != 0)
        {
            return false;
        }
        return true;
    }

    public static bool ReadFromFile(string file)
    {
        field = new int[15, 15];
        int currentMode = gameMode;

        Debug.Log("current game mode begin " + gameMode);

        if (!Directory.Exists(currentDirectory + "\\SavedGames"))
            Directory.CreateDirectory(currentDirectory + "\\SavedGames");

        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                gameType = reader.ReadInt32();
                if (gameType < 0 || gameType > 2) throw new Exception();

                currentMove = reader.ReadInt32();
                if (currentMove < 0 || currentMove > 225) throw new Exception();

                decorativeGameMode = reader.ReadInt32();
                if (decorativeGameMode < 0 || decorativeGameMode > 1)
                {
                    Debug.Log("decorative error "+ decorativeGameMode);
                    throw new Exception();
                }

                int years, months, days, hours, minutes, seconds;

                years = reader.ReadInt32();

                months = reader.ReadInt32();

                days = reader.ReadInt32();

                hours = reader.ReadInt32();

                minutes = reader.ReadInt32();

                seconds = reader.ReadInt32();

                difficulty = reader.ReadInt32();
                if (difficulty < 0 || difficulty > 2) throw new Exception();
                aggressivness = reader.ReadDouble();
                if (aggressivness < 0 || aggressivness > 2) throw new Exception();
                playerMove = reader.ReadInt32();
                if (playerMove < 0 || aggressivness > 2) throw new Exception();

                date = new DateTime(years, months, days, hours, minutes, seconds);

                for (int i = 0; i < 15; i++)
                {
                    for (int i1 = 0; i1 < 15; i1++)
                    {
                        int nn = reader.ReadInt32();
                        if (nn < -1 || nn > 2) throw new Exception();
                        else field[i, i1] = nn;
                    }
                }
            }
        }
        catch
        {
            Init(0,-1);
            Debug.Log("Save corrupted!");
            return false;
        }
        Debug.Log("current game mode end " + gameMode);
        return true;
    }

    public static void WriteToFile(string file)
    {
        if (!Directory.Exists(currentDirectory + "\\SavedGames"))
            Directory.CreateDirectory(currentDirectory + "\\SavedGames");

        decorativeGameMode = gameMode;

        File.WriteAllText(file + ".rnj","");

        using (BinaryWriter writer = new BinaryWriter(File.Open(file + ".rnj", FileMode.OpenOrCreate)))
        {
            writer.Write(gameType);
            writer.Write(currentMove);
            writer.Write(decorativeGameMode);

            date = DateTime.Now;

            writer.Write(date.Year);
            writer.Write(date.Month);
            writer.Write(date.Day);
            writer.Write(date.Hour);
            writer.Write(date.Minute);
            writer.Write(date.Second);
            writer.Write(difficulty);
            writer.Write(aggressivness);
            writer.Write(playerMove);

            for (int i = 0; i < 15; i++)
            {
                for (int i1 = 0; i1 < 15; i1++)
                {
                    writer.Write(field[i, i1]);
                }
            }
        }

        isSavedRecently = true;
    }

    public static bool IsDraw()
    {
        for (int i = 0; i < 15; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                if (field[i, j] == 0)
                    return false;
            }
        }
        return true;
    }

    public static bool IsStoneOur(int x, int y, int step)
    {
        if (step == -1) step = currentMove;
        if ((field[x, y] == -1 && step % 2 == 0) || (field[x, y] == 1 && step % 2 == 1) || field[x, y] == 2)
            return true;
        return false;
    }

    public static bool IsVictory(int step=-1)
    {
        int n = 0;

        if (step == -1) step = currentMove;

        // горизонталь

        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                if (IsStoneOur(i, j,step))
                    n++;
                else
                {
                    if (n == 5) return true;
                    else if (n > 5 && gameType == 0) return true;
                    n = 0;
                }
            }
            if (n == 5) return true;
            else if (n > 5 && gameType == 0) return true;
            n = 0;
        }

        // вертикаль

        n = 0;

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (IsStoneOur(j, i, step))
                    n++;
                else
                {
                    if (n == 5) return true;
                    else if (n > 5 && gameType == 0) return true;
                    n = 0;
                }
            }
            if (n == 5) return true;
            else if (n > 5 && gameType == 0) return true;
            n = 0;
        }

        // главная диагональ

        n = 0;

        for (int w = 0; w< 15; w++)
        {
            for (int i = w, j =0 ; i < 15; i++,j++)
            {
                if (IsStoneOur(i, j, step))
                    n++;
                else
                {
                    if (n == 5) return true;
                    else if (n > 5 && gameType == 0) return true;
                    n = 0;
                }
            }
        }

        n = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = 0, j = w; j < 15; i++, j++)
            {
                if (IsStoneOur(i, j, step))
                    n++;
                else
                {
                    if (n == 5) return true;
                    else if (n > 5 && gameType == 0) return true;
                    n = 0;
                }
            }
        }

        // побочная диагональ

        n = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = w, j = 14; i < 15 && j>=0; i++, j--)
            {
                if (IsStoneOur(j,i, step))
                    n++;
                else
                {
                    if (n == 5) return true;
                    else if (n > 5 && gameType == 0) return true;
                    n = 0;
                }
            }
            if (n == 5) return true;
            else if (n > 5 && gameType == 0) return true;
            n = 0;
        }

        n = 0;

        for (int w = 0; w < 15; w++)
        {
            for (int i = 0, j = 14-w; i < 15 && j >= 0; i++, j--)
            {
                if (IsStoneOur(j, i, step))
                    n++;
                else
                {
                    if (n == 5) return true;
                    else if (n > 5 && gameType == 0) return true;
                    n = 0;
                }
            }
            if (n == 5) return true;
            else if (n > 5 && gameType == 0) return true;
            n = 0;
        }

        return false;
    }
}

