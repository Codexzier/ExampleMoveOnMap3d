using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class BufferedWave
    {
        private float _waveStepX = .05f;
        private float _waveStepY = .2f;

        private readonly int _tilesX = 20;
        private readonly int _tilesY = 20;
        private readonly float _squareLength = 1f;

        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;
        public int VertexCount;
        private int _indexCount;

        public List<VertexPositionNormalTexture> VertexPositions;
        public string Key;

        public const float SetHeight = 1.4f;
        public const int WaveLengthX = 10;
        public const int WaveLengthY = 16;

        public void Initialize(int step, GraphicsDevice graphicsDevice)
        {
            this.VertexPositions = this.GenerateVertexBuffer(step,  graphicsDevice);
        }

        private float GetHeight(double waveX, double waveY, int x = 0, int y = 0)
        {
            float heightY = (float)Math.Sin(((waveY + y) * Math.PI) / WaveLengthY);
            float heigthX1 = (float)Math.Sin((float)((waveX + x) * Math.PI) / WaveLengthX);
            return (heigthX1 + heightY) * SetHeight;
        }

        private List<VertexPositionNormalTexture> GenerateVertexBuffer(int step, GraphicsDevice graphicsDevice)
        {
            this.ClearBuffers();

            var waveStartX = this._waveStepX * step;
            var waveStartY = this._waveStepY * step;

            this.Key = $"{waveStartX}:{waveStartY}";

            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
            List<int> index = new List<int>();

            
            float aa = 5f / this._tilesY;

            for (int iY = 0; iY < this._tilesY; iY++)
            {
                for (int iX = 0; iX < this._tilesX; iX++)
                {
                    this.SetIndex(ref index, vertices.Count);
                    
                    float waveX = iX + waveStartX;
                    float waveY = iY + waveStartY;

                    //float heightY = (float)Math.Sin(((waveY + 1) * Math.PI) / WaveLengthY);
                    //float heigthX1 = (float)Math.Sin((float)(waveX * Math.PI) / WaveLengthX);
                    float height1 = this.GetHeight(waveX, waveY, 0, 1);// (heigthX1 + heightY) * SetHeight;

                    //float heightY2 = (float)Math.Sin(((waveY + 1) * Math.PI) / WaveLengthY);
                    //float heigthX2 = (float)Math.Sin((float)((waveX + 1) * Math.PI) / WaveLengthX);
                    float height2 = this.GetHeight(waveX, waveY, 1, 1); //(heigthX2 + heightY2) * SetHeight;

                    //float heightY3 = (float)Math.Sin((waveY * Math.PI) / WaveLengthY);
                    //float heigthX3 = (float)Math.Sin((float)(waveX * Math.PI) / WaveLengthX);
                    float height3 = this.GetHeight(waveX, waveY); // (heigthX3 + heightY3) * SetHeight;

                    //float heightY4 = (float)Math.Sin((waveY * Math.PI) / WaveLengthY);
                    //float heigthX4 = (float)Math.Sin((float)((waveX + 1) * Math.PI) / WaveLengthX);
                    float height4 = this.GetHeight(waveX, waveY, 1, 0); //(heigthX4 + heightY4) * SetHeight;

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

                    var verticePos1 = new Vector3((iX * this._squareLength) + 0, (iY * this._squareLength) + this._squareLength, height1);
                    var verticePos2 = new Vector3((iX * this._squareLength) + this._squareLength, (iY * this._squareLength) + this._squareLength, height2);
                    var verticePos3 = new Vector3((iX * this._squareLength) + 0, (iY * this._squareLength) + 0, height3);
                    var verticePos4 = new Vector3((iX * this._squareLength) + this._squareLength, (iY * this._squareLength) + 0, height4);

                    vertices.Add(new VertexPositionNormalTexture(verticePos1, Vector3.Up, new Vector2(aaX, aaY1)));
                    vertices.Add(new VertexPositionNormalTexture(verticePos2, Vector3.Up, new Vector2(aaX1, aaY1)));
                    vertices.Add(new VertexPositionNormalTexture(verticePos3, Vector3.Up, new Vector2(aaX, aaY)));
                    vertices.Add(new VertexPositionNormalTexture(verticePos4, Vector3.Up, new Vector2(aaX1, aaY)));
                }
            }

            this.VertexCount = vertices.Count;
            this._indexCount = index.Count;

            this.InitVertexBuffer(graphicsDevice, vertices);
            this.IndexBuffer.SetData<int>(index.ToArray());

            return vertices;
        }



        private void InitVertexBuffer(GraphicsDevice graphicsDevice, List<VertexPositionNormalTexture> vertices)
        {
            this.VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, this.VertexCount, BufferUsage.WriteOnly);
            this.VertexBuffer.SetData(vertices.ToArray());

            this.IndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits, this._indexCount, BufferUsage.WriteOnly);
        }

        private void ClearBuffers()
        {
            if (this.VertexBuffer != null)
            {
                this.VertexBuffer.Dispose();
                this.VertexBuffer = null;
            }

            if (this.IndexBuffer != null)
            {
                this.IndexBuffer.Dispose();
                this.IndexBuffer = null;
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
