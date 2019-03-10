using ExampleMoveOnMap3d.Components.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class ComponentMap : GameComponent
    {
        private const float _speed = 0.3f;
        private readonly ComponentInputs _componentInputs;
        private AnimatedWaterwaves _animatedWaterwaves;
        public AnimatedWaterwavesBuffered AnimatedWaterwavesBuffered;

        public ComponentMap(Game game, ComponentInputs componentInputs) : base(game)
        {
            this._componentInputs = componentInputs;
        }

        public override void Initialize()
        {
            var texture = this.Game.Content.Load<Texture2D>("rpgTile029");
            this._animatedWaterwaves = new AnimatedWaterwaves(texture);
            this._animatedWaterwaves.Initialize(this.Game.GraphicsDevice);

            this.AnimatedWaterwavesBuffered = new AnimatedWaterwavesBuffered(texture);
            this.AnimatedWaterwavesBuffered.Initialize(this.Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            this._animatedWaterwaves.Update(this.Game.GraphicsDevice);
            this.AnimatedWaterwavesBuffered.Update();
        }
        
        public void DrawContent(Matrix view, Matrix projection)
        {
          // this._animatedWaterwaves.Draw(this.Game.GraphicsDevice, view, projection);
            this.AnimatedWaterwavesBuffered.Draw(this.Game.GraphicsDevice, view, projection);
        }
    }
}
