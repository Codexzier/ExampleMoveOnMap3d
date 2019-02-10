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

        private PlateGrassTile[] _groundTiles = new PlateGrassTile[9];

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
                    this._groundTiles[index] = new PlateGrassTile(this.GetPosition(iX, iY),
                                                                    0.5f, 
                                                                    grass);
                    index++;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var item in this._groundTiles)
            {
                item.Position += new Vector3(this._componentInputs.Inputs.MoveX, 
                                                this._componentInputs.Inputs.MoveY, 0) 
                                                * _speed; 
            }
        }

        public void DrawContent(Matrix view, Matrix projection)
        {
            foreach (var item in this._groundTiles)
            {
                item.Draw(view, projection);
            }
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
