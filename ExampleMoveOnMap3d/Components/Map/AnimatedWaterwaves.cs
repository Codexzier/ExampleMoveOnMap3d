using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class AnimatedWaterwaves
    {
        private BasicEffect _effect;

        private int _tilesX = 20;
        private int _tilesY = 20;

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private int _vertexCount;
        private int _indexCount;

        private float _waveXStart = 0f;
        private float _waveYStart = 0f;

        private Texture2D _texture;
        private Vector3 _position = new Vector3(0, 0, 0);

        private List<VertexPositionNormalTexture> _vertexPositions;

        public AnimatedWaterwaves(Texture2D texture)
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

            this._vertexPositions = this.RegenerateVertexBuffer(graphicsDevice);
        }

        private List<VertexPositionNormalTexture> RegenerateVertexBuffer(GraphicsDevice graphicsDevice)
        {
            this.ClearBuffers();

            this._waveXStart += .05f;
            this._waveYStart += .2f;

            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
            List<int> index = new List<int>();

            float setHeight = 1.4f;

            float waveLengthX = 10f;
            float aa = 5f / this._tilesY;
            
            for (int iY = 0; iY < this._tilesY; iY++)
            {
                for (int iX = 0; iX < this._tilesX; iX++)
                {
                    this.SetIndex(ref index, vertices.Count);

                    float waveLengthY = 16f;

                    float waveX = iX + this._waveXStart;
                    float waveY = iY + this._waveYStart;

                    float heightY = (float)Math.Sin(((waveY + 1) * Math.PI) / waveLengthY);
                    float heigthX1 = (float)Math.Sin((float)(waveX * Math.PI) / waveLengthX);
                    float height1 = (heigthX1 + heightY) * setHeight;

                    float heightY2 = (float)Math.Sin(((waveY + 1) * Math.PI) / waveLengthY);
                    float heigthX2 = (float)Math.Sin((float)((waveX + 1) * Math.PI) / waveLengthX);
                    float height2 = (heigthX2 + heightY2) * setHeight;

                    float heightY3 = (float)Math.Sin((waveY * Math.PI) / waveLengthY);
                    float heigthX3 = (float)Math.Sin((float)(waveX * Math.PI) / waveLengthX);
                    float height3 = (heigthX3 + heightY3) * setHeight;

                    float heightY4 = (float)Math.Sin((waveY * Math.PI) / waveLengthY);
                    float heigthX4 = (float)Math.Sin((float)((waveX + 1) * Math.PI) / waveLengthX);
                    float height4 = (heigthX4 + heightY4) * setHeight;

                    float aaX = aa * iX;
                    float aaY = aa * iY;

                    float aaX1 = (aa * iX) + aa;
                    float aaY1 = (aa * iY) + aa;

                    if (aaX > 1f)
                    {
                        aaX = aaX % 1f;
                        aaX1 = aaX1 % 1f;
                    }

                    if (aaY > 1f)
                    {
                        aaY = aaY % 1f;
                        aaY1 = aaY1 % 1f;
                    }

                    float squareLength = 1f;

                    vertices.Add(new VertexPositionNormalTexture(new Vector3((iX * squareLength) + 0, (iY * squareLength) + squareLength, height1), Vector3.Up, new Vector2(aaX, aaY1)));
                    vertices.Add(new VertexPositionNormalTexture(new Vector3((iX * squareLength) + squareLength, (iY * squareLength) + squareLength, height2), Vector3.Up, new Vector2(aaX1, aaY1)));
                    vertices.Add(new VertexPositionNormalTexture(new Vector3((iX * squareLength) + 0, (iY * squareLength) + 0, height3), Vector3.Up, new Vector2(aaX, aaY)));
                    vertices.Add(new VertexPositionNormalTexture(new Vector3((iX * squareLength) + squareLength, (iY * squareLength) + 0, height4), Vector3.Up, new Vector2(aaX1, aaY)));
                }
            }

            this._vertexCount = vertices.Count;
            this._indexCount = index.Count;

            this.InitVertexBuffer(graphicsDevice, vertices);
            this._indexBuffer.SetData<int>(index.ToArray());

            return vertices;
        }

        public void Update(GraphicsDevice graphicsDevice)
        {
            this._vertexPositions = this.RegenerateVertexBuffer(graphicsDevice);
        }

        internal void Draw(GraphicsDevice graphicsDevice, Matrix view, Matrix projection)
        {
            this._effect.View = view;
            this._effect.Projection = projection;

            graphicsDevice.SetVertexBuffer(this._vertexBuffer);
            graphicsDevice.Indices = this._indexBuffer;

            foreach (EffectPass pass in this._effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this._vertexCount);
            }
        }

        private void InitVertexBuffer(GraphicsDevice graphicsDevice, List<VertexPositionNormalTexture> vertices)
        {
            this._vertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, this._vertexCount, BufferUsage.WriteOnly);
            this._vertexBuffer.SetData(vertices.ToArray());

            this._indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, this._indexCount, BufferUsage.WriteOnly);
        }

        private void ClearBuffers()
        {
            if (this._vertexBuffer != null)
            {
                this._vertexBuffer.Dispose();
                this._vertexBuffer = null;
            }

            if (this._indexBuffer != null)
            {
                this._indexBuffer.Dispose();
                this._indexBuffer = null;
            }
        }

        private void SetIndex(ref List<int> index, int localOffset)
        {
            index.Add(localOffset + 0);
            index.Add(localOffset + 1);
            index.Add(localOffset + 3);
            index.Add(localOffset + 0);
            index.Add(localOffset + 3);
            index.Add(localOffset + 2);
        }
    }
}
