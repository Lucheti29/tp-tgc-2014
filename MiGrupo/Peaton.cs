using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    public class Peaton : Persona
    {
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
            if (Utils.getDistance(_ptoRecorrido.X, _ptoRecorrido.Z, this.getMesh().Position.X, this.getMesh().Position.Z) > 1)
            {
                Vector3 movementVector = this.acercarse(_ptoRecorrido.X, _ptoRecorrido.Z,  VELOCIDAD * elapsedTime);
                rotacion = -FastMath.PI_HALF - Utils.calculateAngle(this.getMesh().Position.X, this.getMesh().Position.Z, _ptoRecorrido.X, _ptoRecorrido.Z);
                float antirotar = this.getMesh().Rotation.Y;
                this.getMesh().rotateY(rotacion - antirotar);
                this.caminar();
                this.getMesh().move(movementVector);
               // obb.move(movementVector);
                
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

    }
}

