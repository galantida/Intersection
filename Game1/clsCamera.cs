using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using gameLogic;
using tileWorld;
using physicalWorld;

namespace Game1
{
    public class clsCamera
    {
        private clsRoadWorld world;
        public Rectangle visibleArea;

        public Vector2 topLeft; // world corrdinates of top left corner of visible map.


        public List<intObject> viewableObjects;
        public intTile[,] viewableTiles;


        public List<string> text;
        //public List<Vector4> lines = new List<Vector4>();
        public List<clsWaypointLine> lines = new List<clsWaypointLine>();


        public clsCamera(clsRoadWorld world, Rectangle visibleArea)
        {
            this.world = world;
            this.visibleArea = visibleArea;
        }

        public void update()
        {
            topLeft = new Vector2(0, 0);

            // filter game pieces and tiles down to viewable items 
            viewableObjects  = this.world.worldObjects;

            // overlay
            text = new List<string>();
            foreach (intActor a in world.actors)
            {
                clsDriverAI driverAI = a as clsDriverAI;   // Here is where I get typeof(IA)
                if (driverAI != null)
                {
                    text.Add("--------------------");
                    text.Add("Name : " + driverAI.car.name);
                    text.Add("Speed : " + Math.Floor(driverAI.car.mph) + " MPH");
                    //text.Add("Collisions : " + c.collisions.Count);
                    if (driverAI.route!= null) text.Add("DistanceToNextObstruction : " + driverAI.route.distanceToNextObstruction);
                    //text.Add("Direction : " + c.cardinalDirection.ToString());
                    //text.Add("Coordinates : ( " + c.location.X + " , " + c.location.Y + " )");
                    //text.Add("velocity : " + c.velocity.Length());
                    //text.Add("Steering : " + c.steeringWheel);
                    //text.Add("Shifter : " + c.shifter.ToString().ToUpper());
                    //text.Add("Acceleerator Pedal : " + c.acceleratorPedal);
                    //text.Add("Break Pedal : " + c.breakPedal);
                }
            }

            // viewable tiles
            
            viewableTiles = this.world.tiles;

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
