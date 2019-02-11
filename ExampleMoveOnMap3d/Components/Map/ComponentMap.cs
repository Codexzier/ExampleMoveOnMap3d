using ExampleMoveOnMap3d.Components.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class ComponentMap : GameComponent
    {
        private const float _speed = 0.3f;
        private readonly ComponentInputs _componentInputs;

        private PlateGrassTile[] _groundTiles = new PlateGrassTile[9];
        private PlateGrassTile[] _groundTiles2 = new PlateGrassTile[100];
        private PlateGrassTile[] _groundTiles3 = new PlateGrassTile[400];

        private Stopwatch _stopwatch = new Stopwatch();
        private int _resetStopWatchSec = 1;

        private int _step = 0;
        private int _normalStep = 2;

        private float _moveX = 0.1f;
        private float _moveY = 0.1f;

        private float _rotateX = 1f;
        private float _rotateY = 1f;

        public int ShowObjects { get; set; }

        public ComponentMap(Game game, ComponentInputs componentInputs) : base(game)
        {
            this._componentInputs = componentInputs;
        }

        public override void Initialize()
        {
            var grass = this.Game.Content.Load<Model>("Plate_Grass_01");

            int index = 0;
            for (int iY = 0; iY < 3; iY++)
            {
                for (int iX = 0; iX < 3; iX++)
                {
                    this._groundTiles[index] = new PlateGrassTile(this.GetPosition(iX, iY, this._groundTiles.Length),
                                                                    0.25f,
                                                                    grass);
                    index++;
                }
            }

            index = 0;
            for (int iY = 0; iY < 10; iY++)
            {
                for (int iX = 0; iX < 10; iX++)
                {
                    this._groundTiles2[index] = new PlateGrassTile(this.GetPosition(iX, iY, this._groundTiles2.Length),
                                                                    0.25f,
                                                                    grass);
                    index++;
                }
            }

            index = 0;
            for (int iY = 0; iY < 20; iY++)
            {
                for (int iX = 0; iX < 20; iX++)
                {
                    this._groundTiles3[index] = new PlateGrassTile(this.GetPosition(iX, iY, this._groundTiles3.Length),
                                                                    0.25f,
                                                                    grass);
                    index++;
                }
            }

            this._stopwatch.Start();
        }

        private PlateGrassTile[] GetTiles()
        {
            PlateGrassTile[] tilesTmp;
            switch (this._normalStep)
            {
                case 0:
                    tilesTmp = this._groundTiles;
                    break;
                case 1:
                    tilesTmp = this._groundTiles2;
                    break;
                case 2:
                    tilesTmp = this._groundTiles3;
                    break;
                default:
                    tilesTmp = new PlateGrassTile[0];
                    break;
            }

            return tilesTmp;
        }

        public override void Update(GameTime gameTime)
        {
            if(this._stopwatch.Elapsed.Seconds >= this._resetStopWatchSec)
            {
                if(this._step >= 9)
                {
                    this._step = 0;

                    if(this._normalStep >= 2)
                    {
                        this._normalStep = 0;
                    }
                    else
                    {
                        this._normalStep++;
                    }

                    PlateGrassTile[] tilesTmp = this.GetTiles();
                    
                    foreach (var item in tilesTmp)
                    {
                        item.Position = new Vector3();
                        item.Rotation = new Vector3();
                    }
                }
                else
                {
                    this._step++;
                }

                this._stopwatch.Restart();
            }

            var tiles = this.GetTiles();

            this.ShowObjects = tiles.Length;

            float moveX = 0f;
            float moveY = 0f;
            float rotateX = 0f;
            float rotateY = 0f;
            float rotateZ = 0f;

            switch (this._step)
            {
                case (0):
                    {
                        moveX += this._moveX;
                        break;
                    }
                case (1):
                    {
                        moveY += this._moveY;
                        break;
                    }
                case (2):
                    {
                        moveX -= this._moveX;
                        break;
                    }
                case (3):
                    {
                        moveY -= this._moveY;
                        break;
                    }
                case (4):
                    {
                        rotateX += this._rotateX;
                        break;
                    }
                case (5):
                    {
                        rotateY += this._rotateY;
                        break;
                    }
                case (6):
                    {
                        rotateX -= this._rotateX;
                        break;
                    }
                case (7):
                    {
                        rotateY -= this._rotateY;
                        break;
                    }
                case (8):
                    {
                        moveX -= this._moveX;
                        moveY -= this._moveY;
                        rotateX -= this._rotateX;
                        rotateY -= this._rotateY;
                        rotateZ -= 2;
                        break;
                    }
                case (9):
                    {
                        moveX += this._moveX;
                        moveY += this._moveY;
                        rotateX += this._rotateX;
                        rotateY += this._rotateY;
                        rotateZ += 2;
                        break;
                    }
                default:
                    //item.Position = new Vector3();
                    //item.Rotation = new Vector3();
                    //this._normal = !this._normal;
                    break;
            }

            foreach (var item in tiles)
            {
                item.Position += new Vector3(moveX, moveY, 0) * _speed;
                item.Rotation += new Vector3(MathHelper.ToRadians(rotateX), MathHelper.ToRadians(rotateY), MathHelper.ToRadians(rotateZ));
            }
        }

        public void DrawContent(Matrix view, Matrix projection)
        {
            var tiles = this.GetTiles();
            foreach (var item in tiles)
            {
                item.Draw(view, projection);
            }
        }

        private Vector3 GetPosition(int x, int y, int arrayLengt)
        {
            float mapLength = 3f;
            float distance = .02f;
            float centerMap = (mapLength + distance)
                                * (float)Math.Sqrt(arrayLengt) / 2;

            return new Vector3((y * (mapLength + distance)) - centerMap,
                                (x * (mapLength + distance)) - centerMap,
                                0);
        }
    }
}
