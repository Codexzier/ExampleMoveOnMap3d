using Microsoft.Xna.Framework;
using System;

namespace ExampleMoveOnMap3d.Components.Inputs
{
    /// <summary>
    ///     Contains the move values for x and y
    /// </summary>
    public class InputData
    {
        public Vector2 Move => new Vector2(this.MoveX, this.MoveY);
        public float MoveX { get; set; }
        public float MoveY { get; set; }

        public void Reset()
        {
            this.MoveX = 0;
            this.MoveY = 0;
        }

        public void Normalize()
        {
            this.MoveX = Math.Min(1, Math.Max(-1, this.MoveX));
            this.MoveY = Math.Min(1, Math.Max(-1, this.MoveY));
        }
    }
}
