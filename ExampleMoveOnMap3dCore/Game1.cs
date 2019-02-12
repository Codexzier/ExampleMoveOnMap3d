using ExampleMoveOnMap3dCore.Components.DebugInfo;
using ExampleMoveOnMap3dCore.Components.Inputs;
using ExampleMoveOnMap3dCore.Components.Map;
using ExampleMoveOnMap3dCore.Components.Render;
using Microsoft.Xna.Framework;

namespace ExampleMoveOnMap3dCore
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ComponentInputs _componentInputs;
        private readonly ComponentMap _componentMap;
        private readonly ComponentRender _componentRender;
        private readonly ComponentDebug _componentDebug;

        public Game1()
        {
            this._graphics = new GraphicsDeviceManager(this);


            this.Content.RootDirectory = "Content";

            this.Window.Title = "Move on map 3d";
            this.IsMouseVisible = true;

            this._componentInputs = new ComponentInputs(this);
            this._componentInputs.UpdateOrder = 1;
            this.Components.Add(this._componentInputs);

            this._componentMap = new ComponentMap(this, this._componentInputs);
            this._componentMap.UpdateOrder = 2;
            this.Components.Add(this._componentMap);

            this._componentRender = new ComponentRender(this, this._componentMap);
            this._componentRender.UpdateOrder = 3;
            this._componentRender.DrawOrder = 1;
            this.Components.Add(this._componentRender);

            this._componentDebug = new ComponentDebug(this, this._componentRender);
            this._componentDebug.UpdateOrder = 4;
            this._componentDebug.DrawOrder = 2;
            this.Components.Add(this._componentDebug);
        }
    }
}
