using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExampleMoveOnMap3d.Components.Map
{
    /// <summary>
    /// Contained the simpled of a plane with waves.
    /// </summary>
    public class BufferedWave
    {
        /// <summary>
        /// Step per iteration. It used to animate the wave on the x axis.
        /// </summary>
        private float _waveStepX = .05f;

        /// <summary>
        /// Step per iteration. It used to animate the wave on the y axis.
        /// </summary>
        private float _waveStepY = .2f;

        /// <summary>
        /// Count of squares on the x axis.
        /// </summary>
        private readonly int _tilesX = 20;
        /// <summary>
        /// Count of squares on the y axis.
        /// </summary>
        private readonly int _tilesY = 20;
        /// <summary>
        /// The side lengt of one square
        /// </summary>
        private readonly float _squareLength = 1f;
        /// <summary>
        /// The texturesize to render on the plane.
        /// </summary>
        private float _textureSize;

        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;
        public int VertexCount;
        private int _indexCount;

        public List<VertexPositionNormalTexture> VertexPositions;

        /// <summary>
        /// The height of a wave.
        /// </summary>
        public const float WaveHeight = 1.4f;
        /// <summary>
        /// The x axis lengt of a wave.
        /// </summary>
        public const int WaveLengthX = 10;
        /// <summary>
        /// The y axis lengt of a wave.
        /// </summary>
        public const int WaveLengthY = 10;

        public void Initialize(int step, GraphicsDevice graphicsDevice)
        {
            this._textureSize = 5f / this._tilesY;
            this.VertexPositions = this.GenerateVertexBuffer(step, graphicsDevice);
        }

        /// <summary>
        /// Main method to create the plane with waves.
        /// </summary>
        /// <param name="step">Set the step value to setup the part of a buffered animated waves.</param>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        private List<VertexPositionNormalTexture> GenerateVertexBuffer(int step, GraphicsDevice graphicsDevice)
        {
            this.ClearBuffers();

            var waveStartX = this._waveStepX * step;
            var waveStartY = this._waveStepY * step;

            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();
            List<int> index = new List<int>();

            for (int iY = 0; iY < this._tilesY; iY++)
            {
                for (int iX = 0; iX < this._tilesX; iX++)
                {
                    this.SetIndex(ref index, vertices.Count);

                    float waveX = iX + waveStartX;
                    float waveY = iY + waveStartY;

                    float height1 = this.GetHeight(waveX, waveY, 0, 1);
                    float height2 = this.GetHeight(waveX, waveY, 1, 1);
                    float height3 = this.GetHeight(waveX, waveY);
                    float height4 = this.GetHeight(waveX, waveY, 1, 0);

                    var verticePos1 = new Vector3((iX * this._squareLength), (iY * this._squareLength) + this._squareLength, height1);
                    var verticePos2 = new Vector3((iX * this._squareLength) + this._squareLength, (iY * this._squareLength) + this._squareLength, height2);
                    var verticePos3 = new Vector3((iX * this._squareLength), (iY * this._squareLength), height3);
                    var verticePos4 = new Vector3((iX * this._squareLength) + this._squareLength, (iY * this._squareLength), height4);

                    float aaX = this.GetTextureCoordinate(iX);
                    float aaY = this.GetTextureCoordinate(iY);

                    float aaX1 = this.GetTextureCoordinate(iX) + this._textureSize;
                    float aaY1 = this.GetTextureCoordinate(iY) + this._textureSize;

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

        /// <summary>
        /// Calculate the heigt by parameter.
        /// </summary>
        /// <param name="waveX">x position of the waves</param>
        /// <param name="waveY">y position of the waves</param>
        /// <param name="x">For the secound shift x position</param>
        /// <param name="y">For the secound shift y position</param>
        /// <returns></returns>
        private float GetHeight(double waveX, double waveY, int x = 0, int y = 0)
        {
            float heightY = (float)Math.Sin(((waveY + y) * Math.PI) / WaveLengthY);
            float heigthX1 = (float)Math.Sin((float)((waveX + x) * Math.PI) / WaveLengthX);
            return (heigthX1 + heightY) * WaveHeight;
        }

        /// <summary>
        /// calculate the texture coordinate on the plane
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private float GetTextureCoordinate(int index)
        {
            var textureCoordinate = this._textureSize * index;

            if (textureCoordinate >= 1f)
            {
                textureCoordinate %= 1f;
            }

            return textureCoordinate;
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
