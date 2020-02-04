using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using tileWorld;
using physicalWorld;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public class clsExit : clsWorldObject, intWorldObject
    {
        public Vector2 direction { get; set; }
        
        public clsExit(Vector2 location, Vector2 direction) : base(location, direction, new Vector2(0, 0))
        {
            this.worldObjectType = WorldObjectType.exit;
            base.collisionType = CollisionType.None;
            this.direction = direction;
        }


        new public void update(float currentTime)
        {
            float deltaTime = currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                this.lastUpdated = currentTime; // reset last updated

                // maybe remove cars
                // maybe calculate score

                base.addForce(new Vector2(0,0));
            }

            // always apply phypics
            base.update(currentTime);
        }

        public void removeGameObject(clsWorld gameWorld, clsWorldObject gameObject)
        {
            gameWorld.remove((intWorldObject)gameObject);
        }
    }
}
