using meshDatabase;
using meshDisplay.Interface;
using meshReader;
using meshReader.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TomShane.Neoforce.Controls;

namespace meshDisplay
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Application
    {
        public static readonly RasterizerState WireframeMode = new RasterizerState();

        public static GeneralDialog GeneralDialog { get; private set; }
        public static CameraManager Camera { get; private set; }

        public Game() : base("Default", false)
        {
            WireframeMode.CullMode = CullMode.None;
            WireframeMode.FillMode = FillMode.WireFrame;
            WireframeMode.MultiSampleAntiAlias = false;

            Graphics.PreferredBackBufferWidth = 1024;
            Graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
        }

        public void AddAdt(string path)
        {
            Components.Add(new AdtDrawer(this, path));
        }

        public void AddInstance(string path)
        {
            var wdt = new WDT(path);
            if (!wdt.IsValid || !wdt.IsGlobalModel)
                return;

            Components.Add(new WmoDrawer(this, wdt.ModelFile, wdt.ModelDefinition));
        }

        public void AddMesh(string continent, int x, int y)
        {
            Components.Add(new MeshDrawer(this, continent, x, y));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ClearBackground = true;
            BackgroundColor = Color.White;
            ExitConfirmation = false;

            Camera = new CameraManager(this);
            Components.Add(Camera);

            MpqManager.Initialize();
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_30_58.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_31_58.adt"));

            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_30_59.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_31_59.adt"));
            
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_30_49.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_31_49.adt"));

            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_32_54.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_32_55.adt"));*/

            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_34_40.adt"));
            
            //AddMesh("Azeroth", 30, 58);
            //AddMesh("Azeroth", 31, 58);
            
            //AddMesh("Azeroth", 30, 59);
            AddMesh("Azeroth", 31, 59);

            //AddMesh("Azeroth", 30, 49);
            //AddMesh("Azeroth", 31, 49);

            //AddMesh("Azeroth", 34, 40);

            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_36_49.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_29_49.adt")); Stormwind
            //Components.Add(new AdtDrawer(this, "World\\maps\\Kalimdor\\Kalimdor_32_30.adt"));

            IsMouseVisible = true;

            base.Initialize();

            GeneralDialog = new GeneralDialog(Manager);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1000.0f, 0);
            base.Update(gameTime);
        }
    }
}
