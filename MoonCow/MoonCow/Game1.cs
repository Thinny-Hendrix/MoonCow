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
        public RenderTarget2D worldRender;
        DepthStencilState depthState;

        public Map map;
        public Camera camera;
        public ModelManager modelManager;
	    public EnemyManager enemyManager;
        public AudioManager audioManager;
        public TurretManager turretManager;
        public AsteroidManager asteroidManager;
        public Ship ship;
        private MapData layout;
        public Hud hud;

        BloomComponent bloom;
        int bloomSettingsIndex = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };
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
            //worldRender = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //depthState = new DepthStencilState();
            //depthState.DepthBufferEnable = true;
            //depthState.DepthBufferWriteEnable = true;
            //worldRender.GraphicsDevice.DepthStencilState = depthState;

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            worldRender = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            TextureManager.initialize(this);
            ModelLibrary.initialize(this);

            modelManager = new ModelManager(this);
            ship = new Ship(this);
            hud = new Hud(this, Content.Load<SpriteFont>(@"Hud/Venera40"), spriteBatch, GraphicsDevice);

            camera = new Camera(this, new Vector3(40, 150, 10), Vector3.Zero, Vector3.Up);
            turretManager = new TurretManager(this);
            asteroidManager = new AsteroidManager(this);


            layout = new MapData(@"Content/MapXml/map1-revis.xml");
            map = new Map(this, layout.getNodes());

	        enemyManager = new EnemyManager(this);
            audioManager = new AudioManager(this);
            //bloom = new BloomComponent(this);

            Components.Add(ship);
            Components.Add(camera);
            Components.Add(modelManager);
            Components.Add(audioManager);
            Components.Add(enemyManager);
            Components.Add(turretManager);
            Components.Add(asteroidManager);

            //make sure the post process effects go second last, and the hud is absolute last
            //Components.Add(bloom);
            Components.Add(hud);

            modelManager.makeStarField();
            turretManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //When our main menu is in place it will be loaded here. All actual game assets will be loaded in functions called in the menu (pick level etc etc)
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
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

            TextureManager.Update(this);

            //hud.update(gameTime, spriteBatch);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Utilities.frameCount++;
            GraphicsDevice.Clear(new Color(0.2f,0.2f,0.2f));

            worldRender.GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.SetRenderTarget(worldRender);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            //hud.Draw(gameTime);
        }

        
    }
}
