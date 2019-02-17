using ExampleMoveOnMap3d.Components.Map;
using Microsoft.Xna.Framework;

namespace ExampleMoveOnMap3d.Components.Render
{
    public class ComponentRender : DrawableGameComponent
    {
        private readonly CameraView _cameraViewLeft;
        private readonly CameraView _cameraViewRight;

        public ComponentRender(Game game, ComponentMap componentContent, ComponentMap componentContent2) : base(game)
        {
            this._cameraViewLeft = new CameraView(game, componentContent, false); 
            this._cameraViewRight = new CameraView(game, componentContent2, true);
        }

        public override void Initialize()
        {
            this._cameraViewLeft.Initialize();
            this._cameraViewRight.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this._cameraViewLeft.Draw();
            this._cameraViewRight.Draw();
        }
    }
}
