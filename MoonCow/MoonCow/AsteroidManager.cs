using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AsteroidManager:GameComponent
    {
        Game1 game;
        public List<Asteroid> asteroids;
        List<Asteroid> toDelete;
        List<Asteroid> toAdd;
        List<JunkShip> junkShips;
        List<JunkShip> jToDelete;
        List<MapNode> astNodes;
        AsteroidGenerator astGen;
        public AsteroidManager(Game1 game):base(game)
        {
            this.game = game;
            asteroids = new List<Asteroid>();
            toDelete = new List<Asteroid>();
            toAdd = new List<Asteroid>();
            junkShips = new List<JunkShip>();
            jToDelete = new List<JunkShip>();
            astNodes = new List<MapNode>();
            astGen = new AsteroidGenerator(this, game);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                foreach (Asteroid a in asteroids)
                    a.Update();
                foreach (Asteroid a in toDelete)
                    asteroids.Remove(a);
                foreach (Asteroid a in toAdd)
                    asteroids.Add(a);

                foreach (JunkShip j in junkShips)
                    j.Update();

                toDelete.Clear();
                toAdd.Clear();
            }
        }

        public void addToDelete(Asteroid a)
        {
            toDelete.Add(a);
        }

        public void generateField()
        {
            astGen.run(astNodes);
        }

        public void addSmall(Vector3 pos)
        {
            asteroids.Add(new AstSmall(pos, game));
        }
        public void addMid(Vector3 pos)
        {
            asteroids.Add(new AstMid(pos, game));
        }

        public void addBig(Vector3 pos)
        {
            asteroids.Add(new AstBig(pos, game));
        }

        public void addPos(MapNode pos)
        {
            astNodes.Add(pos);
        }

        public void addAsteroid(Vector3 pos)
        {
            //will do a thing to randomly pick an asteroid size
            asteroids.Add(new AstBig(pos, game));
        }

        public void addAsteroid(Asteroid a)
        {
            toAdd.Add(a);
        }

        public void addShip(Vector3 pos)
        {
            junkShips.Add(new JunkShip(game, pos));
        }
    }
}
