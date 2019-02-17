using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ExampleMoveOnMap3d.Components.Map
{
    public class Car
    {
        protected Vector3 _position = new Vector3();
        protected Vector3 _offsetPosition;
        public float _scale = 1;
        protected readonly Model _model;
        protected readonly Matrix[] _transform;
        protected readonly ModelMesh _mesh;
        protected readonly bool _onlyOneMesh = false;

        protected Vector3 _rotation = new Vector3();
        protected Vector3 _offsetRotation;

        public Car(Vector3 offsetPosition, Vector3 offsetRotation, float scale, Model model)
        {
            this._offsetPosition = offsetPosition;
            this._offsetRotation = offsetRotation;

            this._scale = scale;
            this._model = model;

            this._transform = new Matrix[this._model.Bones.Count];

            this._model.CopyAbsoluteBoneTransformsTo(this._transform);

            this._mesh = this._model.Meshes.First();
            this._onlyOneMesh = this._model.Meshes.Count == 1;
        }

        internal void SetOffsetPosition(Vector3 vector3) => this._offsetPosition = vector3;
        internal void SetPosition(Vector3 vector3) => this._position = vector3;

        public void AddRotation(Vector3 rotation)
        {
            this._rotation += rotation;
        }

        internal void AddPosition(Vector3 move, float speed = 1)
        {
            this._position += move * speed;
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

                effect.EnableDefaultLighting();
                effect.TextureEnabled = false;

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

            effect.World = this._transform[mesh.ParentBone.Index] *
                            Matrix.CreateRotationX(this._rotation.X + this._offsetRotation.X) *
                            Matrix.CreateRotationY(this._rotation.Y + this._offsetRotation.Y) *
                            Matrix.CreateRotationZ(this._rotation.Z + this._offsetRotation.Z) *
                            Matrix.CreateTranslation(this._position + this._offsetPosition) *
                            Matrix.CreateScale(this._scale);
        }
    }
}
