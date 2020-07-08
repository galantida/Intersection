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

namespace renderer
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

        bool inputControled;


        // privates
        public clsRoadWorld world { get; }

        public clsCamera(clsRoadWorld world, Vector2 target, Vector2 size, bool inputControled = false)
        {
            this.world = world;
            this.target = target;
            this.size = size;
            this.inputControled = inputControled;
            
        }

        public void update()
        {
            if (inputControled)
            {
                float zoomScale = 1.25f;
                if (world.input.isActivated(InputActions.ZoomIn))
                {
                    this.size = new Vector2(this.size.X / zoomScale, this.size.Y / zoomScale);
                }

                if (world.input.isActivated(InputActions.ZoomOut))
                {
                    this.size = new Vector2(this.size.X * zoomScale, this.size.Y * zoomScale);
                }
            }


            // filter world objects to viewable items 
            visibleObjects = new List<intObject>();
            foreach (intObject o in this.world.worldObjects)
            {
                if (this.isInVisibleArea(o.location))
                {
                    visibleObjects.Add(o);
                }
            }


            // camera overlay
            text = new List<string>();
            for (int a=0; a<world.actors.Count; a++)
            {
                intActor driver = world.actors[a];

                clsDriverHuman driverHuman = driver as clsDriverHuman;   // Here is where I get typeof(IA)
                if (driverHuman != null)
                {
                    text.Add("--------------------");
                    text.Add("Name : " + driverHuman.car.name + " (" + a + ")");
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
                    text.Add("Name : " + driverAI.car.name + " (" + a + ")");
                    text.Add("Speed : " + Math.Floor(driverAI.car.mph) + " MPH");
                    text.Add("Obstruction : " + driverAI.route.distanceToNextObstruction);
                    text.Add("Collision : " + driverAI.route.distanceToNextCollision);
                    text.Add("Turn : " + driverAI.route.distanceToNextCollision);
                    text.Add("Yield : " + driverAI.yielding);
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
                int top = (int)Math.Floor((decimal)tmpVisibleArea.Top / (decimal)this.world.tileSize);
                int width = (int)Math.Ceiling(tmpVisibleArea.Width / (decimal)this.world.tileSize);
                int height = (int)Math.Ceiling(tmpVisibleArea.Height / (decimal)this.world.tileSize);
                return new Rectangle(left, top, width, height);
            }
        }

        public bool isInVisibleArea(Vector2 worldLocation)
        {
            Rectangle tmpVisibleArea = this.visibleArea; // cache for speed
            if ((worldLocation.X >= tmpVisibleArea.Left) && (worldLocation.Y >= tmpVisibleArea.Top) && (worldLocation.X <= tmpVisibleArea.Right) && (worldLocation.Y <= tmpVisibleArea.Bottom)) return true;
            else return false;
        }

        public bool isInVisibleTileArea(Vector2 tileLocation)
        {
            Rectangle tmpVisibleTileArea = this.visibleTileArea; // cache for speed
            if ((tileLocation.X >= tmpVisibleTileArea.Left) && (tileLocation.Y >= tmpVisibleTileArea.Top) && (tileLocation.X <= tmpVisibleTileArea.Right) && (tileLocation.Y <= tmpVisibleTileArea.Bottom)) return true;
            else return false;
        }

        public Vector2 getCameraCoordinate(Vector2 worldCoordinate)
        {
            return worldCoordinate - new Vector2(this.visibleArea.Location.X, this.visibleArea.Location.Y);
        }
    }
}
