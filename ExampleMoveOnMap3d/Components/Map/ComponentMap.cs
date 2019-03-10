using ExampleMoveOnMap3d.Components.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class ComponentMap : GameComponent
    {
        private const float _speed = 0.3f;
        private readonly ComponentInputs _componentInputs;
        //private AnimatedWaterwaves _animatedWaterwaves;
        public AnimatedWaterwavesBuffered AnimatedWaterwavesBuffered;

        public ComponentMap(Game game, ComponentInputs componentInputs) : base(game)
        {
            this._componentInputs = componentInputs;
        }

        public override void Initialize()
        {
            var texture = this.Game.Content.Load<Texture2D>("rpgTile029");

            // This is the unbuffered waves
            //this._animatedWaterwaves = new AnimatedWaterwaves(texture);
            //this._animatedWaterwaves.Initialize(this.Game.GraphicsDevice);

            // the buffered waves
            this.AnimatedWaterwavesBuffered = new AnimatedWaterwavesBuffered(texture);
            this.AnimatedWaterwavesBuffered.Initialize(this.Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            RasterizerState raster = new RasterizerState();
            raster.CullMode = CullMode.CullCounterClockwiseFace;
            raster.FillMode = FillMode.WireFrame;
            this.Game.GraphicsDevice.RasterizerState = raster;

            // This is the unbuffered waves
            //this._animatedWaterwaves.Update(this.Game.GraphicsDevice);

            // the buffered waves
            this.AnimatedWaterwavesBuffered.Update();
        }

        public void DrawContent(Matrix view, Matrix projection)
        {
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // This is the unbuffered waves
            //this._animatedWaterwaves.Draw(this.Game.GraphicsDevice, view, projection);

            // the buffered waves
            for (int iY = 0; iY < 10; iY++)
            {
                for (int iX = 0; iX < 10; iX++)
                {
                    this.AnimatedWaterwavesBuffered.Draw(this.Game.GraphicsDevice, view, projection, new Vector3(200 - (iX * 20), 200 - (iY * 20), 0), 400 * iY);
                }
            }
        }
    }
}
