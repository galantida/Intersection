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

        // display elements
        public List<intObject> viewableObjects;
        public intTile[,] viewableTiles;
        public List<clsLine> lines = new List<clsLine>();
        public List<string> text;


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

            // lines
            lines = new List<clsLine>();
            foreach (intActor driver in world.actors)
            {
                try
                {
                    // route lines
                    clsDriverAI ai = (clsDriverAI)driver;
                    Vector2 lastWaypointWorldLocation = ai.car.location;
                    for (int t = ai.route.currentWaypointIndex; t < ai.route.waypoints.Count; t++)
                    {
                        Vector2 thisWaypointWorldLocation = world.tileCoordinateToWorldLocation(ai.route.waypoints[t]);
                        lines.Add(new clsLine(lastWaypointWorldLocation, thisWaypointWorldLocation, ai.car.color, 4));
                        lastWaypointWorldLocation = thisWaypointWorldLocation;
                    }

                }
                catch (Exception ex)
                {

                }
            }


            


        }
    }
}
