using ExampleMoveOnMap3d.Components.Map;
using Microsoft.Xna.Framework;
using Windows.UI.ViewManagement;

namespace ExampleMoveOnMap3d.Components.Render
{
    public class CameraView
    {
        private readonly Game _game;
        private readonly ComponentMap _componentContent;

        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        public CameraView(Game game, ComponentMap componentMap)
        {
            this._game = game;
            this._componentContent = componentMap;
        }

        public void Initialize()
        {
            var aspectRatio = this.GetAspectRatio();
            var position = new Vector3(-1f, -20f, 20f);
            var target = new Vector3(10, 10, 0);
            var farPlaneDistance = 10000;

            this.View = Matrix.CreateLookAt(position, target, Vector3.Backward);
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                        aspectRatio,
                                                                        1,
                                                                        farPlaneDistance);
        }

        public void Draw()
        {
            this._componentContent.DrawContent(this.View, this.Projection);
        }

        private float GetAspectRatio()
        {
            var w = (float)ApplicationView.GetForCurrentView().VisibleBounds.Width;
            var h = (float)ApplicationView.GetForCurrentView().VisibleBounds.Height;

            return w / h;
        }
    }
}
