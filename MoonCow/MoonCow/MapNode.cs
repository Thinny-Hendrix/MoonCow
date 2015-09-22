using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    /// <summary>
    /// A node in the map. This will be represented by a tile model in the 3D world.
    /// This class holds the model and collision data for each node
    /// This class is also used as the search node during pathfinding
    /// </summary>
    public class MapNode
    {
        public bool traversable;       //Can this node be walked through?
        public Vector2 position;       //X and Y co-ordinates in map
        public MapNode[] neighbors;    //Array of linked nodes
        public MapNode parent;         //Pointer to node in path before this one
        public Vector3 pos;            //world coordinate of node center

        public bool inOpenList;        //Currently in OpenList?
        public bool inClosedList;      //Currently in ClosedList?

        public float distanceToGoal;   //How far to goal
        public float distanceSoFar;    //How far along in the path this node is

        public float damage;           //How much damage this square can take from turrets
        public float playerDamage;     //How much damage the player can currently do to this node

        public List<OOBB> collisionBoxes = new List<OOBB>();    //A list of all the collision boxes in this node
        public CircleCollider coreCollider;                     //The collision for the core
        public int type;               //What node type this node is
        private BasicModel model;       //The model for this node
        private BasicModel stationModel;
        bool asteroidNode = false;
        

        /// <summary>
        /// Constructor for MapNode. Sets a lot of data based on the type of node it is.
        /// </summary>
        public MapNode(Game1 game, int type, Vector2 pos)
        {
            this.pos = new Vector3(pos.X * 30, 4.5f, pos.Y * 30);
            switch(type)
            {
                case 1:
                    // Is this node included in AI pathfinding?
                    traversable = true;
                    // Set the model for this node
                    model = new TileModel(ModelLibrary.railStraight, new Vector3(pos.X * 30, 0, pos.Y * 30), 0, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationStraight, new Vector3(pos.X * 30, -3f, pos.Y * 30), MathHelper.PiOver2, 1.0f);

                    //add turret bases
                    game.turretManager.addTurret(new Vector3(pos.X * 30, 4, pos.Y * 30 - 11), Vector3.Forward);
                    game.turretManager.addTurret(new Vector3(pos.X * 30, 4, pos.Y * 30 + 11), Vector3.Backward);

                    // Using the constructor of the OOBB that just takes four corners create the bounding boxes for this node and store their positional data
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 2:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railStraight, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationStraight, new Vector3(pos.X * 30, -3f, pos.Y * 30), 0, 1.0f);

                    //add turret bases
                    game.turretManager.addTurret(new Vector3(pos.X * 30-11, 4, pos.Y * 30), Vector3.Left);
                    game.turretManager.addTurret(new Vector3(pos.X * 30+11, 4, pos.Y * 30), Vector3.Right);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    break;
                case 3:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), 0.0f, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 2), new Vector2((pos.X * 30) - 3, (pos.Y * 30) - 12), new Vector2((pos.X * 30), (pos.Y * 30) - 9), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 1)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 3, (pos.Y * 30) - 12), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 2), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 1), new Vector2((pos.X * 30), (pos.Y * 30) - 9)));
                    break;
                case 4:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 12, (pos.Y * 30) - 3), new Vector2((pos.X * 30) - 2, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 9, (pos.Y * 30))));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 9, (pos.Y * 30)), new Vector2((pos.X * 30) + 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 2, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 12, (pos.Y * 30) - 3)));
                    break;
                case 5:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.Pi, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30), (pos.Y * 30) + 9), new Vector2((pos.X * 30) - 3, (pos.Y * 30) + 12), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 2)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30), (pos.Y * 30) + 9), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 2), new Vector2((pos.X * 30) + 3, (pos.Y * 30) + 12)));
                    break;
                case 6:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 2, (pos.Y * 30) -13), new Vector2((pos.X * 30) + 12, (pos.Y * 30) - 3), new Vector2((pos.X * 30) + 9, (pos.Y * 30)), new Vector2((pos.X * 30) - 1, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 9, (pos.Y * 30)), new Vector2((pos.X * 30) + 12, (pos.Y * 30) + 3), new Vector2((pos.X * 30) + 2, (pos.Y * 30) + 13)));
                    break;
                case 7:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), 0.0f, 1.0f);
                    game.modelManager.addAdditive(new SpawnVortex(new Vector3(pos.X * 30, 4.5f, pos.Y * 30+10), new Vector3(0,MathHelper.Pi,0),game));

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 2), new Vector2((pos.X * 30) - 3, (pos.Y * 30) - 12), new Vector2((pos.X * 30), (pos.Y * 30) - 9), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 1)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 3, (pos.Y * 30) - 12), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 2), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 1), new Vector2((pos.X * 30), (pos.Y * 30) - 9)));
                    break;
                case 8:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    game.modelManager.addAdditive(new SpawnVortex(new Vector3(pos.X * 30+10, 4.5f, pos.Y * 30), new Vector3(0, MathHelper.Pi*1.5f, 0), game));


                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 12, (pos.Y * 30) - 3), new Vector2((pos.X * 30) - 2, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 9, (pos.Y * 30))));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 9, (pos.Y * 30)), new Vector2((pos.X * 30) + 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 2, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 12, (pos.Y * 30) - 3)));
                    break;
                case 9:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.Pi, 1.0f);
                    game.modelManager.addAdditive(new SpawnVortex(new Vector3(pos.X * 30, 4.5f, pos.Y * 30-10), new Vector3(0, 0, 0), game));


                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30), (pos.Y * 30) + 9), new Vector2((pos.X * 30) - 3, (pos.Y * 30) + 12), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 2)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30), (pos.Y * 30) + 9), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 2), new Vector2((pos.X * 30) + 3, (pos.Y * 30) + 12)));
                    break;
                case 10:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railDend, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationDend, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    game.modelManager.addAdditive(new SpawnVortex(new Vector3(pos.X * 30-10, 4.5f, pos.Y * 30), new Vector3(0, 0, 0), game));


                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //Diagonals
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 2, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 12, (pos.Y * 30) - 3), new Vector2((pos.X * 30) + 9, (pos.Y * 30)), new Vector2((pos.X * 30) - 1, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 9, (pos.Y * 30)), new Vector2((pos.X * 30) + 12, (pos.Y * 30) + 3), new Vector2((pos.X * 30) + 2, (pos.Y * 30) + 13)));
                    break;
                case 11:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railTInt4, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 100);
                    stationModel = new StationModel(ModelLibrary.stationTInt4, new Vector3(pos.X * 30, -3, pos.Y * 30), 0.0f, 1.0f);

                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 12:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railTInt3, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    stationModel = new StationModel(ModelLibrary.stationTInt3, new Vector3(pos.X * 30, -3, pos.Y * 30), 0.0f, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 13:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railTInt3, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1);
                    stationModel = new StationModel(ModelLibrary.stationTInt3, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs another 6 colliders for the rounded corner bits
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 14:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railTInt3, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1);
                    stationModel = new StationModel(ModelLibrary.stationTInt3, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.Pi, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 15:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railTInt3, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1);
                    stationModel = new StationModel(ModelLibrary.stationTInt3, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 16:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationCorner, new Vector3(pos.X * 30, -3, pos.Y * 30), 0.0f, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    //top left big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 4), new Vector2((pos.X * 30) - 4, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 1)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 17:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationCorner, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //bottom left big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 4), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 4, (pos.Y * 30) + 15)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 18:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationCorner, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.Pi, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //bottom right big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 4), new Vector2((pos.X * 30) + 4, (pos.Y * 30)+ 15)));
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    break;
                case 19:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    stationModel = new StationModel(ModelLibrary.stationCorner, new Vector3(pos.X * 30, -3, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);

                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //top right big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 4, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 4), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 1)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 20:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorst, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 21:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    break;
                case 22:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorstFlip, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    break;
                case 23:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 24:
                    traversable = true;
                    game.modelManager.addAdditive(new CoreSphereModel(ModelLibrary.core, new Vector3(pos.X * 30, 0, pos.Y * 30), game));
                    coreCollider = new CircleCollider(pos, 7.38f);
                    break;
                case 25:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    break;
                case 26:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //bottom left big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 4), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 4, (pos.Y * 30) + 15)));
                    break;
                case 27:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    game.ship.setRespawn(new Vector3(pos.X * 30, 4.5f, pos.Y * 30));
                    break;
                case 28:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //bottom right big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 4), new Vector2((pos.X * 30) + 4, (pos.Y * 30) + 15)));
                    break;
                case 29:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    //top left big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 4), new Vector2((pos.X * 30) - 4, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 1)));
                    break;
                case 30:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //top right big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 4, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 4), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 1)));
                    break;
                case 31:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 32:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 33:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 34:
                    traversable = true;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 35:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    //top left big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 4), new Vector2((pos.X * 30) - 4, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 1)));
                    break;
                case 36:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //bottom left big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 4), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 4, (pos.Y * 30) + 15)));
                    break;
                case 37:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //bottom right big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 1), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 4), new Vector2((pos.X * 30) + 4, (pos.Y * 30) + 15)));
                    break;
                case 38:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1big, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //top right big corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 1, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 4, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 4), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 1)));
                    break;
                case 39:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 40:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 41:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    break;
                case 42:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railStraight1, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    break;
                case 43:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 44:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 45:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 46:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner2small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 47:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1small, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    break;
                case 48:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 49:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 50:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorner1small, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 51:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorst, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 52:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorst, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    break;
                case 53:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorst, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 54:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorst, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 55:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorstFlip, new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    //top left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 12)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) - 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 13), new Vector2((pos.X * 30) - 13, (pos.Y * 30) - 10)));
                    break;
                case 56:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorstFlip, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    //bottom left little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) - 11, (pos.Y * 30) + 14), new Vector2((pos.X * 30) - 14, (pos.Y * 30) + 11)));
                    break;
                case 57:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorstFlip, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //bottom right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 13), new Vector2((pos.X * 30) + 13, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 14, (pos.Y * 30) + 11), new Vector2((pos.X * 30) + 11, (pos.Y * 30) + 14)));
                    break;
                case 58:
                    traversable = false;
                    model = new TileModel(ModelLibrary.railCorstFlip, new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    //top right little corner
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 13), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 11, (pos.Y * 30) - 14), new Vector2((pos.X * 30) + 14, (pos.Y * 30) - 11), new Vector2((pos.X * 30) + 13, (pos.Y * 30) - 10), new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 13)));
                    break;
                case 59:
                    traversable = false;

                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 60:
                    traversable = false;
                    break;
                case 61:
                    traversable = false;
                    model = new TileModel(ModelLibrary.node60, new Vector3(pos.X * 30, 0, pos.Y * 30), 0, 1);
                    break;
                default:
                    traversable = false;
                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
            }

            if(type > 34 && type < 60)//if the type is in the range of asteroid tiles
                game.asteroidManager.addPos(this);

            position = pos;
            this.type = type;
            neighbors = new MapNode[4];
            if (model != null)
            {
                game.modelManager.add(model);
            }

            if (stationModel != null)
            {
                game.modelManager.add(stationModel);
            }
        }
    }
}
  