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
    /// This node is used for many things, primarily pathfinding.
    /// </summary>
    class MapNode
    {
        public bool traversable;       //Can this node be walked through?
        public Vector2 position;       //X and Y co-ordinates in map
        public MapNode[] neighbors;    //Array of linked nodes
        public MapNode parent;         //Pointer to node in path before this one

        public bool inOpenList;        //Currently in OpenList?
        public bool inClosedList;      //Currently in ClosedList?

        public float distanceToGoal;   //How far to goal
        public float distanceSoFar;    //How far along in the path this node is

        public float damage;           //How much damage this square can take from turrets
        public float playerDamage;     //How much damage the player can currently do to this node

        private TileModel model;
        

        /// <summary>
        /// Constructor for MapNode. Sets a lot of data based on the type of node it is.
        /// </summary>
        public MapNode(Game1 game, int type, Vector2 pos)
        {
            switch(type)
            {
                case 1:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/raildaetest"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 2:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/raildaetest"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 3:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 4:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 5:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 6:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 7:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 8:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 9:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 10:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/dendProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 11:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/tInt4Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 100);
                    break;
                case 12:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/tInt3Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 100);
                    break;
                case 13:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/tInt3Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 100);
                    break;
                case 14:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/tInt3Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 100);
                    break;
                case 15:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/tInt3Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 100);
                    break;
                case 16:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/cornerProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 17:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/cornerProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 18:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/cornerProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 19:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/cornerProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 20:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 100);
                    break;
                case 21:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 22:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 100);
                    break;
                case 23:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 24:
                    traversable = true;
                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f); // Core!!
                    break;
                case 25:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 26:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 27:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 28:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 29:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 30:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 31:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 32:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 33:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 34:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 35:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 36:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 37:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 38:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1bigProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 39:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 40:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 41:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 42:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/straight1Proto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 43:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 44:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 45:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 46:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner2smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 47:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 48:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    break;
                case 49:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    break;
                case 50:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corner1smallProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    break;
                case 51:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 100);
                    break;
                case 52:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 100);
                    break;
                case 53:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 100);
                    break;
                case 54:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 100);
                    break;
                case 55:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 100);
                    break;
                case 56:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 100);
                    break;
                case 57:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 100);
                    break;
                case 58:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstFlipProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 100);
                    break;
                case 59:
                    traversable = false;
                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 60:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Misc/octahedron"), new Vector3(pos.X * 30, 4.5f, pos.Y * 30), MathHelper.PiOver4, 300);
                    break;
                default:
                    traversable = false;
                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
            }

            position = pos;
            if (model != null)
            {
                game.modelManager.add(model);
            }
        }
    }
}
  