using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    /// <summary>
    /// Holy shit this is gunna be hard.
    /// It'll be an adventure! A space adventure!
    /// </summary>
    /// 

    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        Texture2D gameDraw;

        public Map map;
        public Camera camera;
        public ModelManager modelManager;
	    public EnemyManager enemyManager;
        public AudioManager audioManager;
        public Ship ship;
        private MapData layout;
        public Hud hud;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphicsDevice = new GraphicsDevice();
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera(this, new Vector3(40, 150, 10), Vector3.Zero, Vector3.Up);
            modelManager = new ModelManager(this);
	        enemyManager = new EnemyManager(this);
            audioManager = new AudioManager(this);
            ship = new Ship(this);
            hud = new Hud(this, Content.Load<SpriteFont>(@"Hud/Venera900"), spriteBatch, GraphicsDevice);


            Components.Add(camera);
            Components.Add(modelManager);
            Components.Add(audioManager);
            Components.Add(enemyManager);
            Components.Add(ship);
            Components.Add(hud);

            modelManager.makeStarField();


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            layout = new MapData(@"Content/MapXml/map1.xml");

            map = new Map(this, layout.getNodes());

            Pathfinder pathfinder = new Pathfinder(map);
            List<Vector2> path = pathfinder.findPath(new Point(2, 0), new Point(5, 9));
		
	        enemyManager.addEnemy(new Enemy(this));

            foreach (Vector2 point in path)
            {
                System.Diagnostics.Debug.WriteLine(point);
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Utilities.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
            //hud.update(gameTime, spriteBatch);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0.2f,0.2f,0.2f));

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            //hud.Draw(gameTime);
        }

        
    }
}
