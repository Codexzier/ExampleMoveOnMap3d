using ExampleMoveOnMap3d.Components.Inputs;
using ExampleMoveOnMap3d.Components.Map.Grounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class ComponentMap : GameComponent
    {
        private const float _speed = 0.3f;
        private readonly ComponentInputs _componentInputs;
        private AnimatedWaterwaves _animatedWaterwaves;
        public AnimatedWaterwavesBuffered AnimatedWaterwavesBuffered;
        private BottleModel _bottleModel;
        //private BottleModel _bottleModel2;

        // normal
        private PhysicHelper _helper1 = new PhysicHelper();
        // the bottle
        private PhysicHelper _helper2 = new PhysicHelper(72.75f, .7f, 5f);
        
        private float _reducedUp = 0;
        private Vector3 _lastRotation = new Vector3();

        public ComponentMap(Game game, ComponentInputs componentInputs) : base(game)
        {
            this._componentInputs = componentInputs;
        }

        public override void Initialize()
        {
            var texture = this.Game.Content.Load<Texture2D>("rpgTile029");

            // This is the unbuffered waves
            this._animatedWaterwaves = new AnimatedWaterwaves(texture);
            this._animatedWaterwaves.Initialize(this.Game.GraphicsDevice);

            var bottle = this.Game.Content.Load<Model>("bottle");
            this._bottleModel = new BottleModel(new Vector3(0,0,-2), new Vector3(MathHelper.ToRadians(0), MathHelper.ToRadians(90), 0), 1f, bottle);
            this._bottleModel.SetPosition(new Vector3(10,10,0));

            // the buffered waves
            //this.AnimatedWaterwavesBuffered = new AnimatedWaterwavesBuffered(texture);
            //this.AnimatedWaterwavesBuffered.Initialize(this.Game.GraphicsDevice);

            //this._bottleModel2 = new BottleModel(new Vector3(0, 0, -2), new Vector3(MathHelper.ToRadians(0), MathHelper.ToRadians(90), 0), 1f, bottle);
            //this._bottleModel2.SetPosition(new Vector3(5, 5, 0));

            var seaLevel = this._animatedWaterwaves.GetSeaLevelInformation(this._bottleModel.Position);
            this._bottleModel.SetRotation(seaLevel.Rotation);
            this._lastRotation = this._bottleModel.Rotation + new Vector3(0,0,1);
        }

        public override void Update(GameTime gameTime)
        {
            RasterizerState raster = new RasterizerState();
            raster.CullMode = CullMode.CullCounterClockwiseFace;
            raster.FillMode = FillMode.Solid;
            this.Game.GraphicsDevice.RasterizerState = raster;

            this._bottleModel.Update(gameTime);
            this._animatedWaterwaves.Update(this.Game.GraphicsDevice);

            // the buffered waves
            //this.AnimatedWaterwavesBuffered.Update();
        
            // TODO: must going to refactore this chaos -.-

            this._bottleModel.SetOffsetRotation(new Vector3(MathHelper.ToRadians(0), MathHelper.ToRadians(90), 0));
            this._bottleModel.AddPosition(new Vector3(this._componentInputs.Inputs.Move, 0), .3f);
            
            // This is the unbuffered waves
            this._animatedWaterwaves.Update(this.Game.GraphicsDevice);
            var seaLevel = this._animatedWaterwaves.GetSeaLevelInformation(this._bottleModel.Position);

            // direction move
            var resultBottle = this._helper2.CalculateUptrustValue(seaLevel.Position.Z, this._bottleModel.Position.Z);

            if (resultBottle > 0)
            {
                if (resultBottle > this._reducedUp)
                {
                    this._reducedUp = resultBottle;
                }

                resultBottle = 0;
            }

            Debug.WriteLine($"Reduced: {this._reducedUp}");
            var velocityChange = this._bottleModel.PhysicData.Velocity * (1f / this._reducedUp);
            this._bottleModel.AddPosition(velocityChange);

            // rotation
            // TODO: sometime it jump to quick rotation state
            var rotationChanged = (seaLevel.Rotation + this._lastRotation - seaLevel.Rotation) + this._bottleModel.PhysicData.AngularMementum * 10f;
            this._bottleModel.SetRotation(rotationChanged);
            this._lastRotation = seaLevel.Rotation;

            //var seaLevel2 = this._animatedWaterwaves.GetSeaLevelInformation(this._bottleModel2.Position);
            //this._bottleModel2.SetPosition(seaLevel2.Position);
            //this._bottleModel2.SetRotation(seaLevel2.Rotation);
        }

        public void DrawContent(Matrix view, Matrix projection)
        {
            this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            this._animatedWaterwaves.Draw(this.Game.GraphicsDevice, view, projection);

            // the buffered waves
            //for (int iY = 0; iY < 10; iY++)
            //{
            //    for (int iX = 0; iX < 10; iX++)
            //    {
            //        this.AnimatedWaterwavesBuffered.Draw(this.Game.GraphicsDevice, view, projection, new Vector3(200 - (iX * 20), 200 - (iY * 20), 0), 400 * iY);
            //    }
            //}

            this._bottleModel.Draw(view, projection);
            //this._bottleModel2.Draw(view, projection);
        }
    }
}
