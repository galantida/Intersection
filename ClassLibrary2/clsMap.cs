using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameEngine
{
    public class clsMap
    {
        public clsSquare[,] squares;

        public clsMap(int size)
        {
            LoadMap(size);
        }

        private void LoadMap(int size)
        {
            squares = new clsSquare[size, size];

            int x, y;
            for (y = 0; y < size; y++)
            {
                for (x = 0; x < size; x++)
                {
                    squares[x, y] = new clsSquare(Direction.any, false);
                }
            }

            y = size / 2;
            for (x = 0; x < size; x++)
            {
                squares[x, y-1] = new clsSquare(Direction.west, true);
                squares[x, y] = new clsSquare(Direction.east, true);
            }

            x = (size / 2);
            for (y = 0; y < size; y++)
            {
                squares[x , y] = new clsSquare(Direction.north, true);
                squares[x-1, y] = new clsSquare(Direction.south, true);
            }


            squares[(size / 2) - 1, (size / 2)] = new clsSquare(Direction.any, true);
            squares[(size / 2), (size / 2) - 1] = new clsSquare(Direction.any, true);
            squares[(size / 2) - 1, (size / 2) - 1] = new clsSquare(Direction.any, true);
            squares[(size / 2), (size / 2)] = new clsSquare(Direction.any, true);
        }


    }
}
