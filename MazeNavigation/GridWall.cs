using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    public class GridWall : IGridCellCoordinates
    {
        private int x;
        private int y;
        private int width;
        private int height;

        public GridWall(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Height { get { return height; } }
        public int Width { get { return width; } }
    }
}
