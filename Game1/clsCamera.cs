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
        public Vector2 worldLocation;
        public Vector2 viewSize;

        public List<intGamePiece> gamePieces;


        public Vector2 topLeft; // world corrdinates of top left corner of visible map.
        public clsSquare[,] squares;


        public List<string> text;
        //public List<Vector4> lines = new List<Vector4>();
        public List<clsWaypointLine> lines = new List<clsWaypointLine>();


        public clsCamera(clsWorld world, Vector2 worldLocation, Vector2 viewSize)
        {
            this.world = world;
            this.worldLocation = worldLocation;
            this.viewSize = viewSize;
        }

        public void update()
        {
            // filter game pieces and tiles down to viewable items 
            this.gamePieces = world.gamePieces;

            // over lay
            text = new List<string>();
            foreach (intGamePiece g in this.gamePieces)
            {if (g.gamePieceType == GamePieceType.car)
                {
                    clsGamePieceCar c = (clsGamePieceCar)g;
                    text.Add("----------");
                    //text.Add("Location : " + c.location);
                    //text.Add("Mass : " + c.mass);
                    //text.Add("Velocity : " + c.velocity);
                    //text.Add("Direction : " + c.direction);
                    //text.Add("pedals : " + c.pedals);
                    text.Add("Steering : " + c.steering);
                    //text.Add("Shifter : " + c.shifter);
                    //text.Add("Speed : " + c.velocity.Length());
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
