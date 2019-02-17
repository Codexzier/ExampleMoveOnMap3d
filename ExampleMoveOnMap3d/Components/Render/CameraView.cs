using ExampleMoveOnMap3d.Components.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Windows.UI.ViewManagement;

namespace ExampleMoveOnMap3d.Components.Render
{
    public class CameraView
    {
        private readonly Game _game;
        private readonly ComponentMap _componentContent;

        public Vector3 Position { get; set; }
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        private bool _isRightView = false;
        private Viewport _viewPort;

        public CameraView(Game game, ComponentMap componentMap, bool isRightView)
        {
            this._game = game;
            this._componentContent = componentMap;
            this._isRightView = isRightView;
        }

        public void Initialize()
        {
            this._viewPort = this._game.GraphicsDevice.Viewport;
            this._viewPort.Width = this._viewPort.Width / 2;

            if (this._isRightView)
            {
                this._viewPort.X = this._viewPort.Width;
            }

            var aspectRatio = this.GetAspectRatio();
            var position = new Vector3(-1f, 5f, 5f);
            var target = new Vector3(0, 0, 0);
            var farPlaneDistance = 10000;

            this.View = Matrix.CreateLookAt(position, target, Vector3.Backward);
            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                        aspectRatio,
                                                                        1,
                                                                        farPlaneDistance);
        }

        public void Draw()
        {
            this._game.GraphicsDevice.Viewport = this._viewPort;
            this._componentContent.DrawContent(this.View, this.Projection);
        }

        private float GetAspectRatio()
        {
            var w = (float)ApplicationView.GetForCurrentView().VisibleBounds.Width / 2;
            var h = (float)ApplicationView.GetForCurrentView().VisibleBounds.Height;

            return w / h;
        }
    }
}
