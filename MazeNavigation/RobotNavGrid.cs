using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    public class RobotNavGrid<T>
    {
        private char[,] _grid;
        private Agent agent;
        private GoalState goalState1;
        private GoalState goalState2;
        private List<GridWall> gridWalls;
        private int mDistance;
        private int n;
        private int m;

        public RobotNavGrid(int n, int m, Agent agent, GoalState goalState1, GoalState goalState2, List<GridWall> gridWalls)
        {
            _grid = new char[n, m];
            this.agent = agent;
            this.goalState1 = goalState1;
            this.goalState2 = goalState2;
            this.gridWalls = gridWalls;
            this.n = n;
            this.m = m;
        }

        public void PrintGrid() // x-width-columns-j | y-height-rows-i
        {
            Console.WriteLine("\n    SEARCHED GRID  ");

            for (int i = 0; i < Grid.GetLength(0); i++) // rows
            {
                for (int j = 0; j < Grid.GetLength(1); j++) // column 
                {
                    if (i == agent.Y && j == agent.X || i == goalState1.Y && j == goalState1.X || i == goalState2.Y && j == goalState2.X)
                    {
                        Grid[agent.Y, agent.X] = 'a';
                        Grid[goalState1.Y, goalState1.X] = 'b';
                        Grid[goalState2.Y, goalState2.X] = 'c';
                    }
                    else
                    {
                        //Grid[i, j] = ' ';
                    }

                    foreach (var wall in gridWalls) // height is different y index but same x index - loop height -> nested loop to fill out the width -> go to next height -> loop to fill out width -> continue until count value = height
                    {
                        int w = wall.X; // width (columns)
                        int h = wall.Y; // height (row)

                        int originalWidth = w;

                        for (int heightCount = 0; heightCount < wall.Height; heightCount++)
                        {
                            for (int widthCount = 0; widthCount < wall.Width; widthCount++)
                            {
                                Grid[h, w] = '#'; // wall.y = rows (height), wall.x = columns (width)
                                w++;
                            }

                            h++;
                            w = originalWidth;
                        }
                    }

                    Console.Write("{0} ", Grid[i, j]);
                }

                Console.WriteLine(); // makes new line on grid
            }
        }

        public void ResetMap() // x-width-columns-j | y-height-rows-i
        {
            for (int i = 0; i < Grid.GetLength(0); i++) // rows
            {
                for (int j = 0; j < Grid.GetLength(1); j++) // column 
                {
                    if (i == agent.Y && j == agent.X || i == goalState1.Y && j == goalState1.X || i == goalState2.Y && j == goalState2.X)
                    {
                        Grid[agent.Y, agent.X] = 'a';
                        Grid[goalState1.Y, goalState1.X] = 'b';
                        Grid[goalState2.Y, goalState2.X] = 'c';
                    }
                    else
                    {
                        Grid[i, j] = ' ';
                    }

                    foreach (var wall in gridWalls) // height is different y index but same x index - loop height -> nested loop to fill out the width -> go to next height -> loop to fill out width -> continue until count value = height
                    {
                        int w = wall.X; // width (columns)
                        int h = wall.Y; // height (row)

                        int originalWidth = w;

                        for (int heightCount = 0; heightCount < wall.Height; heightCount++)
                        {
                            for (int widthCount = 0; widthCount < wall.Width; widthCount++)
                            {
                                Grid[h, w] = '#'; // wall.y = rows (height), wall.x = columns (width)
                                w++;
                            }

                            h++;
                            w = originalWidth;
                        }
                    }
                }

                Console.WriteLine(); // makes new line on grid
            }
        }

        public void PrintMap() // x-width-columns-j | y-height-rows-i
        {
            Console.WriteLine("     EMPTY GRID  ");

            for (int i = 0; i < Grid.GetLength(0); i++) // rows
            {
                for (int j = 0; j < Grid.GetLength(1); j++) // column 
                {
                    if (i == agent.Y && j == agent.X || i == goalState1.Y && j == goalState1.X || i == goalState2.Y && j == goalState2.X)
                    {
                        Grid[agent.Y, agent.X] = 'a';
                        Grid[goalState1.Y, goalState1.X] = 'b';
                        Grid[goalState2.Y, goalState2.X] = 'c';
                    }
                    else
                    {
                        Grid[i, j] = ' ';
                    }

                    foreach (var wall in gridWalls) // height is different y index but same x index - loop height -> nested loop to fill out the width -> go to next height -> loop to fill out width -> continue until count value = height
                    {
                        int w = wall.X; // width (columns)
                        int h = wall.Y; // height (row)

                        int originalWidth = w;

                        for (int heightCount = 0; heightCount < wall.Height; heightCount++)
                        {
                            for (int widthCount = 0; widthCount < wall.Width; widthCount++)
                            {
                                Grid[h, w] = '#'; // wall.y = rows (height), wall.x = columns (width)
                                w++;
                            }

                            h++;
                            w = originalWidth;
                        }
                    }

                    Console.Write("{0} ", Grid[i, j]);
                }

                Console.WriteLine(); // makes new line on grid
            }
        }

        public void DisplaySolution(List<Pair> path, int searched, int discovered, string direction) // prints the matrix indexes here e.g (path: row, column => row, colum => etc...)
        {
            Console.WriteLine($"\ndiscoverd {discovered} nodes and explored {searched} of them \nshortest path length is {path.Count - 1}");
            Console.Write($"shortest path is: {direction}");

            ResetMap();
        }

        public char[,] Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

    }
}
