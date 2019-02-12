using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ExampleMoveOnMap3dCore.Components.Map
{
    public class PlateGrassTile
    {
        private Vector3 _offsetPosition;
        private readonly float _scale = 1;
        private readonly Model _model;
        private readonly Matrix[] _transform;
        private readonly ModelMesh _mesh;
        private readonly bool _onlyOneMesh = false;
        private Vector3 _offsetRotation;

        public Vector3 Position { set; get; } = new Vector3();
        public Vector3 Rotation { get; set; } = new Vector3();
        
        public PlateGrassTile(Vector3 offsetPosition, float scale, Model model)
        {
            this._offsetPosition = offsetPosition;
            var offsetRotateX = MathHelper.ToRadians(90);
            this._offsetRotation = new Vector3(offsetRotateX, 0, 0);  

            this._scale = scale;
            this._model = model;

            this._transform = new Matrix[this._model.Bones.Count];
            this._model.CopyAbsoluteBoneTransformsTo(this._transform);

            this._mesh = this._model.Meshes.First();
            this._onlyOneMesh = !this._model.Meshes.Any();
        }
        
        public void Draw(Matrix view, Matrix projection)
        {

            foreach (ModelMesh mesh in this._model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
                    {
                        effectPass.Apply();
                    }

                    effect.EnableDefaultLighting();

                    this.SetTransform(effect, mesh);

                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

        private void SetTransform(BasicEffect effect, ModelMesh mesh)
        {
            effect.World = this._transform[mesh.ParentBone.Index] *
                            Matrix.CreateRotationX(this._offsetRotation.X + this.Rotation.X) *
                            Matrix.CreateRotationY(this._offsetRotation.Y + this.Rotation.Y) *
                            Matrix.CreateRotationZ(this._offsetRotation.Z + this.Rotation.Z) *
                            Matrix.CreateTranslation(this.Position + this._offsetPosition) *
                            Matrix.CreateScale(this._scale);
        }

     
    }
}
