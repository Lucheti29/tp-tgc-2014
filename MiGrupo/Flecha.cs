using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    class Flecha
    {
        private Vector3 _position = new Vector3(0,75,0);
        private Vector3 _direccion = new Vector3(0, 0, -1);
        private Vector3 _objetivo = new Vector3(-1037, 857, -1070);
        private Boolean _show = false;
        private static Flecha _instance;
        private TgcBox _mesh;
        private float _currentElapsedTime = 0f;

        public void inicializar()
        {
            Vector3 size = new Vector3(2, 2, 10);
            _mesh = TgcBox.fromSize(_position, size);
        }

        //El Y es constante
        public void setPosition()
        {
            _mesh.getPosition(_position);
        }

        //No es funcional aun
        public void rotate()
        {
            //Vector3 vecAuxiliar = new Vector3(_direccion.X, _direccion.Y, _direccion.Z);
            Vector3 vecAuxiliar2 = new Vector3(_objetivo.X, _objetivo.Y, _objetivo.Z);

            float angle = FastMath.Acos(Vector3.Dot(Vector3.Normalize(_direccion), Vector3.Normalize(vecAuxiliar2)));

            float yaw = FastMath.ToRad(angle);
            Matrix rotation = Matrix.RotationYawPitchRoll(yaw, 0, 0);

            Vector4 transformedVec4 = Vector3.Transform(_direccion, rotation);
            Vector3 transformedVec3 = new Vector3(transformedVec4.X, transformedVec4.Y, transformedVec4.Z);

            _direccion = transformedVec3;
        }

        public void rotateY(float angle)
        {
            _mesh.rotateY(angle);
        }

        public void show()
        {
            _show = true;
        }

        public void hide()
        {
            _show = false;
        }

        public void render(float elapsedTime)
        {
            //_currentElapsedTime = elapsedTime;
            //if(_show)
            //{
            //    this.setPosition();
                //this.rotate();
            //    _mesh.render();
            //}
        }

        public static Flecha getInstance()
        {
            if (_instance == null)
            {
                _instance = new Flecha();
            }

            return _instance;
        }
    }
}
