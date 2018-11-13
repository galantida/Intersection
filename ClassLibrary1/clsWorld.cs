using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public class clsWorld
    {
        public List<intGamePiece> gamePieces;
        public clsSquare[,] squares;

        public Random rnd;

        public clsInput input;

        public List<intDriver> drivers = new List<intDriver>();


        public clsWorld(long tilesWide, clsInput input)
        {
            // randomize
            rnd = new Random();

            this.input = input;

            // create the map
            loadSquares(tilesWide);
            loadGamePieces();
        }

        public void update()
        {
            // process each driver
            for (int t = 0; t < drivers.Count; t++)
            {
                drivers[t].update();
            }

            // process each game piece
            for (int t = 0; t < gamePieces.Count; t++)
            {
                gamePieces[t].update();
            }
        }

        public Vector2 worldToSquare(Vector2 worldCoordinate)
        {
            return new Vector2(worldCoordinate.X / 64, worldCoordinate.Y / 64);
        }

        public Vector2 squareToWorld(Vector2 squareCoordinate)
        {
            return new Vector2(squareCoordinate.X * 64 + 32, squareCoordinate.Y * 64 + 32);
        }



        public void loadSquares(long tilesWide, int tileSize = 64)
        {
            squares = new clsSquare[tilesWide, tilesWide];
            long x, y;

            // grass
            for (y = 0; y < tilesWide; y++)
            {
                for (x = 0; x < tilesWide; x++)
                {
                    squares[x, y] = new clsSquare(false, false, false, false);
                }
            }

            // east west road
            y = tilesWide / 2;
            for (x = 0; x < tilesWide; x++)
            {
                squares[x, y - 1] = new clsSquare(false, true, false, false);
                squares[x, y] = new clsSquare(true, false, false, false);
            }

            // north south road
            x = (tilesWide / 2);
            for (y = 0; y < tilesWide; y++)
            {
                squares[x, y] = new clsSquare(false, false, true, false);
                squares[x - 1, y] = new clsSquare(false, false, false, true);
            }

            // intersection
            squares[(tilesWide / 2) - 1, (tilesWide / 2)] = new clsSquare(true, false, false, true); // southwest
            squares[(tilesWide / 2), (tilesWide / 2) - 1] = new clsSquare(false, true, true, false); // northeast
            squares[(tilesWide / 2) - 1, (tilesWide / 2) - 1] = new clsSquare(false, true, false, true); // northwest
            squares[(tilesWide / 2), (tilesWide / 2)] = new clsSquare(true, false, true, false); // southeast
        }

        public void loadGamePieces()
        {
            // create container for game pieces
            gamePieces = new List<intGamePiece>();

            // create entry points
            createEntry(new Vector2(6,0), new Vector2(0, 1), GamePieceType.car, 10000);
            createEntry(new Vector2(7, 13), new Vector2(0, -1), GamePieceType.car, 10000);
            createEntry(new Vector2(13, 6), new Vector2(-1, 0), GamePieceType.car, 10000);
            createEntry(new Vector2(0, 7), new Vector2(1, 0), GamePieceType.car, 10000);



            // create exit points
            clsExit exit = createExit(new Vector2(7, 0));
            createExit(new Vector2(6, 13));
            createExit(new Vector2(13, 7));
            createExit(new Vector2(0, 6));

            
            clsCar car = createCar(new Vector2(6, 6), new Vector2(-1, 0), new Vector2(0, 0));
            clsHuman human = new clsHuman(car, input);
            drivers.Add(human);
        }

        public void removeGamePiece(intGamePiece gamePiece)
        {
            gamePieces.Remove(gamePiece);
        }

        public bool inWorldBounds(Vector2 squareCoordinate)
        {
            bool result = true;
            if ((squareCoordinate.X < 0) || (squareCoordinate.X >= squares.GetLength(0))) result = false; ;
            if ((squareCoordinate.Y < 0) || (squareCoordinate.Y >= squares.GetLength(1))) result = false; ;
            return result;
        }

        public clsEntry createEntry(Vector2 square, Vector2 direction, GamePieceType gamePieceType, int maxSpawnTime)
        {
            clsEntry entry = new clsEntry(this, squareToWorld(square), direction, gamePieceType, maxSpawnTime);
            gamePieces.Add(entry);
            return entry;
        }

        public clsExit createExit(Vector2 square)
        {
            clsExit exit = new clsExit(this, squareToWorld(square), new Vector2(0, 1));
            gamePieces.Add(exit);
            return exit;
        }

        public clsCar createCar(Vector2 location, Vector2 direction, Vector2 velocity)
        {
            clsCar car = new clsCar(this, location, direction, velocity);
            gamePieces.Add(car);
            return car;
        }


    }
}
