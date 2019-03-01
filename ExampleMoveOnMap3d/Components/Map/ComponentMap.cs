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
        private BottleModel _bottleModel;
        private Effect _effect;

        public ComponentMap(Game game, ComponentInputs componentInputs) : base(game)
        {
            this._componentInputs = componentInputs;
        }

        public override void Initialize()
        {
            var texture = this.Game.Content.Load<Texture2D>("rpgTile029");
            this._animatedWaterwaves = new AnimatedWaterwaves(texture);
            this._animatedWaterwaves.Initialize(this.Game.GraphicsDevice);

            var bottle = this.Game.Content.Load<Model>("bottle");
            this._bottleModel = new BottleModel(new Vector3(), new Vector3(MathHelper.ToRadians(90), MathHelper.ToRadians(90), 0), 1f, bottle);
            this._bottleModel.SetPosition(new Vector3(10,10,0));

            this._lastPositionZ = this._bottleModel.Position.Z;

            this._effect = this.Game.Content.Load<Effect>("SimpleShadow");
        }

        public override void Update(GameTime gameTime)
        {
            this._bottleModel.Update(gameTime);

            this._bottleModel.SetOffsetRotation(new Vector3(MathHelper.ToRadians(0), MathHelper.ToRadians(90), 0));


            this._bottleModel.AddPosition(new Vector3(this._componentInputs.Inputs.Move, 0), .3f);
            this._lastPositionZ = this._bottleModel.Position.Z;

            this._animatedWaterwaves.Update(this.Game.GraphicsDevice);
            var d = this._animatedWaterwaves.GetAngle(this._bottleModel.Position);

            var actualPosition = this.SetDelay(d.Position, this._bottleModel.Position);

            if(actualPosition.Z > d.Position.Z)
            {
                actualPosition += this._bottleModel.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                actualPosition -= this._bottleModel.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds / 1000;
            }
            

            this._bottleModel.SetPosition(actualPosition);

            this._bottleModel.SetRotation(d.Rotation);
        }

        private float _lastPositionZ = 0;

        private Vector3 SetDelay(Vector3 waterPosition, Vector3 objectPosition)
        {
            return new Vector3(objectPosition.X, objectPosition.Y, (waterPosition.Z + this._lastPositionZ) / 2);
        }

        public void DrawContent(Matrix view, Matrix projection)
        {
            this._animatedWaterwaves.Draw(this.Game.GraphicsDevice, view, projection);
            this._bottleModel.Draw(view, projection);


            
        }


    }
}
