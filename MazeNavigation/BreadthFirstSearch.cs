using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    class BreadthFirstSearch
    {
        // counts how many nodes have been discovered while searching, and how many have actually been searched
        private int searched;
        private int discovered;

        static int[] dRow = { -1, 0, 1, 0 }; // y axis [top, left, bottom, right]
        static int[] dCol = { 0, -1, 0, 1 }; // x axis [top, left, bottom, right]

        int n;
        int m;

        bool[,] visited;

        public BreadthFirstSearch(int n, int m)
        {
            searched = 0;
            discovered = 0;
            visited = new bool[n, m];
            this.n = n;
            this.m = m;
        }

        public void BFS(RobotNavGrid<char[,]> grid, Agent agent, GoalState goalState, List<GridWall> gridWalls) // x-width-columns-j | y-height-rows-i // this converts 2d array cells to indexes for the adjacency list
        {
            Queue<Pair> cells = new Queue<Pair>();

            var startPair = new Pair(agent.Y, agent.X, "start");
            visited[agent.Y, agent.X] = true; // marking the start cell as visited so we don't return to that state again - keeps track of all visited cells
            cells.Enqueue(startPair); // Mark the starting cell as visited and push it into the queue

            List<List<int>> adj = new List<List<int>>(n * m); // creates list of adjacencies
            for (int i = 0; i < n * m; i++)
            {
                adj.Add(new List<int>());
            }
            
            visited[startPair.Row, startPair.Column] = true;
            while (cells.Count != 0)
            {
                Pair pr = cells.Dequeue();
                int index = PairToIndex(pr, m); // assigns an index value that results from pr.y & x and m (columns)
                for (int i = 0; i < 4; i++)
                {
                    int adjy = pr.Row + dRow[i]; // changes the current row value
                    int adjx = pr.Column + dCol[i]; // changes the current column value

                    if (isValid2(grid, adjy, adjx, gridWalls))
                    {
                        var neighbour = new Pair(adjy, adjx); // uses the row and column of the cell queue to create a neighbour

                        if (isValid(grid, visited, adjy, adjx, gridWalls))
                        {
                            cells.Enqueue(neighbour);
                            visited[adjy, adjx] = true;
                        }

                        int indexToNeighbour = PairToIndex(neighbour, m); // the neightbour pair is used to create an index
                        adj[index].Add(indexToNeighbour); // added the index to the adjacency list
                    }
                }
            }

            int source = PairToIndex(startPair, m);
            int dest = PairToIndex(new Pair(goalState.Y, goalState.X), m);

            GetShortestPath(grid, adj, source, dest, n * m, m);
        }

        private int PairToIndex(Pair pair, int m) // converts pair to index for the adjacency list
        {
            return pair.Column + (m * pair.Row);
        }

        private Pair IndexToPair(int idx, int m) // converts the index to return a pair to print the final solution
        {
            return new Pair(idx / m, idx % m);
        }

        private void GetShortestPath(RobotNavGrid<char[,]> grid, List<List<int>> adj, int source, int destination, int rowsAndColumns, int m)
        {
            int[] previous = new int[rowsAndColumns];

            if (BFS(adj, source, destination, rowsAndColumns, previous) == false)
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

        private bool BFS(List<List<int>> adj, int startStateIndex, int goalStateIndex, int gridVertex, int[] previous)
        {
            Queue<int> frontier = new Queue<int>(); // holds the frontier - queue since it is BFS - FIFO

            bool[] visited = new bool[gridVertex]; // bool array that checks whether or not the vertex has been reached

            for (int i = 0; i < gridVertex; i++) // all vertices set to false since no vertices have been visited
            {
                visited[i] = false;
                previous[i] = -1;
            }

            visited[startStateIndex] = true; // marks the source index as visited
            frontier.Enqueue(startStateIndex);

            while (frontier.Count != 0) // starts the BFS search
            {
                int q = frontier.Peek(); // assigns q the value at the beginning of the frontier - the index of a cell pair
                frontier.Dequeue();

                searched++;

                for (int i = 0; i < adj[q].Count; i++)
                {
                    //Console.WriteLine($"adj[q][i] = {adj[q][i]}");
                    if (visited[adj[q][i]] == false)
                    {
                        visited[adj[q][i]] = true;
                        previous[adj[q][i]] = q;
                        discovered++;
                        frontier.Enqueue(adj[q][i]); // enqueues the index value of adj[q][i]

                        if (adj[q][i] == goalStateIndex) // returns true if we find the destination
                        {
                            return true;
                        }
                    }
                }
            }

            return false; // returns false if no goal state is found
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
                //Console.WriteLine($"{row}, {col} has veen visited");
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
    }
}
