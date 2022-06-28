using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeNavigation
{
    public class Pair : IEquatable<Pair> // stores the indices of matrix cells
    {
        private int row;
        private int column;

        private string message; // stores directions or goal and start strings

        public Pair(int row, int column, string message)
        {
            this.row = row;
            this.column = column;
            this.message = message;
        }

        public Pair(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public bool Equals(Pair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return row == other.row && column == other.column;
        }
    }
}
