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
        // a camera refind area in the world.
        // It can be used as a container tha hold the tiles and objects that are viewable in that area

        // properties
        public Rectangle visibleArea { get; set; }// world coordinates of top left corner of visible map.

        // camera captured objects in order of display depth
        public List<string> text { get; set; } // camera over lay text
        public List<intObject> visibleObjects { get; set; }
        public List<clsLine> lines { get; set; } // lines above the tiles but below the objects

        // privates
        public clsRoadWorld world { get; }

        public clsCamera(clsRoadWorld world, Rectangle visibleArea)
        {
            this.world = world;
            this.visibleArea = visibleArea;

            // init objects
            this.lines = new List<clsLine>();
        }

        public void update()
        {

            if (world.input.isActivated(InputActionNames.ZoomIn))
            {
                int v = 64;
                visibleArea = new Rectangle(visibleArea.X, visibleArea.Y, visibleArea.Width - v, visibleArea.Height - v);
            }

            if (world.input.isActivated(InputActionNames.ZoomOut))
            {
                int v = 64;
                visibleArea = new Rectangle(visibleArea.X, visibleArea.Y, visibleArea.Width + v, visibleArea.Height + v);
            }


            // filter world objects to viewable items 
            visibleObjects = new List<intObject>();
            foreach (intObject o in this.world.worldObjects)
            {
                if (this.isVisible(o.location))
                {
                    visibleObjects.Add(o);
                }
            }


            // camera overlay
            lines = new List<clsLine>();
            text = new List<string>();
            foreach (intActor driver in world.actors)
            {
                clsDriverHuman driverHuman = driver as clsDriverHuman;   // Here is where I get typeof(IA)
                if (driverHuman != null)
                {
                    text.Add("--------------------");
                    text.Add("Name : " + driverHuman.car.name);
                    text.Add("Speed : " + Math.Floor(driverHuman.car.mph) + " MPH");
                    //text.Add("Collisions : " + c.collisions.Count);
                    //text.Add("Direction : " + c.cardinalDirection.ToString());
                    //text.Add("Coordinates : ( " + c.location.X + " , " + c.location.Y + " )");
                    //text.Add("velocity : " + c.velocity.Length());
                    //text.Add("Steering : " + c.steeringWheel);
                    text.Add("Shifter : " + driverHuman.car.shifter.ToString().ToUpper());
                    text.Add("Acceleerator Pedal : " + driverHuman.car.acceleratorPedal);
                    text.Add("Break Pedal : " + driverHuman.car.breakPedal);
                }

                // route lines
                clsDriverAI driverAI = driver as clsDriverAI;
                if (driverAI != null)
                {
                    text.Add("--------------------");
                    text.Add("Name : " + driverAI.car.name);
                    text.Add("Speed : " + Math.Floor(driverAI.car.mph) + " MPH");
                    //text.Add("Collisions : " + c.collisions.Count);
                    //if (driverAI.route != null) text.Add("DistanceToNextObstruction : " + driverAI.route.distanceToNextObstruction);
                    //text.Add("Direction : " + c.cardinalDirection.ToString());
                    //text.Add("Coordinates : ( " + c.location.X + " , " + c.location.Y + " )");
                    //text.Add("velocity : " + c.velocity.Length());
                    //text.Add("Steering : " + c.steeringWheel);
                    //text.Add("Shifter : " + c.shifter.ToString().ToUpper());
                    //text.Add("Acceleerator Pedal : " + c.acceleratorPedal);
                    //text.Add("Break Pedal : " + c.breakPedal);

                    if (driverAI.route != null)
                    {
                        Vector2 lastWaypointWorldLocation = driverAI.car.location;
                        for (int t = driverAI.route.currentWaypointIndex; t < driverAI.route.waypoints.Count; t++)
                        {
                            Vector2 thisWaypointWorldLocation = world.tileCoordinateToWorldLocation(driverAI.route.waypoints[t]);
                            if (this.isVisible(thisWaypointWorldLocation)) lines.Add(new clsLine(lastWaypointWorldLocation, thisWaypointWorldLocation, driverAI.car.color, 4));
                            lastWaypointWorldLocation = thisWaypointWorldLocation;
                        }
                    }

                }
                
            }
        }

        public bool isVisible(Vector2 location)
        {
            if ((location.X > visibleArea.Left) && (location.Y > visibleArea.Top) && (location.X < visibleArea.Right) && (location.Y < visibleArea.Bottom)) return true;
            else return false;
        }

        public bool visible(Rectangle box)
        {
            if (this.isVisible(new Vector2(box.Left, box.Top))) return true;
            if (this.isVisible(new Vector2(box.Left, box.Bottom))) return true;
            if (this.isVisible(new Vector2(box.Right, box.Top))) return true;
            if (this.isVisible(new Vector2(box.Right, box.Bottom))) return true;
            return false;
        }

        public Rectangle visibleTileArea
        {
            get
            {
                // assume tile world starts at 0,0 but the camera may not
                int left = (int)Math.Floor((decimal)this.visibleArea.Left / (decimal)this.world.tileSize);
                int top = (int)Math.Floor((decimal)this.visibleArea.Top / (decimal)this.world.tileSize);
                int width = (int)Math.Floor(this.visibleArea.Width / (decimal)this.world.tileSize);
                int height = (int)Math.Floor(this.visibleArea.Height / (decimal)this.world.tileSize);
                return new Rectangle(left, top, width, height);
            }
        }
    }
}
