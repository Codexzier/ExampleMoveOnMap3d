using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

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

            //this._effect.EnableDefaultLighting();

            //this._effect.LightingEnabled = true;
            //this._effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1);
            //this._effect.DirectionalLight0.Direction = new Vector3(0f, 0f, 1f);
            //this._effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 10);
            //this._effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
            //this._effect.EmissiveColor = new Vector3(1, 0, 0);

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
            this._effect.LightingEnabled = true;
            this._effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1);
            this._effect.DirectionalLight0.Direction = new Vector3(0f, 0f, 1f);
            this._effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 10);
            this._effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
            this._effect.EmissiveColor = new Vector3(1f, 1f, 1f);


            this._vertexPositions = this.RegenerateVertexBuffer(graphicsDevice);

           // this._effect.World = Matrix.CreateTranslation(this._position);
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


        public CoordinateDetails GetAngle(Vector3 position)
        {
            // get the near vertex
            DistanceAndPosition[] results = new DistanceAndPosition[9];

            foreach (VertexPositionNormalTexture vertexPosition in this._vertexPositions)
            {
                float distanceP1 = Vector3.Distance(vertexPosition.Position + this._position, position);

                for (int index = 0; index < results.Length; index++)
                {
                    if (results[index] == null || distanceP1 < results[index].Distance)
                    {

                        var newResult = new DistanceAndPosition(distanceP1, vertexPosition.Position);
                        if (this.ResultExist(results, newResult))
                        {
                            break;
                        }

                        this.MoveResults(results, index);

                        results[index] = newResult;

                        break;
                    }
                }
            }

            // get Middle position
            DistanceAndPosition middlePoint = results[0];

            // front
            var positionInLineWithMiddleX = results.Where(w => w.Position.X == middlePoint.Position.X);
            var positionInLineWithMiddleY = results.Where(w => w.Position.Y == middlePoint.Position.Y);

            var front = positionInLineWithMiddleX.Where(w => w.Position.Y < middlePoint.Position.Y);
            var radiansX = front.Any() ? this.GetRadiantX(middlePoint.Position, front.First().Position) : 0;

            var right = positionInLineWithMiddleY.Where(w => w.Position.X < middlePoint.Position.X);
            var radiansY = right.Any() ? this.GetRadiantY(middlePoint.Position, right.First().Position) : 0;

            var rotationState = new Vector3(radiansX, radiansY * -1, 0);

            CoordinateDetails coordinateDetails = new CoordinateDetails
            {
                Position = middlePoint.Position,
                Rotation = rotationState
            };

            return coordinateDetails;
        }

        private float GetRadiantX(Vector3 pos1, Vector3 pos2)
        {
            if (pos1.Y > pos2.Y)
            {
                return (float)Math.Atan2(pos1.Y - pos2.Y, pos2.Z - pos1.Z);
            }

            if (pos1.Z > pos2.Z)
            {
                return (float)Math.Atan2(pos2.Y - pos1.Y, pos1.Z - pos2.Z);
            }

            return (float)Math.Atan2(pos2.Y - pos1.Y, pos2.Z - pos1.Z);
        }

        private float GetRadiantY(Vector3 pos1, Vector3 pos2)
        {
            if (pos1.X > pos2.X)
            {
                return (float)Math.Atan2(pos1.X - pos2.X, pos2.Z - pos1.Z);
            }

            if (pos1.Z > pos2.Z)
            {
                return (float)Math.Atan2(pos1.X - pos2.X, pos1.Z - pos2.Z);
            }

            return (float)Math.Atan2(pos2.X - pos1.X, pos2.Z - pos1.Z);
        }

        private void MoveResults(DistanceAndPosition[] results, int startIndex)
        {
            for (int index = results.Length - 1; index > startIndex; index--)
            {
                if (index - 1 >= startIndex)
                {
                    results[index] = results[index - 1];
                }
            }
        }

        public bool ResultExist(DistanceAndPosition[] results, DistanceAndPosition checkResult)
        {
            for (int index = 0; index < results.Length; index++)
            {
                if (results[index] == null)
                {
                    break;
                }

                if (results[index].Distance.Equals(checkResult.Distance) &&
                    results[index].Position.X == checkResult.Position.X &&
                    results[index].Position.Y == checkResult.Position.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public class DistanceAndPosition
        {
            public float Distance { get; set; } = 100000f;
            public Vector3 Position { get; set; }

            public DistanceAndPosition() { }

            public DistanceAndPosition(float distance, Vector3 position)
            {
                this.Distance = distance;
                this.Position = position;
            }

            public override string ToString()
            {
                return $"Distance: {this.Distance}, Position: {this.Position}";
            }
        }
    }
}
