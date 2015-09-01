﻿using System;
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
    public class MapNode
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

        public List<OOBB> collisionBoxes = new List<OOBB>();
        public CircleCollider coreCollider;
        public int type;
        private TileModel model;
        

        /// <summary>
        /// Constructor for MapNode. Sets a lot of data based on the type of node it is.
        /// </summary>
        public MapNode(Game1 game, int type, Vector2 pos)
        {
            switch(type)
            {
                case 1:
                    // Is this node included in AI pathfinding?
                    traversable = true;
                    // Set the model for this node
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0, 1.0f);
                    // Using the constructor of the OOBB that just takes four corners create the bounding boxes for this node and store their positional data
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 2:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    break;
                case 3:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 4:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 5:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 6:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 7:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 8:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 9:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 10:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/dend"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs more colliders for the two diagonals
                    break;
                case 11:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/tInt4"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 100);
                    // Need at least 12 colliders for the rounded corner edges
                    break;
                case 12:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/tInt3"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs another 6 colliders for the rounded corner bits
                    break;
                case 13:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/tInt3"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs another 6 colliders for the rounded corner bits
                    break;
                case 14:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/tInt3"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs another 6 colliders for the rounded corner bits
                    break;
                case 15:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/tInt3"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    // Needs another 6 colliders for the rounded corner bits
                    break;
                case 16:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    // Needs one more collider for diagonal on corner
                    // Needs another three for the little rounded inner corner bit
                    break;
                case 17:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs one more collider for diagonal on corner
                    // Needs another three for the little rounded inner corner bit
                    break;
                case 18:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs one more collider for diagonal on corner
                    // Needs another three for the little rounded inner corner bit
                    break;
                case 19:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs one more collider for diagonal on corner
                    // Needs another three for the little rounded inner corner bit
                    break;
                case 20:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corst"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs 3 more colliders for the rounded corner bit
                    break;
                case 21:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    break;
                case 22:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corstFlip"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs three more for little rounded corner bit
                    break;
                case 23:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 24:
                    traversable = true;
                    game.modelManager.addAdditive(new CoreSphereModel(game.Content.Load<Model>(@"Models/Base/coreSphere"), new Vector3(pos.X * 30, 0, pos.Y * 30), game));
                    coreCollider = new CircleCollider(pos, 7.38f);
                    break;
                case 25:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    break;
                case 26:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs one more for diagonal corner bit
                    break;
                case 27:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 28:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs 1 more for diagonal corner bit
                    break;
                case 29:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    // Needs 1 more for diagonal corner bit
                    break;
                case 30:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs one more for diagonal part
                    break;
                case 31:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 32:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 33:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 34:
                    traversable = true;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 35:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    // Needs one more for diagonal corner bit
                    break;
                case 36:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs one more for diagonal bit
                    break;
                case 37:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs one more for diagonal bit
                    break;
                case 38:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1big"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs one more for diagonal corner bit
                    break;
                case 39:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 40:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    break;
                case 41:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    break;
                case 42:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/straight1"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    break;
                case 43:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 44:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 45:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 46:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner2small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    // Needs three more for little rounded corner bits
                    // Needs three more for little rounded corner bits
                    break;
                case 47:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1small"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    // Needs three more for little rounded corner bits
                    break;
                case 48:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1.0f);
                    // Needs three more for little rounded corner bits
                    break;
                case 49:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1.0f);
                    // Needs three more for little rounded corner bits
                    break;
                case 50:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corner1small"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1.0f);
                    // Needs three more for little rounded corner bits
                    break;
                case 51:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corst"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs 3 more for little rounded corner bits
                    break;
                case 52:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corst"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs 3 more for little rounded corner bits
                    break;
                case 53:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corst"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 10), new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 10)));
                    // Needs 3 more for little rounded corner bit
                    break;
                case 54:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corst"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs three more for little rounded corner bit
                    break;
                case 55:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corstFlip"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) + 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) + 10, (pos.Y * 30) + 15)));
                    // Needs three more for little rounded corner bit
                    break;
                case 56:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corstFlip"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2, 1);
                    break;
                case 57:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corstFlip"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 2, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) - 15), new Vector2((pos.X * 30) - 10, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs three more for the little rounded corner bit
                    break;
                case 58:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/Rails/corstFlip"), new Vector3(pos.X * 30, 0, pos.Y * 30), MathHelper.PiOver2 * 3, 1);
                    collisionBoxes.Add(new OOBB(new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 10), new Vector2((pos.X * 30) + 15, (pos.Y * 30) + 15), new Vector2((pos.X * 30) - 15, (pos.Y * 30) + 15)));
                    // Needs three more for little rounded corner bits
                    break;
                case 59:
                    traversable = false;
                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
                case 60:
                    traversable = false;
                    model = new TileModel(game.Content.Load<Model>(@"Models/BgTiles/node60"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0, 100);
                    break;
                default:
                    traversable = false;
                    //model = new TileModel(game.Content.Load<Model>(@"Models/TempRails/corstProto"), new Vector3(pos.X * 30, 0, pos.Y * 30), 0.0f, 1.0f);
                    break;
            }

            position = pos;
            this.type = type;
            if (model != null)
            {
                game.modelManager.add(model);
            }
        }
    }
}
  