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
    public class AutoComun
    {
        private TgcMesh _mesh;
        private Vector3 _ptoRecorrido;
        private int i = 0;
        private float _velocidad = 100f;
        private int frames;
        private float rotacion = 0;
        private List<Vector3> _recorrido;
        const float epsilon = 0.001f; // error de la distancia al objetivo
        public AutoComun(string archXML)
        {
            
            //primero cargamos una escena 3D entera.
            TgcSceneLoader loader = new TgcSceneLoader();

            //Luego cargamos otro modelo aparte que va a hacer el taxi
            TgcScene scene = loader.loadSceneFromFile( archXML);

            this._mesh= scene.Meshes[0];

        }
        public void render()
        {
            _mesh.render();
        }
        public void dispose()
        {
            _mesh.dispose();
        }
        public void setPosition(Vector3 pos)
        {
            
            _mesh.Position = pos;
        }
        public Vector3 getPosition()
        {
            return _mesh.Position;
        }
        public void setRecorrido(List<Vector3> recorrido)
        {
            _recorrido = recorrido;
            _ptoRecorrido = recorrido[0];
            GuiController.Instance.UserVars.setValue("ptosRec", _ptoRecorrido);
        }

        //distancia entre el (_x,_z) auto, y el (x,z) pasados como parametro
        private float getDistancia(float x, float z)
        {
            return FastMath.Sqrt(FastMath.Pow2(x - _mesh.Position.X) + FastMath.Pow2(z - _mesh.Position.Z));
        }
        
        public void move(float elapsedtime)
        {
            if ( getDistancia(_ptoRecorrido.X, _ptoRecorrido.Z) >1)
            {
                Vector3 movementVector = acercarse(_ptoRecorrido.X, _ptoRecorrido.Z, _velocidad * elapsedtime);
                rotacion = -FastMath.PI_HALF - calcular_angulo(_mesh.Position.X, _mesh.Position.Z, _ptoRecorrido.X, _ptoRecorrido.Z);
                float antirotar = _mesh.Rotation.Y;
                _mesh.rotateY(rotacion - antirotar);
                _mesh.move(movementVector);
               GuiController.Instance.UserVars.setValue("DistpRec", getDistancia(_ptoRecorrido.X, _ptoRecorrido.Z));
            }
            else
            {
                i++;
                if (i < _recorrido.Count)
                {
                    _ptoRecorrido = _recorrido[i];
                    GuiController.Instance.UserVars.setValue("ptosRec", _ptoRecorrido);
                }
                else
                {
                    i = 0;
                }
            }

          
        }

        //retorna el angulo formado por ambos puntos
        private float calcular_angulo(float posPasajX, float posPasjZ, float posTaxiX, float posTaxiZ)
        {
            float cateto_o;
            float cateto_a;
            float angulo = 0.0f;
            bool arriba = true;
            bool iguales = false;

            if (posPasjZ < posTaxiZ)
                arriba = true;
            else if (posPasjZ > posTaxiZ)
                arriba = false;
            else
                iguales = true;

            cateto_a = posTaxiX - posPasajX;
            cateto_o = posTaxiZ - posPasjZ;


            if (arriba)
            {

                angulo = FastMath.Atan((float)(cateto_o / cateto_a));

                if (angulo < 0.0f)
                    angulo = FastMath.PI + angulo;

            }//arriba
            else if (!arriba)
            {

                angulo = FastMath.Atan((float)(cateto_o / cateto_a));

                if (angulo >= 0.0f)
                    angulo = FastMath.PI + angulo;
                else
                    angulo = FastMath.PI * 2 + angulo;

            }//abajo


            if (iguales)
            {
                if (cateto_a > 0)
                    angulo = 0.0f;
                else
                    angulo = FastMath.PI;
            }

            return angulo;
        }


        //retorna el vector movimiento al acercarse a tal punto a tal velocidad
        private Vector3 acercarse(float x, float z, float velocidad)
        {
            float angulo = calcular_angulo(_mesh.Position.X, _mesh.Position.Z, x, z);

            return new Vector3(FastMath.Cos(angulo) * velocidad, 0, FastMath.Sin(angulo) * velocidad);

        }

    }
}
