using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public class clsActor
    {
        public clsRoute route { get; set; }
        public clsObject worldObject { get; set; }
        protected clsWorld world { get; set; }
        public bool yield { get; set; }

        public float lastUpdated { get; set; }

        public clsActor(clsWorld world, clsObject worldObject)
        {
            this.world = world;
            this.worldObject = worldObject;
        }

        public void update()
        {
            if (route != null)
            {
                route.update(); // not all actors have planned routes

                // yeild to other AI based on conflict resolution
                if (this.yield)
                {
                    // yielding to another object on the right
                    if (this.route.distanceToNextCollision > 1) this.yield = false; // if collision has clear remove yield
                }
                else if (this.route.distanceToNextCollision <= 1)
                {
                    // determine if we should yield   
                    conflictResolution(this.route.nextCollision);
                }
            }

        }

        public void calculateShortestRoute(Vector2 fromLocation, Vector2 toLocation)
        {
            Vector2 carSquareCoordinate = world.worldLocationToTileCoordinate(fromLocation);
            Vector2 exitSquareCoordinate = world.worldLocationToTileCoordinate(toLocation);

            this.route = new clsRoute(world, carSquareCoordinate, exitSquareCoordinate);
        }

        public void conflictResolution(clsObject conflictingObject)
        {
            // yeild to cars on the right
            this.yield = false;
            int cardinalDistance = (this.worldObject.cardinalDirection - conflictingObject.cardinalDirection) + 4;
            if (cardinalDistance >= 2)
            {
                this.yield = true;
            }
        }
    }
}
