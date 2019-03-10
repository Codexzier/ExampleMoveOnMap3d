using ExampleMoveOnMap3d.Components.Inputs;
using ExampleMoveOnMap3d.Components.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Windows.System;

namespace ExampleMoveOnMap3d.Components.Render
{
    public class ComponentRender : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;


        private readonly CameraView _cameraView;
        private readonly ComponentInputs _componentInputs;
        private readonly ComponentMap _componentContent;

        public ComponentRender(Game game, ComponentInputs componentInputs, ComponentMap componentContent) : base(game)
        {
            this._cameraView = new CameraView(game, componentContent);
            this._cameraView.Initialize();
            this._componentInputs = componentInputs;
            this._componentContent = componentContent;
        }

        public override void Initialize()
        {
            this._spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this._spriteFont = this.Game.Content.Load<SpriteFont>("Debug");
        }

        public override void Update(GameTime gameTime)
        {
            this._cameraView.AddMove(new Vector3(this._componentInputs.Inputs.MoveX, this._componentInputs.Inputs.MoveY, 0));
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this._cameraView.Draw();

            this._spriteBatch.Begin();


            var memoryUsed = GC.GetTotalMemory(false) / 1024f / 1024f;

            var memoryUsed2 = (float)MemoryManager.AppMemoryUsage / 1024f / 1024f;

            this._spriteBatch.DrawString(this._spriteFont,
                $"{memoryUsed:N2}MB | {memoryUsed2:N2}MB",
                new Vector2(50, 50), Color.Yellow);

            this._spriteBatch.End();
        }
    }
}
