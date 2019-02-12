using Microsoft.Xna.Framework;

namespace ExampleMoveOnMap3dCore.Components.Inputs
{
    /// <summary>
    ///     Contains the move values for x and y
    /// </summary>
    public class InputData
    {
        public Vector2 Move => new Vector2(this.MoveX, this.MoveY);
        public float MoveX { get; set; }
        public float MoveY { get; set; }
    }
}
