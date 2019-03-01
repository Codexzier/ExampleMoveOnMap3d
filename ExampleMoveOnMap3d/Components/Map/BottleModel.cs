using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class BottleModel
    {
        #region Physics

        public const float POWER = 600f;
        public const float FRICTION = 60f;
        public const float MASS = 80f;
        public const float UPWARDTREND = 20f;
        
        public readonly Vector3 GRAVITY = new Vector3(0, 0, -20);

        public Vector3 EXTERNALFORCE = new Vector3(0, 0, -20) * MASS;

        public Vector3 Velocity = new Vector3();
        public Vector3 SeaLevel = new Vector3();

        #endregion

        private Vector3 _position = new Vector3();
        private Vector3 _offsetPosition;
        private float _scale = 1;
        private readonly Model _model;
        private readonly Matrix[] _transform;
        
        protected Vector3 _rotation = new Vector3();
        protected Vector3 _offsetRotation;

        public Vector3 RelativePosition => this._position + this._offsetPosition;

        public Vector3 RelativeOffsetPosition { get; set; }
        public Vector3 Position => this._position;

        public BottleModel(Vector3 offsetPosition, Vector3 offsetRotation, float scale, Model model)
        {
            this._offsetPosition = offsetPosition;
            this._offsetRotation = offsetRotation;

            this._scale = scale;
            this._model = model;

            this._transform = new Matrix[this._model.Bones.Count];

            this._model.CopyAbsoluteBoneTransformsTo(this._transform);
        }

        internal void SetOffsetPosition(Vector3 vector3) => this._offsetPosition = vector3;
        internal void SetPosition(Vector3 vector3) => this._position = vector3;
        internal void SetOffsetRotation(Vector3 vector3) => this._offsetRotation = vector3;
        internal void SetRotation(Vector3 rotation) => this._rotation = rotation;

        public void AddRotation(Vector3 rotation)
        {
            this._rotation += rotation; // * .001f;
        }

        internal void AddPosition(Vector3 move, float speed = 1)
        {
            this._position += move * speed;

            Debug.WriteLine($"Pos: {this._position}");
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in this._model.Meshes)
            {
                this.IterateEffect(mesh, view, projection);
                mesh.Draw();
            }
        }

        internal void SetSeaLevel(Vector3 position)
        {
            this.SeaLevel = position;
        }

        private void IterateEffect(ModelMesh mesh, Matrix view, Matrix projection)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                this.EffectApply(effect);
                //
                effect.EnableDefaultLighting();
                //effect.TextureEnabled = false;

                effect.LightingEnabled = true;
               // this.SetLight(effect);

                this.SetTransform(effect, mesh);

                effect.View = view;
                effect.Projection = projection;
            }
        }

        private void EffectApply(BasicEffect effect)
        {
            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
            }
        }

        protected virtual void SetTransform(BasicEffect effect, ModelMesh mesh)
        {
            effect.World =   Matrix.CreateRotationX(this._rotation.X + this._offsetRotation.X) *
                            Matrix.CreateRotationY(this._rotation.Y + this._offsetRotation.Y) *
                            Matrix.CreateRotationZ(this._rotation.Z + this._offsetRotation.Z) *
                            Matrix.CreateTranslation(this._position + this._offsetPosition) *
                            Matrix.CreateScale(this._scale);
        }

        private void SetLight(BasicEffect effect)
        {
            effect.LightingEnabled = true; 
            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1); 
            effect.DirectionalLight0.Direction = new Vector3(-1f, 1f, 1f);  
            effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 10); 
            
        }

        public void Update(GameTime gameTime)
        {
            this.EXTERNALFORCE = this.GRAVITY * MASS;

            var upwardForce = this.Position.Z < this.SeaLevel.Z ? this.CalcUpwardTrend() : 0f;

            this.EXTERNALFORCE += new Vector3(0, 0, upwardForce);

            this.SetFriction(gameTime);
        }

        private float CalcUpwardTrend()
        {
            float upward = (this.SeaLevel.Z - this.Position.Z) * UPWARDTREND;


            return upward;
        }

        private void SetFriction(GameTime gameTime)
        {
            // TODO: Schräglage als Beschleunigung verwenden
            var velocityDirection = new Vector3();

            Vector3 friction = new Vector3(1, 1, .1f) * FRICTION;
            Vector3 powerDirection = (POWER * velocityDirection) + this.EXTERNALFORCE;

            Vector3 velocityChange = (2.0f / MASS * (powerDirection - friction * this.Velocity)) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Velocity += new Vector3(
                (float)(velocityChange.X < 0 ? -Math.Sqrt(-velocityChange.X) : Math.Sqrt(velocityChange.X)),
                (float)(velocityChange.Y < 0 ? -Math.Sqrt(-velocityChange.Y) : Math.Sqrt(velocityChange.Y)),
                (float)(velocityChange.Z < 0 ? -Math.Sqrt(-velocityChange.Z) : Math.Sqrt(velocityChange.Z)));
        }
    }
}
