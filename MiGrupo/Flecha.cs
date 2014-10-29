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
    public class Flecha
    {
        private Vector3 _position = new Vector3(0,75,0);
        private Vector3 _direccion = new Vector3(0, 0, -1);
        private Vector3 _objetivo = new Vector3(-100, 5, -850);
        private Boolean _show = false;
        private static Flecha _instance;
        private TgcBox _mesh;

        public void inicializar()
        {
            Vector3 size = new Vector3(2, 2, 10);
            _mesh = TgcBox.fromSize(_position, size);
        }

        public void setPosition()
        {
            _mesh.getPosition(_position);
        }

        public TgcBox getMesh()
        {
            return _mesh;
        }

        public void rotate()
        {
            float angle = -FastMath.PI_HALF - Utils.calculateAngle(_position.X, _position.Z, _objetivo.X, _objetivo.Z);
            float antiRotate = _mesh.Rotation.Y;
            _mesh.rotateY(angle - antiRotate);
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
            _mesh.getPosition(_position);
            if(_show)
            {
                this.rotate();
                _mesh.render();
            }
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
