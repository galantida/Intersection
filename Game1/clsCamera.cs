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
        // a camera is using to define a displayable area
        // and act as a container to hold all of those objects.

        // properties
        public Vector2 target { get; set; }// world corrdinates of top left corner of visible map.
        public Vector2 size { get; set; }// world corrdinates of top left corner of visible map.

        // camera captured objects
        public List<intObject> viewableObjects { get; set; }
        public intTile[,] viewableTiles { get; set; }
        public List<clsLine> lines { get; set; }
        public List<string> text { get; set; }

        // privates
        private clsRoadWorld world;

        public clsCamera(clsRoadWorld world, Vector2 target, Vector2 size)
        {
            this.world = world;
            this.target = target;
            this.size = size;

            // init objects
            this.lines = new List<clsLine>();
        }

        public Rectangle visibleArea
        {
            get
            {
                int topLeftX = (int)target.X - (int)(size.X / 2);
                int topLeftY = (int)target.Y - (int)(size.Y / 2);
                return new Rectangle(topLeftX, topLeftY, (int)size.X, (int)size.Y);
            }
        }

        public void update()
        {
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
