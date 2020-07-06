using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public enum PathNodeStatus { unknown, discovered, procesed }

    public class clsPathFinding
    {
        List<clsPathNode> pathNodes = new List<clsPathNode>();

        clsPathFinding(clsWorld world, Vector2 startSquareLocation, Vector2 endSquareLocation)
        {
            // get start then connections and process like video
            clsPathNode startNode = new clsPathNode(startSquareLocation, world, startSquareLocation, endSquareLocation);
            startNode.status = PathNodeStatus.procesed;

            // get connected nodes
            List<clsPathNode> connectedNodes = startNode.getPossiblePathNodes();
            foreach (clsPathNode pathNode in connectedNodes)
            {
                pathNode.status = PathNodeStatus.procesed;
            }
            
        }


    }

    public class clsPathNode
    {
        public clsWorld world;
        public Vector2 squareLocation;
        public Vector2 endSquareLocation;
        public Vector2 startSquareLocation;
        public clsPathNode parent;
        public PathNodeStatus status = PathNodeStatus.unknown;

        public clsPathNode(Vector2 squareLocation, clsWorld world, Vector2 startSquareLocation, Vector2 endSquareLocation)
        {
            this.world = world;
            this.squareLocation = squareLocation;
            this.startSquareLocation = startSquareLocation;
            this.endSquareLocation = endSquareLocation;
        }

        // this is the black number
        public float hCost
        {
            get
            {
                return Vector2.Distance(squareLocation, endSquareLocation);
            }
        }

        public float gCost
        {
            get
            {
                return Vector2.Distance(squareLocation, startSquareLocation);
            }
        }

        public float fCode
        {
            get
            {
                return gCost + hCost;
            }
        }

        public List<clsPathNode> getPossiblePathNodes()
        {
            clsSquare square = world.squares[(int)squareLocation.X, (int)squareLocation.Y];

            List<clsPathNode> pathNodes = new List<clsPathNode>();
            foreach (Vector2 direction in square.directions)
            {
                Vector2 newSquareLocation = this.squareLocation + (direction * 64);
                float weight = Vector2.Distance(squareLocation, newSquareLocation);
                clsPathNode pathNode = new clsPathNode(newSquareLocation, world, startSquareLocation, endSquareLocation);
                pathNodes.Add(pathNode);
            }
            return pathNodes;
        }
    }
}
