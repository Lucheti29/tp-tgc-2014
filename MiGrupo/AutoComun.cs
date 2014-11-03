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

        private TgcObb obb;
        private Vector3 _ptoRecorrido;
        private int i = 0;
        private float _velocidad = 100f;
        private float _velAux = 100f;

        private float rotacion = 0;
        private List<Vector3> _recorrido;

        private bool _collisionFound;

        public AutoComun(string archXML)
        {

            //primero cargamos una escena 3D entera.
            TgcSceneLoader loader = new TgcSceneLoader();

            //Luego cargamos otro modelo aparte que va a hacer el taxi
            TgcScene scene = loader.loadSceneFromFile(archXML);

            this._mesh = scene.Meshes[0];
            //Computar OBB a partir del AABB del mesh. Inicialmente genera el mismo volumen que el AABB, pero luego te permite rotarlo (cosa que el AABB no puede)
            obb = TgcObb.computeFromAABB(this._mesh.BoundingBox);

        }
        public void calculate(float elapsedTime)
        {
            if (_collisionFound)
            {
                _velAux = _velocidad;
                _velocidad = 0;

            }
            else
            {
                _velocidad = _velAux;
            }

            this.move(elapsedTime);
        }

        public void checkCollision(List<TgcMesh> obs)
        {
            _collisionFound = false;
            foreach (TgcMesh mesh in obs)
            {
                //Los dos BoundingBox que vamos a testear
                TgcBoundingBox mainMeshBoundingBox = _mesh.BoundingBox;

                TgcBoundingBox sceneMeshBoundingBox = mesh.BoundingBox;

                //Ejecutar algoritmo de detección de colisiones
                TgcCollisionUtils.BoxBoxResult collisionResult = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, sceneMeshBoundingBox);

                //Hubo colisión con un objeto. Guardar resultado y abortar loop.
                if (collisionResult != TgcCollisionUtils.BoxBoxResult.Afuera)
                {
                    _collisionFound = true;
                    break;
                }
            }
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
        }
     
        public void move(float elapsedtime)
        {
            if (Utils.getDistance(_ptoRecorrido.X, _ptoRecorrido.Z, this.getPosition().X,this.getPosition().Z) > 1)
            {
                Vector3 movementVector = acercarse(_ptoRecorrido.X, _ptoRecorrido.Z, _velocidad * elapsedtime);
                rotacion = -FastMath.PI_HALF - Utils.calculateAngle(_mesh.Position.X, _mesh.Position.Z, _ptoRecorrido.X, _ptoRecorrido.Z);
                float antirotar = _mesh.Rotation.Y;
                _mesh.rotateY(rotacion - antirotar);
                _mesh.move(movementVector);
                obb.move(movementVector);
             
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

        //retorna el vector movimiento al acercarse a tal punto a tal velocidad
        private Vector3 acercarse(float x, float z, float velocidad)
        {
            float angulo = Utils.calculateAngle(_mesh.Position.X, _mesh.Position.Z, x, z);

            return new Vector3(FastMath.Cos(angulo) * velocidad, 0, FastMath.Sin(angulo) * velocidad);
        }

        public void render()
        {
            _mesh.render();
            //Ver si hay que mostrar el BoundingBox
            if ((bool)GuiController.Instance.Modifiers.getValue("showBoundingBox"))
            {
                obb.render();
            }
        }
    }
}
