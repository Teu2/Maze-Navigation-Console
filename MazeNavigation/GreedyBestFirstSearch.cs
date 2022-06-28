using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    class GreedyBestFirstSearch
    {
        // counts how many nodes have been discovered while searching, and how many have actually been searched
        private int searched;
        private int discovered;

        static int[] dRow = { -1, 0, 1, 0 }; // y axis [top, left, bottom, right]
        static int[] dCol = { 0, -1, 0, 1 }; // x axis [top, left, bottom, right]

        bool[,] visited;

        int N;
        int M;

        public GreedyBestFirstSearch(int n, int m)
        {
            searched = 0;
            discovered = 0;
            visited = new bool[n, m];
            N = n;
            M = m;
        }

        public void GBFS(RobotNavGrid<char[,]> grid, Agent agent, GoalState goalState, List<GridWall> gridWalls) // NOTE: BFS grom goal to every index in the cell works
        {
            int[,] distanceGrid = new int[N, M];

            for (int n = 0; n < N; n++) // rows 
            {
                for (int m = 0; m < M; m++) // columns
                {
                    // assigns each row and column index a heuristic of goal + start manhattan distances
                    distanceGrid[n, m] = ManhattanDistFromGoal(n, m, goalState); // calculates the distance from the grid
                }
            }

            List<Pair> paths = new List<Pair>(); // this stores the path that will be taken from that star to the goal state
            paths.Add(new Pair(agent.Y, agent.X));
            visited[agent.Y, agent.X] = true; // marks the visited state as true so we avoid loops

            while (true)
            {
                if (paths[paths.Count - 1].Row == goalState.Y && paths[paths.Count - 1].Column == goalState.X)
                {
                    break;
                }

                int nearestNeighbour = 0;
                int smallestDistance = (N * M) * 2; // will be used to compare the manhattan distance value

                Pair last = paths[paths.Count - 1];

                for (int i = 0; i < 4; i++) // traverses the grid
                {
                    int adjy = last.Row + dRow[i];
                    int adjx = last.Column + dCol[i];

                    if (!IsValid(grid, visited, adjy, adjx, gridWalls)) continue;

                    if (distanceGrid[adjy, adjx] < smallestDistance) // compares the current cell's manhattan heuristic with the smallestDistance value
                    {
                        discovered++;
                        smallestDistance = distanceGrid[adjy, adjx]; // the condition succeeds the new smallest distance will be the current cell
                        nearestNeighbour = i; // gives us the new neighbour
                    }
                }

                // gives us the neighbour with the smallest distance
                int y = last.Row + dRow[nearestNeighbour];
                int x = last.Column + dCol[nearestNeighbour];

                searched++;
                paths.Add(new Pair(y, x));
                visited[y, x] = true;

                if (IsBlocked(grid, y, x, visited, gridWalls)) // this checks if that path is blocked
                {
                    while (IsBlocked(grid, y, x, visited, gridWalls)) // while the paths are still blocked
                    {
                        paths.RemoveAt(paths.Count - 1); // removes the paths that have been added until we reach the last cell that is free
                        y = paths[paths.Count - 1].Row;
                        x = paths[paths.Count - 1].Column;
                    }
                }
            }

            for (int i = paths.Count - 1; i >= 0; i--) // gets the current index and converts it into a pair 
            {
                grid.Grid[paths[i].Row, paths[i].Column] = '*'; // places a star on the grid representing the shortset path

            }

            string superDirection = ""; // this will hold a string of appended values [up; down; left; right; etc..]

            for (int i = 0; i < paths.Count() - 1; i++) // get's the direction of the printedPath list
            {
                superDirection += GetDirection(paths[i], paths[i + 1]);
                superDirection += "; ";
            }
            superDirection += "\b ";


            grid.PrintGrid();
            grid.DisplaySolution(paths, searched, discovered, superDirection); // prints the shortest path
        }

        private string GetDirection(Pair current, Pair next) // returns a string of which direction the path went
        {
            string Direction = " ";

            if (next.Row == current.Row - 1 && next.Column == current.Column)
            {
                Direction = "Up";
            }
            if (next.Row == current.Row + 1 && next.Column == current.Column)
            {
                Direction = "Down";
            }
            if (next.Row == current.Row && next.Column == current.Column - 1)
            {
                Direction = "Left";
            }
            if (next.Row == current.Row && next.Column == current.Column + 1)
            {
                Direction = "Right";
            }

            return Direction;
        }

        private bool IsBlocked(RobotNavGrid<char[,]> grid, int adjy, int adjx, bool[,] visited, List<GridWall> gridWalls)
        {
            for (int i = 0; i < 4; i++)
            {
                int y = adjy + dRow[i];
                int x = adjx + dCol[i];

                if (IsValid(grid, visited, y, x, gridWalls))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValid(RobotNavGrid<char[,]> grid, bool[,] visited, int row, int col, List<GridWall> gridWalls)
        {
            if (row < 0 || col < 0 || row >= visited.GetLength(0) || col >= visited.GetLength(1)) // If cell lies out of bounds
            {
                return false;
            }

            if (visited[row, col]) // If cell is already visited
            {
                return false;
            }

            foreach (var wall in gridWalls) // checks for walls
            {
                if (row == wall.Y && col == wall.X)
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }

            foreach (var gridChar in grid.Grid)
            {
                if (grid.Grid[row, col] == '#')
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }

            return true; // Otherwise
        }

        static int ManhattanDistFromGoal(int N, int M, GoalState goal) // returns the manhattan distance from the goal to the row and colum index (n & m)
        {
            int distance = Math.Abs(M - goal.X) + Math.Abs(N - goal.Y);
            return distance;
        }
    }
}
