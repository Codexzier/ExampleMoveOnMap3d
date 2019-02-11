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
            this.ComponentContent = componentContent;
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this._cameraView.Draw();
            this.FpsResult = this.GetFramesPerSecound(gameTime);
        }

        #region Debug information

        private int _framecount;
        private float _secounds;
        private float _lastFps = 0;

        public float FpsResult { get; private set; }
        public ComponentMap ComponentContent { get; }

        private float GetFramesPerSecound(GameTime gameTime)
        {
            this._framecount++;
            this._secounds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this._framecount >= 10)
            {
                this._lastFps = this._secounds / this._framecount;
                this._framecount = 0;
                this._secounds = 0;
            }

            float frames = 1 / this._lastFps;

            if (float.IsInfinity(frames))
            {
                frames = 1;
            }

            return frames;
        }

        #endregion
    }
}
