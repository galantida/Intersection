using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using gameLogic;

namespace Game1
{
    class clsKeyboard
    {
        public void process(clsInputs inputs)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) input.forward = true;
            else input.forward = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Down)) input.backward = true;
            else input.backward = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Left)) input.left = true;
            else input.left = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Right)) input.right = true;
            else input.right = false;

            if (Keyboard.GetState().IsKeyDown(Keys.N)) input.n = true;
            else input.n = false;

            return input;
        }
    }
}
