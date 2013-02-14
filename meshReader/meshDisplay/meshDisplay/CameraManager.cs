using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace meshDisplay
{

    public class CameraManager : GameComponent
    {
        protected Vector2 OffMousePos;
        protected Vector2 PreMousePos;

        public CameraManager(Game game)
            : base(game)
        {
            Camera = new Camera();
            {
                Camera.Rotation = new Quaternion(0, 0, 0, 0);
                Camera.Position = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        public Camera Camera { get; private set; }

        protected void HandleKeyboard()
        {
            // Variable that controls the speed
            float speed = 5f;

            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.LeftShift))
                speed = 0.5f;

            // Check for specified key presses
            if (state.IsKeyDown(Keys.W))
                Camera.Translate(new Vector3(0, 0, -1) * speed);

            if (state.IsKeyDown(Keys.S))
                Camera.Translate(new Vector3(0, 0, 1) * speed);

            if (state.IsKeyDown(Keys.A))
                Camera.Translate(new Vector3(-1, 0, 0) * speed);

            if (state.IsKeyDown(Keys.D))
                Camera.Translate(new Vector3(1, 0, 0) * speed);
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboard();

            // Retrieve the mousestate
            MouseState ms = Mouse.GetState();

            // Check if pressed the left mouse button
            if (ms.LeftButton == ButtonState.Pressed)
            {
                // Rotate camera
                Camera.Rotate(OffMousePos.X * 0.005f, OffMousePos.Y * 0.005f, 0);
            }
            // Save the offset between mousecoordinates, and the current mouse pos
            OffMousePos = PreMousePos - new Vector2(ms.X, ms.Y);
            PreMousePos = new Vector2(ms.X, ms.Y);

            base.Update(gameTime);
        }
    }

}