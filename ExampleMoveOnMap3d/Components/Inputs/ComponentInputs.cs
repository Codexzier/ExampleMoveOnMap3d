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
        public InputData Inputs { get; private set; } = new InputData();

        public ComponentInputs(Game game) : base(game)
        {
        }

        /// <summary>
        ///     Update all inputs from xbox controllern and keybord.
        /// </summary>
        /// <param name="gameTime">Not used</param>
        public override void Update(GameTime gameTime)
        {
            this.Inputs.MoveX = 0;
            this.Inputs.MoveY = 0;

            KeyboardState stateKeyboard = Keyboard.GetState();

            this.Inputs.MoveX += stateKeyboard.IsKeyDown(Keys.A) ? 1 : 0;
            this.Inputs.MoveX -= stateKeyboard.IsKeyDown(Keys.D) ? 1 : 0;
            this.Inputs.MoveY += stateKeyboard.IsKeyDown(Keys.W) ? 1 : 0;
            this.Inputs.MoveY -= stateKeyboard.IsKeyDown(Keys.S) ? 1 : 0;

            GamePadState stateGamePad = GamePad.GetState(PlayerIndex.One);

            this.Inputs.MoveX += stateGamePad.ThumbSticks.Left.X * -1;
            this.Inputs.MoveY += stateGamePad.ThumbSticks.Left.Y;

            // invert 
            this.Inputs.MoveX *= -1;

            // Normalize
            this.Inputs.MoveX = Math.Min(1, Math.Max(-1, this.Inputs.MoveX));
            this.Inputs.MoveY = Math.Min(1, Math.Max(-1, this.Inputs.MoveY));
        }
    }
}
