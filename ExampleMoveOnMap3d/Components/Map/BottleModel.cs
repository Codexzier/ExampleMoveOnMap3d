using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class BottleModel
    {
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
    }
}
