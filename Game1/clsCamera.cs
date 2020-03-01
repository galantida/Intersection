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
        public Vector2 target { get; set; } // world target
        public Vector2 size { get; set; } // world screen size

        // camera captured objects in order of display depth
        public List<string> text { get; set; } // camera over lay text
        public List<intObject> visibleObjects { get; set; }


        // privates
        public clsRoadWorld world { get; }
        
        public clsCamera(clsRoadWorld world, Vector2 target, Vector2 size)
        {
            this.world = world;
            this.target = target;
            this.size = size;
        }

        public void update()
        {
            int zoomScale = 64;
            if (world.input.isActivated(InputActionNames.ZoomIn))
            {
                this.size = new Vector2(this.size.X - zoomScale, this.size.Y - zoomScale);
            }

            if (world.input.isActivated(InputActionNames.ZoomOut))
            {
                this.size = new Vector2(this.size.X + zoomScale, this.size.Y + zoomScale);
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
                }
                
            }
        }

        public Rectangle visibleArea {
            // world coordinates of top left corner of visible map.
            get
            {
                int top = (int)(this.target.Y - (this.size.Y / 2.0f));
                int left = (int)(this.target.X - (this.size.X / 2.0f));
                return new Rectangle(left, top, (int)this.size.X, (int)this.size.Y);
            }
        }

        public Rectangle visibleTileArea
        {
            get
            {
                Rectangle tmpVisibleArea = this.visibleArea; // cache for speed
                // assume tile world starts at 0,0 but the camera may not
                int left = (int)Math.Floor((decimal)tmpVisibleArea.Left / (decimal)this.world.tileSize);
                //if (left < 0) left = 0;

                int top = (int)Math.Floor((decimal)tmpVisibleArea.Top / (decimal)this.world.tileSize);
                //if (top < 0) top = 0;

                int width = (int)Math.Floor(tmpVisibleArea.Width / (decimal)this.world.tileSize);
                int height = (int)Math.Floor(tmpVisibleArea.Height / (decimal)this.world.tileSize);
                return new Rectangle(left, top, width, height);
            }
        }

        public bool isVisible(Vector2 location)
        {
            Rectangle tmpVisibleArea = this.visibleArea; // cache for speed
            if ((location.X >= tmpVisibleArea.Left) && (location.Y >= tmpVisibleArea.Top) && (location.X <= tmpVisibleArea.Right) && (location.Y <= tmpVisibleArea.Bottom)) return true;
            else return false;
        }
    }
}
