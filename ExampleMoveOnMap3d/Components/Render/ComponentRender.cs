using ExampleMoveOnMap3d.Components.Map;
using Microsoft.Xna.Framework;

namespace ExampleMoveOnMap3d.Components.Render
{
    public class ComponentRender : DrawableGameComponent
    {
        private readonly CameraView _cameraView;

        public ComponentRender(Game game, ComponentMap componentContent) : base(game)
        {
            this._cameraView = new CameraView(game, componentContent);
            this._cameraView.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this._cameraView.Draw();
        }
    }
}
