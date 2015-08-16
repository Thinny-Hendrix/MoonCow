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
    /// </summary>
    /// 

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int[,] layout;
        Map map;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            layout = new int[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, }, 
                { 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, },
                { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, }, 
                { 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, }, 
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, },
                { 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, }, 
                { 1, 0, 1, 0, 1, 1, 0, 1, 0, 0, }, 
                { 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, }, 
                { 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, }, 
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, }, 
            };

            map = new Map(layout);

            Pathfinder pathfinder = new Pathfinder(map);
            
            List<Vector2> path = pathfinder.findPath(new Point(0, 0), new Point(9, 9));

            foreach (Vector2 point in path)
            {
                System.Diagnostics.Debug.WriteLine(point);
            }

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
