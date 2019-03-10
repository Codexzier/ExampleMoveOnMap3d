using ExampleMoveOnMap3d.Components.Inputs;
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
        private BottleModel _bottleModel2;

        // normal
        private PhysicHelper _helper1 = new PhysicHelper();
        // the bottle
        private PhysicHelper _helper2 = new PhysicHelper(72.75f, .7f, 5f);

        private PidController _pidController = new PidController(2, .5, .05, 200, -200);

        private PidController _pidControllerX = new PidController(2, .5, .05, 200, -200);
        private PidController _pidControllerY = new PidController(2, .5, .05, 200, -200);
        private PidController _pidControllerZ = new PidController(2, .5, .05, 200, -200);

        private Vector3 _lastPosition = new Vector3();
        private Vector3 _lastRotation = new Vector3();
        private float _reducedUp = 0;
        private float _lastPositionZ = 0;

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

            this._bottleModel2 = new BottleModel(new Vector3(0, 0, -2), new Vector3(MathHelper.ToRadians(0), MathHelper.ToRadians(90), 0), 1f, bottle);
            this._bottleModel2.SetPosition(new Vector3(5, 5, 0));

            this._lastPositionZ = this._bottleModel.Position.Z;
            this._lastRotation = this._bottleModel.Rotation;
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

            this._lastPositionZ = this._bottleModel.Position.Z;

            // This is the unbuffered waves
            this._animatedWaterwaves.Update(this.Game.GraphicsDevice);
            var seaLevel = this._animatedWaterwaves.GetSeaLevelInformation(this._bottleModel.Position);

            var resultBottle4 = this._helper2.CalculateUptrustValue(seaLevel.Position.Z, this._bottleModel.Position.Z);

            if (resultBottle4 > 0)
            {
                if (resultBottle4 > this._reducedUp)
                {
                    this._reducedUp = resultBottle4;
                }

                resultBottle4 = 0;
            }


            //var velocityChange = new Vector3(this._bottleModel.Position.X, 
            //                                 this._bottleModel.Position.Y, 
            //                                 this._lastPositionZ + (resultBottle4 / 100f));

         

            this._pidController.Proportional = .02;
            this._pidController.Integral = .05;
            this._pidController.Derivative = .005;

            this._pidController.SetPoint = resultBottle4;
            var ch = (float)this._pidController.ControlVariable(gameTime.ElapsedGameTime);
            this._pidController.ProcessVariable = ch;

            Debug.WriteLine($"{ch}");

            var velocityChange = new Vector3(this._bottleModel.Position.X,
                                        this._bottleModel.Position.Y,
                                        ch);

            //Debug.WriteLine($"{resultBottle4:N1}, position: {velocityChange.Z:N1}");
            this._lastPositionZ = velocityChange.Z;
            
            this._bottleModel.SetPosition(velocityChange);

            var rotationChange = seaLevel.Rotation - this._lastRotation;
            //Debug.Write($"Bottle: {this._bottleModel.Rotation:N1}, lastRotate: {this._lastRotation.Z:N1}, rotation change: {rotationChange}, ");
            //seaLevel.Rotation =  this._bottleModel.Rotation + (rotationChange /70);
            //Debug.WriteLine($"resultRotation: {seaLevel.Rotation:N1}");

            this._bottleModel.SetRotation(seaLevel.Rotation);

            this._lastRotation = this._bottleModel.Rotation;


            var seaLevel2 = this._animatedWaterwaves.GetSeaLevelInformation(this._bottleModel2.Position);
            this._bottleModel2.SetPosition(seaLevel2.Position);
            this._bottleModel2.SetRotation(seaLevel2.Rotation);

            //this._pidControllerX.Proportional = .3;
            //this._pidControllerX.Integral = .0001;
            //this._pidControllerX.Derivative = .00005;

            //this._pidControllerY.Proportional = this._pidControllerX.Proportional;
            //this._pidControllerZ.Proportional = this._pidControllerX.Proportional;

            //this._pidControllerY.Integral = this._pidControllerX.Integral;
            //this._pidControllerZ.Integral = this._pidControllerX.Integral;

            //this._pidControllerY.Derivative = this._pidControllerX.Derivative;
            //this._pidControllerZ.Derivative = this._pidControllerX.Derivative;

            //this._pidControllerX.SetPoint = seaLevel.Rotation.X;
            //this._pidControllerY.SetPoint = seaLevel.Rotation.Y;
            //this._pidControllerZ.SetPoint = seaLevel.Rotation.Z;

            //var x = (float)this._pidControllerX.ControlVariable(gameTime.ElapsedGameTime);
            //var y = (float)this._pidControllerY.ControlVariable(gameTime.ElapsedGameTime);
            //var z = (float)this._pidControllerZ.ControlVariable(gameTime.ElapsedGameTime);

            //seaLevel.Rotation = new Vector3(x, y, z); 

           
            //this._bottleModel.SetRotation(seaLevel.Rotation);
            //this._pidControllerX.ProcessVariable = this._bottleModel.Rotation.X;
            //this._pidControllerY.ProcessVariable = this._bottleModel.Rotation.Y;
            //this._pidControllerZ.ProcessVariable = this._bottleModel.Rotation.Z;
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
            this._bottleModel2.Draw(view, projection);
        }
    }
}
