using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using gameLogic;

namespace Game1
{
    public class clsCamera
    {
        private clsWorld world;
        public Rectangle visibleArea;

        public List<intGamePiece> gamePieces;


        public Vector2 topLeft; // world corrdinates of top left corner of visible map.
        public clsSquare[,] squares;


        public List<string> text;
        //public List<Vector4> lines = new List<Vector4>();
        public List<clsWaypointLine> lines = new List<clsWaypointLine>();


        public clsCamera(clsWorld world, Rectangle visibleArea)
        {
            this.world = world;
            this.visibleArea = visibleArea;
        }

        public void update()
        {
            // filter game pieces and tiles down to viewable items 
            this.gamePieces = world.gamePieces;

            // overlay
            text = new List<string>();
            foreach (intGamePiece g in this.gamePieces)
            {if (g.gamePieceType == GamePieceType.car)
                {
                    clsCarObject c = (clsCarObject)g;
                    text.Add("----------");
                    text.Add("Steering : " + c.steeringWheel);
                    text.Add("Shifter : " + c.shifter.ToString().ToUpper());
                    text.Add("Acceleerator Pedal : " + c.acceleratorPedal);
                    text.Add("Break Pedal : " + c.breakPedal);
                    text.Add("Speed : " + Math.Floor(c.speed) + " MPH");
                    text.Add("Direction : " + c.cardinalDirection.ToString());
                    text.Add("Coordinates : ( " + c.location.X + " , " + c.location.Y + " )");
                }

            }

            topLeft = new Vector2(0, 0);
            this.squares = world.squares;

            /*

            // lines
            int x = 0;
            int y = 0;
            //lines = new List<Vector4>();
            lines = new List<clsWaypointLine>();
            foreach (intDriver driver in world.drivers)
            {
                try
                {
                    clsDriverAI ai = (clsDriverAI)driver;
                    lines.Add(new clsWaypointLine(ai.car.location, world.squareCoordinateToWorldLocation(ai.route.currentWaypoint)));
                    x += 25;
                    y += 25;
                }
                catch (Exception ex)
                {

                }
            }
            */


        }
    }
}
