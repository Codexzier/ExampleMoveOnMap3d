using ExampleMoveOnMap3d.Components.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class ComponentMap : GameComponent
    {
        private const float _speed = 0.3f;
        private readonly ComponentInputs _componentInputs;
        private readonly int _player;
        private PlateGrassTile[] _groundTiles = new PlateGrassTile[9];

        private Car _car;

        public ComponentMap(Game game, ComponentInputs componentInputs, int player) : base(game)
        {
            this._componentInputs = componentInputs;
            this._player = player;

            
            
        }

        public override void Initialize()
        {
            string carModel = this._player == 1 ? "raceCarGreen" : "raceCarOrange";
            this._car = new Car(new Vector3(0, 0, 05), new Vector3(MathHelper.ToRadians(90), 0, MathHelper.ToRadians(180)), .2f, this.Game.Content.Load<Model>(carModel));

            var grass = this.Game.Content.Load<Model>("Plate_Grass_01");

            int index = 0;
            for (int iY = 0; iY < 3; iY++)
            {
                for (int iX = 0; iX < 3; iX++)
                {
                    this._groundTiles[index] = new PlateGrassTile(this.GetPosition(iX, iY),
                                                                    0.5f, 
                                                                    grass);
                    index++;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 positon = new Vector3();
            if (this._player == 1)
            {
                positon = new Vector3(this._componentInputs.Inputs1.MoveX, this._componentInputs.Inputs1.MoveY, 0) * _speed;
            }
            else
            {
                positon = new Vector3(this._componentInputs.Inputs2.MoveX, this._componentInputs.Inputs2.MoveY, 0) * _speed;
            }

            foreach (var item in this._groundTiles)
            {
                item.Position += positon;
            }
        }

        public void DrawContent(Matrix view, Matrix projection)
        {
            foreach (var item in this._groundTiles)
            {
                item.Draw(view, projection);
            }

            this._car.Draw(view, projection);
        }

        private Vector3 GetPosition(int x, int y)
        {
            float mapLength = 3f;
            float distance = .02f;
            float centerMap = (mapLength + distance) 
                                * (float)Math.Sqrt(this._groundTiles.Length) / 2; ;

            return new Vector3((y * (mapLength + distance)) - centerMap,
                                (x * (mapLength + distance)) - centerMap, 
                                0);
        }
    }
}
