using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BarbieBear
{
    public static class Input
    {
        private static MouseState mouseState = Mouse.GetState();
        private static MouseState mouseStatePrev = Mouse.GetState();
        private static KeyboardState keyboardState = Keyboard.GetState();
        private static KeyboardState keyboardStatePrev = Keyboard.GetState();

        public static void Update()
        {
            mouseStatePrev = mouseState;
            mouseState = Mouse.GetState();

            keyboardStatePrev = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public static bool ScreenHeld
        {
            get { return mouseState.LeftButton == ButtonState.Pressed; }
        }

        public static bool ScreenTapped
        {
            get { return mouseState.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Released; }
        }

        public static Vector2 TapPosition
        {
            get { return new Vector2(mouseState.X, mouseState.Y); }
        }

        public static bool KeyboardTapped(Keys key)
        {
            return keyboardState.IsKeyDown(key) && keyboardStatePrev.IsKeyUp(key);
        }
    }
}
