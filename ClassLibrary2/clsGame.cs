using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gameEngine
{
    public enum Direction { any, none, northSouth, eastWest, north, south, east, west }

    class clsGame
    {
        public int size;
        public clsMap map;
        public List<clsGamePiece> cars;

        public clsGame(int size)
        {
            this.size = size;
            map = new clsMap(size);

            cars = new List<clsGamePiece>();
            cars.Add(new clsGamePiece(new Vector2(14, 5), "car"));
            cars.Add(new clsGamePiece(new Vector2(2, 6), "car"));
            cars.Add(new clsGamePiece(new Vector2(7, 7), "car"));
            cars.Add(new clsGamePiece(new Vector2(5, 8), "car"));
        }

        public void run()
        {
            foreach (clsCar car in cars)
            {
                car.squareLocation.X--;
                if (car.squareLocation.X < 0) car.squareLocation.X = 14;
            }
        }


    }
}
