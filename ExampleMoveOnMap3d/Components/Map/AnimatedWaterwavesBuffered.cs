using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class AnimatedWaterwavesBuffered
    {
        private BasicEffect _effect;
        private readonly Texture2D _texture;

        Dictionary<int, BufferedWave> _dictionaryVertexPositions = new Dictionary<int, BufferedWave>();
        public int Index { get; private set; } = 0;

        public AnimatedWaterwavesBuffered(Texture2D texture)
        {
            this._texture = texture;
        }
        
        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this._effect = new BasicEffect(graphicsDevice);
            this._effect.World = Matrix.Identity;

            this._effect.TextureEnabled = true;
            this._effect.Texture = this._texture;

            this._effect.EnableDefaultLighting();

            // is estimated
            int bufferIndexMay = 1600;

            for (int i = 0; i < bufferIndexMay; i++)
            {
                BufferedWave bufferedWave = new BufferedWave();
                bufferedWave.Initialize(i, graphicsDevice);

                this._dictionaryVertexPositions.Add(i, bufferedWave);
            }
        }

        public void Update()
        {
            if(this.Index >= this._dictionaryVertexPositions.Count - 1)
            {
                this.Index = 0;
            }
            else
            {
                this.Index++;
            }
        }

        internal void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            this._effect.View = view;
            this._effect.Projection = projection;

            var item = this._dictionaryVertexPositions[this.Index];

            graphicsDevice.SetVertexBuffer(item.VertexBuffer);
            graphicsDevice.Indices = item.IndexBuffer;

            foreach (EffectPass pass in this._effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, item.VertexCount);
            }
        }
    }
}
