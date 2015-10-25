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
        bool justLoadedContent;
        bool loadedGameContent;

        public Map map;
        public Camera camera;
        public ModelManager modelManager;
	    public EnemyManager enemyManager;
        public WaveManager waveManager;
        public AudioManager audioManager;
        public TurretManager turretManager;
        public AsteroidManager asteroidManager;
        public Minigame minigame;
        public Ship ship;
        private MapData layout;
        public Hud hud;
        public BaseCore core;
        public StatTracker levelStats;

        //menu
        public MainMenu mainMenu;

        BloomComponent bloom;
        int bloomSettingsIndex = 0;

        public enum RunState { MainMenu, LevelSelect, Options, MainGame, StatScreen, LevelCreator }
        public RunState runState;

        MapData testMap;
        LevelCreator levelCreator;
        public string levelFileName;
        public float loadPercentage;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };
            //graphicsDevice = new GraphicsDevice();
            Content.RootDirectory = "Content";

            runState = RunState.MainMenu;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            loadPercentage = 0;

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

            loadSettings();

            LcAssets.initialize(this);

            initializeMenu();
            //initializeGame();
            //initializeLevelCreator();
            runState = RunState.MainMenu;
            //runState = RunState.LevelCreator;

            base.Initialize();
            Utilities.setScale(this);

        }

        void loadSettings()
        {
            // Read in the XML settings file and set values in static class, hardcoded for now
            Settings.initialize();
            EnemyBehaviour.load();
        }

        public void initializeLevelCreator()
        {
            levelCreator = new LevelCreator(this);
            Components.Add(levelCreator);
        }

        public void initializeMenu()
        {
            MenuAssets.Initialize(this);
            mainMenu = new MainMenu(this);

            Components.Add(mainMenu);
        }

        public void initializeGame(string level)
        {
            loadPercentage = 0;
            if (!loadedGameContent)
            {
                TextureManager.initialize(this);
                loadPercentage = 0.1f;
                ModelLibrary.initialize(this);
                loadPercentage = 0.2f;
                AudioLibrary.initialize(this);
                loadPercentage = 0.25f;
                loadedGameContent = true;
            }

            
            modelManager = new ModelManager(this);
            ship = new Ship(this);
            core = new BaseCore(this);
            camera = new Camera(this, new Vector3(40, 150, 10), Vector3.Zero, Vector3.Up);
            turretManager = new TurretManager(this);
            asteroidManager = new AsteroidManager(this);
            enemyManager = new EnemyManager(this);
            waveManager = new WaveManager(this);
            hud = new Hud(this, Content.Load<SpriteFont>(@"Hud/Venera40"), spriteBatch, GraphicsDevice);
            audioManager = new AudioManager(this);
            minigame = new Minigame(this);
            levelStats = new StatTracker();
            loadPercentage = 0.65f;

            levelFileName = "dends";
            //levelFileName = "map1-revis";
            //levelFileName = "pac-man";
            layout = new MapData(level);
            //layout = new MapData(@"Content/MapXml/Level2.xml");
            //layout = new MapData(@"Content/MapXml/pac-man.xml");
            //layout = new MapData(@"Content/MapXml/broktes.xml");

            map = new Map(this, layout.getNodes());

            bloom = new BloomComponent(this);

            Components.Add(ship);
            Components.Add(camera);
            Components.Add(modelManager);
            Components.Add(audioManager);
            Components.Add(enemyManager);
            Components.Add(waveManager);
            Components.Add(core);
            Components.Add(turretManager);
            Components.Add(asteroidManager);
            Components.Add(minigame);

            //make sure the post process effects go second last, and the hud is absolute last
            //Components.Add(bloom);
            Components.Add(hud);

            loadPercentage = 1f;

            modelManager.makeStarField();
            turretManager.Initialize();

            justLoadedContent = true;
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
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) && runState != RunState.MainMenu)
            {
                if(runState == RunState.LevelCreator)
                {
                    Components.Remove(levelCreator);
                    IsMouseVisible = false;
                }
                if(runState == RunState.MainGame)
                {
                    audioManager.shutup();
                    Components.Remove(ship);
                    Components.Remove(camera);
                    Components.Remove(modelManager);
                    Components.Remove(audioManager);
                    Components.Remove(enemyManager);
                    Components.Remove(waveManager);
                    Components.Remove(core);
                    Components.Remove(turretManager);
                    Components.Remove(asteroidManager);
                    Components.Remove(minigame);
                    Components.Remove(hud);
                }
                if (runState != RunState.LevelSelect)
                {
                    initializeMenu();
                    runState = RunState.MainMenu;
                }
            }

            Utilities.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);

            if(runState == RunState.MainGame)
            {
                TextureManager.Update(this);
                if(!Utilities.paused && !Utilities.softPaused)
                {
                    levelStats.timeInLevel += Utilities.deltaTime;
                }
            }

            //hud.update(gameTime, spriteBatch);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (justLoadedContent)
            {
                Update(gameTime);
                justLoadedContent = false;
            }

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
