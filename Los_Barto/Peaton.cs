using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.Los_Barto
{
    public class Peaton : Persona
    {
        /// <summary>
        /// Peaton: persona que no se sube al taxi y
        /// tiene una ruta determinada por la cual caminar
        /// </summary>

        private List<Vector3> _recorrido;
        private Vector3 _ptoRecorrido;
        private float rotacion;
        private int i;

        public Peaton(string mesh, string textura)
            : base(mesh, textura)
        {

        }
        public void setRecorrido(List<Vector3> recorrido)
        {
            _recorrido = recorrido;
            _ptoRecorrido = recorrido[0];
        }
        public List<Vector3> getRecorrido()
        {
            return _recorrido;
        }
        public override void move(float elapsedTime)
        {
            if (!_collisionFound)
            {
                if (Utils.getDistance(_ptoRecorrido.X, _ptoRecorrido.Z, this.getMesh().Position.X, this.getMesh().Position.Z) > 1)
                {

                    float angulo = Utils.calculateAngle(this.getMesh().Position.X, this.getMesh().Position.Z, _ptoRecorrido.X, _ptoRecorrido.Z);
                    Vector3 movementVector = Utils.movementVector(VELOCIDAD * elapsedTime, angulo);
                    rotacion = -FastMath.PI_HALF - angulo;
                    float antirotar = this.getMesh().Rotation.Y;
                    this.getMesh().rotateY(rotacion - antirotar);

                    this.getMesh().move(movementVector);

                    this.caminar();
                }
                else
                {
                    i++;
                    if (i >= _recorrido.Count)
                    {
                        i = 0;

                    }
                    _ptoRecorrido = _recorrido[i];
                }
            }
            else
            {
                this.parar();
            }

        }

    }
}

