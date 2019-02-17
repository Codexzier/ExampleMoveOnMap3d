using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExampleMoveOnMap3d.Components.Inputs
{
    public class ComponentInputs : GameComponent
    {
        /// <summary>
        ///     Contains all input values.
        /// </summary>
        public InputData Inputs1 { get; private set; } = new InputData();
        public InputData Inputs2 { get; private set; } = new InputData();

        public ComponentInputs(Game game) : base(game)
        {
        }

        /// <summary>
        ///     Update all inputs from xbox controllern and keybord.
        /// </summary>
        /// <param name="gameTime">Not used</param>
        public override void Update(GameTime gameTime)
        {
            this.Inputs1.Reset();
            this.Inputs2.Reset();

            KeyboardState stateKeyboard = Keyboard.GetState();

            this.Inputs1.MoveX += stateKeyboard.IsKeyDown(Keys.A) ? 1 : 0;
            this.Inputs1.MoveX -= stateKeyboard.IsKeyDown(Keys.D) ? 1 : 0;
            this.Inputs1.MoveY += stateKeyboard.IsKeyDown(Keys.W) ? 1 : 0;
            this.Inputs1.MoveY -= stateKeyboard.IsKeyDown(Keys.S) ? 1 : 0;

            GamePadState stateGamePad = GamePad.GetState(PlayerIndex.One);

            this.Inputs2.MoveX += stateGamePad.ThumbSticks.Left.X;
            this.Inputs2.MoveY += stateGamePad.ThumbSticks.Left.Y;

            // invert 
            this.Inputs1.MoveX *= -1;

            // Normalize
            this.Inputs1.Normalize();
            this.Inputs2.Normalize();
        }
    }
}
