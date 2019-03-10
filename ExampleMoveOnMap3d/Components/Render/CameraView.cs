using System;
using ExampleMoveOnMap3d.Components.Map;
using Microsoft.Xna.Framework;
using Windows.UI.ViewManagement;

namespace ExampleMoveOnMap3d.Components.Render
{
    public class CameraView
    {
        private readonly Game _game;
        private readonly ComponentMap _componentContent;

        public Matrix View => Matrix.CreateLookAt(this._position, this._target, Vector3.Backward);
        public Matrix Projection { get; private set; }

        private float _aspectRatio;
        private Vector3 _position;
        private Vector3 _target;

        public CameraView(Game game, ComponentMap componentMap)
        {
            this._game = game;
            this._componentContent = componentMap;
        }

        public void Initialize()
        {
            this._aspectRatio = this.GetAspectRatio();
            //this._position = new Vector3(230f, -4f, 40f);
            this._position = new Vector3(20f, -4f, 20f);
            this._target = new Vector3(10, 10, 0);
            var farPlaneDistance = 10000;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                        this._aspectRatio,
                                                                        1,
                                                                        farPlaneDistance);
        }

        internal void AddMove(Vector3 move)
        {
            this._position += move;
            this._target += move;
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
