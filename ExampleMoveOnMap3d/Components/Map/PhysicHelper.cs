using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMoveOnMap3d.Components.Map
{
    /// <summary>
    /// Foundation formel uptrust / buoyancy: m = p * V
    /// https://de.wikipedia.org/wiki/Statischer_Auftrieb
    /// https://en.wikipedia.org/wiki/Buoyancy
    /// https://www.leifiphysik.de/mechanik/auftrieb-und-luftdruck/auftriebskraft
    /// 
    /// Foundation formal 
    /// </summary>
    public class PhysicHelper
    {
        /// <summary>
        /// Volume of the body.
        /// value unit is dm³
        /// </summary>
        private float _V_K = 1f;

        /// <summary>
        /// Medium
        /// value unit is kg/dm³
        /// </summary>
        private float _P_M = 1f;

        /// <summary>
        /// Weight
        /// value unit is N
        /// </summary>
        private float _F_G = 1f;

        /// <summary>
        /// 
        /// </summary>
        private float _mass = 1f;

        private float _friction = 1f;

        /// <summary>
        /// All value are 1. The result of all calculation return 1.
        /// </summary>
        public PhysicHelper()
        {
        }

        /// <summary>
        /// Setup the value to calculate the force on actual body.
        /// </summary>
        /// <param name="medium">Set medium value in kg/dm³</param>
        /// <param name="volume">Set object volume value in dm³</param>
        /// <param name="wieght">Set object weight value in N</param>
        /// <param name="mass">Set object mass value in kg</param>
        public PhysicHelper(float medium, float volume, float weight)
        {
            this._V_K = volume;
            this._P_M = medium;
            this._F_G = weight;

            // TODO: m = Fg / g
            // TODO: g = 9,81m/g²
            this._mass = weight / 9.81f;
        }

        /// <summary>
        /// Return Uptrust/buoyancy value.
        /// </summary>
        /// <param name="seaLevel"></param>
        /// <param name="objectLevel"></param>
        /// <returns></returns>
        public float CalculateUptrustValue(float seaLevel, float objectLevel)
        {
            if(objectLevel > seaLevel)
            {
                return this._mass * this._F_G * -1;
            }

            var fa = this._P_M * this._V_K * this._F_G;

            var fg = this._mass * this._F_G;

            // dawn
            if (fg > fa)
            {
                return fg - fa;
            }

            // up
            if (fa > fg)
            {
                return fa - fg;
            }

            return 0;
        }
    }
}
