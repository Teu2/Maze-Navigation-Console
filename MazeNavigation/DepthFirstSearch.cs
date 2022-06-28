using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    public class DepthFirstSearch // stack
    {
        // counts how many nodes have been discovered while searching, and how many have actually been searched
        private int searched;
        private int discovered;

        static int[] dRow = { -1, 0, 1, 0 }; // y axis [top, left, bottom, right]
        static int[] dCol = { 0, -1, 0, 1 }; // x axis [top, left, bottom, right]

        int n;
        int m;

        bool[,] visited;

        public DepthFirstSearch(int n, int m)
        {
            searched = 0;
            discovered = 0;
            visited = new bool[n, m];
            this.n = n;
            this.m = m;
        }

        public void DFS(RobotNavGrid<char[,]> grid, Agent agent, GoalState goalState, List<GridWall> gridWalls) // x-width-columns-j | y-height-rows-i // this converts 2d array cells to indexes for the adjacency list
        {
            Stack<Pair> cells = new Stack<Pair>();
            string direction;

            var startPair = new Pair(agent.Y, agent.X, "start");
            visited[agent.Y, agent.X] = true; // marking the start cell as visited so we don't return to that state again - keeps track of all visited cells
            cells.Push(startPair); // adds the starting pair to the top of the stack 

            List<List<int>> adj = new List<List<int>>(n * m);
            for (int i = 0; i < n * m; i++) // creates a list of adjacencies
            {
                adj.Add(new List<int>());
            }

            visited[startPair.Row, startPair.Column] = true;
            while (cells.Count != 0)
            {
                Pair pr = cells.Pop();
                int index = PairToIndex(pr, m);

                for (int i = 0; i < 4; i++) // traverses through the rows and columns
                {
                    int adjy = pr.Row + dRow[i];
                    int adjx = pr.Column + dCol[i];
                    direction = CheckDirection(i);

                    if (isValid2(grid, adjy, adjx, gridWalls)) // doesn't check for walls - ensures that an index can be created for each cell pair
                    {
                        var neighbour = new Pair(adjy, adjx, direction); // Console.WriteLine(neighbour.Message);

                        if (isValid(grid, visited, adjy, adjx, gridWalls))
                        {
                            cells.Push(neighbour);
                            visited[adjy, adjx] = true;
                        }

                        int indexToNeighbour = PairToIndex(neighbour, m); // creates an index for each pair

                        adj[index].Add(indexToNeighbour);
                    }
                }
            }

            // creates indexes for both source and destination to use on the 2d matrix
            int source = PairToIndex(startPair, m);
            int dest = PairToIndex(new Pair(goalState.Y, goalState.X), m);

            GetShortestPath(grid, adj, source, dest, n * m, m);
        }

        private int PairToIndex(Pair pair, int n) // converts pair to index for the adjacency list
        {
            return pair.Column + (n * pair.Row);
        }

        private Pair IndexToPair(int idx, int n) // converts the index to return a pair to print the final solution
        {
            return new Pair(idx / n, idx % n);
        }

        private void GetShortestPath(RobotNavGrid<char[,]> grid, List<List<int>> adj, int source, int destination, int rowsAndColumns, int m)
        {
            int[] previous = new int[rowsAndColumns];

            if (DFS(adj, source, destination, rowsAndColumns, previous) == false)
            {
                Console.WriteLine("No solution found");
                return;
            }

            List<int> path = new List<int>(); // List to store path
            int traversal = destination;
            path.Add(traversal);

            while (previous[traversal] != -1) // adding values to the list - the indexes for the shortest path
            {
                //Console.WriteLine($"previous[traversal] = {previous[traversal]}");
                path.Add(previous[traversal]);
                traversal = previous[traversal];
            }

            List<Pair> printedPath = new List<Pair>();

            for (int i = path.Count - 1; i >= 0; i--) // gets the current index and converts it into a pair 
            {
                var shortestPath = IndexToPair(path[i], m); // assigns shortestPath a type of Pair with rows and columns using index added in the path list
                grid.Grid[shortestPath.Row, shortestPath.Column] = '*'; // places a star on the grid representing the shortset path

                printedPath.Add(shortestPath);
            }

            string superDirection = ""; // this will hold a string of appended values [up; down; left; right; etc..]

            for (int i = 0; i < printedPath.Count() - 1; i++) // get's the direction of the printedPath list
            {
                superDirection += GetDirection(printedPath[i], printedPath[i + 1]);
                superDirection += "; ";
            }
            superDirection += "\b ";

            grid.PrintGrid();
            grid.DisplaySolution(printedPath, searched, discovered, superDirection); // prints the shortest path
        }

        private string GetDirection(Pair current, Pair next)
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

        private bool DFS(List<List<int>> adj, int src, int dest, int v, int[] previous)
        {
            Stack<int> frontier = new Stack<int>(); // holds the frontier - stack since it is DFS - LIFO

            bool[] visited = new bool[v]; // bool array that checks whether or not the vertex has been reached

            for (int i = 0; i < v; i++) // all vertices set to false  since no vertices have been visited
            {
                visited[i] = false;
                previous[i] = -1;
            }

            visited[src] = true; // marks the source index as visited
            frontier.Push(src);

            while (frontier.Count != 0) // starts the DFS search everything is the same as BFS except we are popping and pushing the frontier instead of enqueuing and dequeuing
            {
                int s = frontier.Peek();
                frontier.Pop();

                searched++;

                for (int i = 0; i < adj[s].Count; i++)
                {
                    if (visited[adj[s][i]] == false)
                    {
                        visited[adj[s][i]] = true;
                        previous[adj[s][i]] = s;
                        discovered++;
                        frontier.Push(adj[s][i]);

                        if (adj[s][i] == dest) // returns true if we find the destination
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool isValid2(RobotNavGrid<char[,]> grid, int row, int col, List<GridWall> gridWalls)
        {
            if (row < 0 || col < 0 || row >= n || col >= m) // If cell lies out of bounds
            {
                return false;
            }

            foreach (var gridChar in grid.Grid) // checks for walls
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

            return true;
        }

        private bool isValid(RobotNavGrid<char[,]> grid, bool[,] visited, int row, int column, List<GridWall> gridWalls)
        {
            if (row < 0 || column < 0 || row >= visited.GetLength(0) || column >= visited.GetLength(1)) // If cell lies out of bounds
            {
                return false;
            }

            if (visited[row, column]) // If cell is already visited
            {
                return false;
            }

            foreach (var gridChar in grid.Grid)
            {
                if (grid.Grid[row, column] == '#')
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

        private string CheckDirection(int i)
        {
            string direction = "none";

            if (i == 0)
            {
                direction = "up";
            }
            else if (i == 1)
            {
                direction = "right";
            }
            else if (i == 2)
            {
                direction = "bottom";
            }
            else if (i == 3)
            {
                direction = "left";
            }

            return direction;
        }
    }
}
