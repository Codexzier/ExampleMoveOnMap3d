/// #########################################################################################################
///
///  Blog: Meine Welt in meinem Kopf
///  Post: 3d Karte bewegen (MonoGame)
///  Postdate: 10.02.2019
///  --------------------------------------------------------------------------------------------------------
///  Kurze Information:
///  Diese Solution dient als Quellcode Beispiel und zeigt lediglich 
///  die Funktionsweise für das Initialisieren des Sensors und abruf der Daten.
///  Fehlerbehandlung, sowie Logging oder andere Erweiterungen 
///  für eine stabile Laufzeit der Anwendung sind nicht vorhanden.
///  
///  Für Änderungsvorschläge oder ergänzende Informationen zu meiner
///  Beispiel Anwendung, der oder die kann mich unter der Mail Adresse 
///  j.langner@gmx.net erreichen.
///  
///  Vorraussetzung:
///  Windows 10
///  MonoGame SDK
/// 
/// #########################################################################################################

using ExampleMoveOnMap3d.Components.Debug;
using ExampleMoveOnMap3d.Components.Inputs;
using ExampleMoveOnMap3d.Components.Map;
using ExampleMoveOnMap3d.Components.Render;
using Microsoft.Xna.Framework;

namespace ExampleMoveOnMap3d
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
