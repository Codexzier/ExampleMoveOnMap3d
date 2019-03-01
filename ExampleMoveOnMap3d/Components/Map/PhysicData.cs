using Microsoft.Xna.Framework;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class PhysicData
    {
        public float POWER = 600f;
        public float FRICTION = 60f;
        public float MASS = 10f;
        public float UPWARDTREND = 20f;

        public Vector3 GRAVITY = new Vector3(0, 0, -2f);

        public Vector3 EXTERNALFORCE;

        public Vector3 Velocity = new Vector3();
        public Vector3 SeaLevel = new Vector3();

        public PhysicData(float friction, float mass, float upwardTrend, Vector3 gravity)
        {
            this.FRICTION = friction;
            this.MASS = mass;
            this.UPWARDTREND = upwardTrend;
            this.GRAVITY = gravity;

            this.EXTERNALFORCE = this.GRAVITY * this.MASS;
        }
    }
}
