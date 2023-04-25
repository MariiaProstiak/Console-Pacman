using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyPacman
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            int[] level = StartGame();
            int pacX = 1, pacY = 1, score = 0, maxScore = level[1], nbAdversary = level[0];
            bool isLoose = false;
            int[][] enemysPos = new int[nbAdversary][];
            char[,] map = ReadMap("map.txt", maxScore, nbAdversary, ref enemysPos);
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            Task.Run(() =>
            {
                while (score != maxScore && !isLoose)
                {
                    pressedKey = Console.ReadKey();

                }
            });

            while (score!= maxScore && !isLoose)
            {
                Console.Clear();
                RenderMap(map);
                CreatePackman(pacX, pacY);
                

                Console.CursorVisible = false;

                Console.SetCursorPosition((map.GetLength(1) + 5), 0);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Score: {score}");

                Console.ForegroundColor = ConsoleColor.White;
               
                HandleInput(pressedKey, ref pacX, ref pacY, map, ref score, ref isLoose);
                EnemysRun(enemysPos, map, ref isLoose);
                

               Thread.Sleep(100);
            }
            if (isLoose)
            {
                FinishGame(map, isLoose);
            }
            else
            {
                FinishGame(map, isLoose, pacX, pacY, score);
            }
        }

        static int[] StartGame()
        {
            int res = 0;
            while(res!=1 && res != 2 && res != 3 && res != 4)
            {
                Console.Clear();
                Console.WriteLine("Welcom to MyPacman!");
                Console.WriteLine("Please choose level of complexyty:");
                Console.WriteLine("1 - light");
                Console.WriteLine("2 - medium");
                Console.WriteLine("3 - hard");
                Console.WriteLine("4 - very hard");
                res = Convert.ToInt32(Console.ReadLine());
            }
            
             
            return new int[] { res * 3, res * 5 };
            
        }

        static void FinishGame(char[,] map, bool isLoose, int pacX =0, int pacY = 0, int score = 0)
        {
            Console.Clear();
            RenderMap(map);
            if(!isLoose) CreatePackman(pacX, pacY);

            Console.SetCursorPosition((map.GetLength(1) + 5), 0);
            if (!isLoose)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Congratulations! You are win! Your score is {score}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"Game over! You loose!");
            }
            Console.CursorVisible = true;
            Console.SetCursorPosition(0, (map.GetLength(0) + 2));
        }
        static void EnemysRun(int[][] enemysPos, char[,] map, ref bool isLoose)
        {
            foreach (int[] enemys in enemysPos)
            {
                EnemyMove(ref enemys[0], ref enemys[1], map, ref isLoose);
            }
        }

        static void EnemyMove(ref int posX,  ref int posY, char[,] map, ref bool isLoose)
        {
            
            Random random = new Random();
            int newPosX = posX + random.Next(-1, 2);
            int newPosY = posY + random.Next(-1, 2);
            while(map[newPosX, newPosY] == '#')
            {
                newPosX = posX + random.Next(-1, 2);
                newPosY = posY + random.Next(-1, 2);
            }
            map[posX, posY] = ' ';
            posX = newPosX;
            posY = newPosY;
            map[posX, posY] = 'C';
            if (map[newPosX, newPosY] == '@')
            {
                isLoose = true;
            }
        }

        

        static void HandleInput(ConsoleKeyInfo key, ref int pacX, ref int pacY, char[,] map, ref int score, ref bool isLoose)
        {
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (map[pacY, pacX - 1] != '#')
                    {
                        pacX -= 1;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (map[pacY, pacX + 1] != '#')
                    {
                        pacX += 1;
                    }

                    break;
                case ConsoleKey.UpArrow:
                    if (map[pacY - 1, pacX] != '#')
                    {
                        pacY -= 1;
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (map[pacY + 1, pacX] != '#')
                    {
                        pacY += 1;
                    }
                    break;
                default: break;
            }
            if (map[pacY, pacX] == 'X')
            {
                map[pacY, pacX] = ' ';
                score++;
            }
            if (map[pacY, pacX] == 'C')
            {
                map[pacY, pacX] = 'C';
                isLoose = true;
            }
        }

  
        static void CreatePackman(int positionX, int positionY)
        {
            Console.SetCursorPosition(positionX, positionY);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("@");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void RenderMap(char[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    if (map[x, y] == 'C')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(map[x, y]);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine();
            }
        }
        static void RandomAdversary(char[,] map, int nbAdversary, ref int[][] enemysPos)
        {
            
            Random random = new Random();
            while (nbAdversary != 0)
            {
                int randX = random.Next(1, map.GetLength(0));
                int randY = random.Next(1, map.GetLength(1));
                if (map[randX, randY] == ' ')
                {
                    map[randX, randY] = 'C';
                    enemysPos[nbAdversary - 1] = new int[] { randX, randY };
                    nbAdversary--;
                }
            }
        }
        static void RandomTreasure(char[,] map, int treasureCount)
        {
            Random random = new Random();
            while (treasureCount != 0)
            {
                int randX = random.Next(1, map.GetLength(0));
                int randY = random.Next(1, map.GetLength(1));
                if (map[randX, randY] == ' ')
                {
                    treasureCount--;
                    map[randX, randY] = 'X';
                }
            }

        }

        static char[,] ReadMap(string filePath, int treasures, int nbEnemy, ref int[][] enemysPos)
        {
            string[] file = File.ReadAllLines(filePath);
            char[,] map = new char[GetMaxLengthOfLines(file), file.Length];
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[y][x];

            RandomTreasure(map, treasures);
            RandomAdversary(map, nbEnemy, ref enemysPos);



            return map;
        }

        static int GetMaxLengthOfLines(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (string line in lines)
            {
                if (line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }
            return maxLength;
        }
    }
}
