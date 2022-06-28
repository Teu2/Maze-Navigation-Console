using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    public class Agent : IGridCellCoordinates
    {
        private int x;
        private int y;

        public Agent(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }
    }
}
