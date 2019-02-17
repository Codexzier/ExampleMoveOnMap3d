using ExampleMoveOnMap3d.Components.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExampleMoveOnMap3d.Components.DebugInfo
{
    public class ComponentDebug : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Texture2D _blank;

        private readonly ComponentRender _componentRender;

        private float _averageFps = 0f;
        private float _averageFps2 = 0f;
        private float _averageFps3 = 0f;

        public ComponentDebug(Game game, ComponentRender componentRender) : base(game)
        {
            this._componentRender = componentRender;
        }

        public override void Initialize()
        {
            this._spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this._spriteFont = this.Game.Content.Load<SpriteFont>("Debug");
            this._blank = this.Game.Content.Load<Texture2D>("Blank");
        }

        public override void Draw(GameTime gameTime)
        {
            this._spriteBatch.Begin();
            
            float framesRender = this._componentRender.FpsResult;


            switch (this._componentRender.ComponentContent.Mode)
            {
                case 0:
                    this._averageFps = (this._averageFps + framesRender) / 2;
                    break;
                case 1:
                    this._averageFps2 = (this._averageFps2 + framesRender) / 2;
                    break;
                case 2:
                    this._averageFps3 = (this._averageFps3 + framesRender) / 2;
                    break;
                default:
                    break;
            }
            
            int objects = this._componentRender.ComponentContent.ShowObjects;

            this._spriteBatch.Draw(this._blank,
                                new Vector2(95, 0),
                                null,
                                Color.White,
                                0,
                                new Vector2(0, 0),
                                new Vector2(34, 22),
                                SpriteEffects.None,
                                0f);

            this.DrawValue($"Objects {objects:N1}", 20);
            this.DrawValue($"FPS {framesRender:N1}", 50);
            this.DrawValue($"FPS {this._averageFps:N1}", 80);
            this.DrawValue($"FPS {this._averageFps2:N1}", 110);
            this.DrawValue($"FPS {this._averageFps3:N1}", 140);

            this._spriteBatch.End();
        }

        private void DrawValue(string strValue, float positionY)
        {
            this._spriteBatch.DrawString(this._spriteFont,
                                        strValue,
                                        new Vector2(100, positionY),
                                        Color.White,
                                        0f,
                                        new Vector2(),
                                        0.5f,
                                        SpriteEffects.None,
                                        1f);
        }
    }
}
