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
            if (Constant.Division == 1)
                Components.Add(new MeshDrawer(this, continent, x, y));
            else
            {
                for (int i = 0; i < Constant.Division; i++)
                   for (int j = 0; j < Constant.Division; j++)
                       Components.Add(new MeshDrawer(this, continent, x, y, i, j));
            }
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

            //Components.Add(new AdtDrawer(this, "World\\maps\\EyeoftheStorm2.0\\EyeoftheStorm2.0_28_27.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\EyeoftheStorm2.0\\EyeoftheStorm2.0_28_28.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_35_20.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_36_20.adt"));

            /*Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_28_30.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_28_31.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_28_32.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_28_33.adt"));
            */
            Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_30_43.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_30_44.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_30_53.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Azeroth\\Azeroth_31_53.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Kalimdor\\Kalimdor_29_12.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Kalimdor\\Kalimdor_30_12.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Kalimdor\\Kalimdor_29_13.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Kalimdor\\Kalimdor_30_13.adt"));

            /*Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_29_30.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_29_31.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_29_32.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_29_33.adt"));

            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_30_30.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_30_31.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_30_32.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_30_33.adt"));

            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_31_30.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_31_31.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_31_32.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_31_33.adt"));

            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_32_30.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_32_31.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_32_32.adt"));
            Components.Add(new AdtDrawer(this, "World\\maps\\GoldRushBG\\GoldRushBG_32_33.adt"));
            */
            //Components.Add(new AdtDrawer(this, "World\\maps\\Firelands1\\Firelands1_32_54.adt"));
            //Components.Add(new AdtDrawer(this, "World\\maps\\Firelands1\\Firelands1_32_55.adt"));*/

            //Components.Add(new AdtDrawer(this, "World\\maps\\Firelands1\\Firelands1_34_40.adt"));

            AddMesh("Azeroth", 30, 43);
            AddMesh("Azeroth", 30, 44);
            //AddMesh("Azeroth", 30, 53);
            //AddMesh("Azeroth", 31, 53);

            //AddMesh("Kalimdor", 29, 12);
            //AddMesh("Kalimdor", 30, 12);
            //AddMesh("Kalimdor", 29, 13);
            //AddMesh("Kalimdor", 30, 13);

            //AddMesh("Azeroth", 35, 20);
            //AddMesh("Azeroth", 36, 20);
            /*
             AddMesh("GoldRushBG", 28, 30);
             AddMesh("GoldRushBG", 28, 31);
             AddMesh("GoldRushBG", 28, 32);

             AddMesh("GoldRushBG", 29, 30);
             AddMesh("GoldRushBG", 29, 31);
             AddMesh("GoldRushBG", 29, 32);

             AddMesh("GoldRushBG", 30, 30);
             AddMesh("GoldRushBG", 30, 31);
             AddMesh("GoldRushBG", 30, 32);

             AddMesh("GoldRushBG", 31, 30);
             AddMesh("GoldRushBG", 31, 31);
             AddMesh("GoldRushBG", 31, 32);

             AddMesh("GoldRushBG", 32, 30);
             AddMesh("GoldRushBG", 32, 31);
             AddMesh("GoldRushBG", 32, 32);*/
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
