using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    class Program
    {
        // place .txt files in "Assignment1\bin\Debug"
        static void Main(string[] args)
        {
            int N = 0; // rows
            int M = 0; // collumns
            char[] delimiterChars = { '[', ']', ',', '(', ')', '|' }; // char array removes characters that aren't values
            string[] value;

            Agent agent;
            RobotNavGrid<char[,]> grid;
            GoalState goalState1;
            GoalState goalState2;

            List<GridWall> gridWalls;

            bool fileNotValid = true;
            string fileName = string.Empty;

            while (fileNotValid)
            {
                Console.Write("Enter tescase: ");
                fileName = Console.ReadLine();

                if (File.Exists(fileName))
                {
                    Console.WriteLine();
                    fileNotValid = false;
                }
                else
                {
                    Console.WriteLine("File not found. \n");
                }
            }

            bool loop = true;
            while (loop)
            {
                string rowsAndColumns = File.ReadLines(fileName).First();
                value = rowsAndColumns.Split(delimiterChars);
                N = Int32.Parse(value[1]); // assign row values in text file to N
                M = Int32.Parse(value[2]); // assign column values in text file to M

                string agentInitialState = File.ReadLines(fileName).ElementAt(1);
                value = agentInitialState.Split(delimiterChars);
                agent = new Agent(Int32.Parse(value[1]), Int32.Parse(value[2])); // get agent x 'columns' and y 'rows' coordinates

                string goalState = File.ReadLines(fileName).ElementAt(2); // get goal x and y coordinates
                value = goalState.Split(delimiterChars);
                goalState1 = new GoalState(Int32.Parse(value[1]), Int32.Parse(value[2]));
                goalState2 = new GoalState(Int32.Parse(value[5]), Int32.Parse(value[6]));

                gridWalls = GetWalls(delimiterChars, fileName);

                grid = new RobotNavGrid<char[,]>(N, M, agent, goalState1, goalState2, gridWalls);

                grid.PrintMap(); // display a visual of the grid
                Console.WriteLine("\nUNINFORMED SEARCH TYPES: \n1. Depth-First Search \n2. Breadth-First Search");
                Console.WriteLine("\nINFORMED SEARCH TYPES: \n3. Greedy Best-First \n4. A* (“A Star”)");
                Console.WriteLine("\n5. Change Test Case");
                Console.Write("\nChoice: ");
                int userinput = Convert.ToInt32(Console.ReadLine());

                switch (userinput)
                {
                    case 1:
                        DepthFirstSearch dfs = new DepthFirstSearch(N, M);
                        dfs.DFS(grid, agent, goalState1, gridWalls);
                        Console.ReadLine();
                        loop = true; break;

                    case 2:
                        BreadthFirstSearch bfs = new BreadthFirstSearch(N, M);
                        bfs.BFS(grid, agent, goalState1, gridWalls);
                        Console.ReadLine();
                        loop = true; break;

                    case 3:
                        GreedyBestFirstSearch gbfs = new GreedyBestFirstSearch(N, M);
                        gbfs.GBFS(grid, agent, goalState1, gridWalls);
                        Console.ReadLine();
                        loop = true; break;

                    case 4:
                        AStarSearch aStar = new AStarSearch(N, M);
                        aStar.AS(grid, agent, goalState1, gridWalls);
                        Console.ReadLine();
                        loop = true; break;

                    case 5:
                        fileNotValid = true;
                        while (fileNotValid)
                        {
                            Console.Write("Enter tescase: ");
                            fileName = Console.ReadLine();

                            if (File.Exists(fileName))
                            {
                                Console.WriteLine();
                                fileNotValid = false;
                            }
                            else
                            {
                                Console.WriteLine("File not found. \n");
                            }
                        }

                        Console.WriteLine();
                        loop = true; break;

                    case 6: Console.WriteLine("Goodbye~"); loop = false; break;
                }
            }

            Console.ReadLine();
        }

        static void IniateValues(string fileName)
        {

        }

        static List<GridWall> GetWalls(char[] delimiterChars, string file) // returns a list of wall information (x, y, width, height)
        {
            List<GridWall> wallValues = new List<GridWall>(); // creates a list of walls that will be returned to the main function
            int x; int y;
            int width; int height;

            try
            {
                StreamReader reader = File.OpenText(file);

                if (File.Exists(file))
                {
                    string line = "";
                    int i = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        i++; // skips the lines with data for NxM grid, agent position, and both goal state positions
                        if (i > 3)
                        {
                            string[] value = line.Split(delimiterChars);

                            x = Int32.Parse(value[1]);
                            y = Int32.Parse(value[2]);
                            width = Int32.Parse(value[3]);
                            height = Int32.Parse(value[4]);

                            wallValues.Add(new GridWall(x, y, width, height));
                        }
                    }

                }
                reader.Close(); // closes file
            }
            catch (Exception e) // FileNotFoundException
            {
                Console.WriteLine($"\n{e.Message}");
            }

            return wallValues; // returns a list of gridwalls
        }
    }
}
