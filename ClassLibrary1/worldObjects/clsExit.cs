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
        
        public clsExit(string textureName, Vector2 location, Vector2 direction) : base(textureName, location, direction, new Vector2(0, 0))
        {
            base.typeName = "exit";
            base.collisionType = CollisionType.None;
        }


        new public void update(float currentTime)
        {
            float deltaTime = currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                this.lastUpdated = currentTime; // reset last updated

                // maybe remove cars. Cars know their exit so maybe not
                // maybe calculate score

                base.addForce(new Vector2(0,0)); // not sure why we add no force
            }

            // always apply physics
            base.update(currentTime);
        }

        public void removeGameObject(clsWorld world, clsWorldObject worldObject)
        {
            world.remove((intWorldObject)worldObject);
        }
    }
}
