using AlumnoEjemplos.MiGrupo.Entities;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    public class Auto
    {
        static Auto _instance = new Auto();

        // --------------- Variables de instancia ---------------

        private TgcMesh _mesh;
        private Vector3 _direccion;
        //private Vector3 _direccionDerrape;
        private Velocity _velocidad;

        // --------------- Fin variables de instancia ---------------

        // --------------- Métodos de instancia ---------------
        public void inicializar(TgcMesh mesh)
        {
            _mesh = mesh;
            _direccion = new Vector3(0,0,-1);
            _velocidad = new Velocity();
        }

        public TgcMesh getMesh()
        {
           return _mesh;
        }

        public Vector3 getPosicion()
        {
            return _mesh.Position;
        }

        private float getVelocity()
        {
            return _velocidad.getAmount();
        }

        private void acelerar()
        {
            _velocidad.acelerar();
        }

        private void desacelerar()
        {
            _velocidad.desacelerar();
        }

        private void frenar()
        {
            _velocidad.frenar();
        }

        private void doblar(Boolean right, Boolean left)
        {
            if (right)
                _direccion = Movement.rightMove(_direccion);
            else if (left)
                _direccion = Movement.leftMove(_direccion);
        }

        private void derrapar(Boolean right, Boolean left)
        {
            //TODO: hacer
        }

        public void aplicarMovimiento(Boolean right, Boolean left, Boolean up, Boolean down, Boolean brake)
        {
            Vector3 direccionAuxiliar = new Vector3(_direccion.X, _direccion.Y, _direccion.Z);

            if (!(_velocidad.getAmount() < 0.5f && _velocidad.getAmount() > -0.5f))
            {
                if (brake)
                {
                    if (right)
                        derrapar(true, false);
                    else if (left)
                        derrapar(false, true);
                    else
                        frenar();
                }
                else if (left || right)
                {
                    int signAngle = 1;

                    if (left)
                    {
                        signAngle *= -1;
                        doblar(false, true);
                    }
                    else if (right)
                    {
                        doblar(true, false);
                    }

                    _direccion = Vector3.Normalize(_direccion);

                    //Calcular ángulo entre movement original y el nuevo
                    float angle = FastMath.Acos(Vector3.Dot(_direccion, direccionAuxiliar));

                    //Rotar auto y camara en Y
                    _mesh.rotateY(signAngle * angle);
                    Camara.rotar(signAngle * angle);
                }
            }

            if (up)
                acelerar();
            else if (down)
                desacelerar();
        }

        public void render(float elapsedTime)
        {
            Teclado.handlear();
            _velocidad.friccion();
            Camara.setearPosicion(getPosicion());
            _mesh.move(_direccion * _velocidad.getAmount() * elapsedTime);
            _mesh.render();
        }
        // --------------- Fin de métodos de instancia ---------------

        // --------------- Métodos estáticos ---------------
        public static Auto getInstance()
        {
            if (_instance == null)
            {
                _instance = new Auto();
            }
            return _instance;
        }

        public static void reset()
        {
            _instance = new Auto();
        }
        // --------------- Fin Métodos estáticos ---------------



    }
}
