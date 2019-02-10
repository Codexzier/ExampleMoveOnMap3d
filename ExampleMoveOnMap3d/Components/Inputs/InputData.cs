using Microsoft.Xna.Framework;

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
    }
}
