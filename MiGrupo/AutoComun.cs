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
        /// <summary>
        /// AutoComun: es un auto que va circulando por la ciudad
        /// por medio de una ruta predefinida
        /// </summary>

        private TgcMesh _mesh;
        private TgcObb obb;
        private Vector3 _ptoRecorrido;
        private int i = 0;
        private float _velocidad = 100f;
       

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
            

        }
        public void calculate(float elapsedTime)
        {

            float _lastVelocidad = _velocidad;
            Vector3 _lastPosition = this.getPosition();
            this.move(elapsedTime);
            if (_collisionFound)
            {
                //this.setPosition(_lastPosition);
                _mesh.Position = _lastPosition;
            }
            
        }

        private void createObb(Vector3 pos)
        {
            //Computar OBB a partir del AABB del mesh. Inicialmente genera el mismo volumen que el AABB, pero luego te permite rotarlo (cosa que el AABB no puede)
            obb = TgcObb.computeFromAABB(this._mesh.BoundingBox);

        }
        public void checkCollision( )
        { //el objeto con el q puede colisionar el AutoComun es el taxi
            _collisionFound = false;
         
            if (TgcCollisionUtils.testObbObb(this.obb, Auto.getInstance().orientedBB()))
            {   
                _collisionFound = true; 
            }

        }
        public TgcObb getOBB()
        {
            return obb;
        }
        public TgcMesh getMesh()
        {
            return _mesh;
        }

        public void dispose()
        {
            _mesh.dispose();
        }

        public void setPosition(Vector3 pos)
        {
            _mesh.Position = pos;
            if (obb == null)
            {
                createObb(pos);
            }
            else
            {
                obb.move(pos);
            }
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
            if (!_collisionFound)
            {
                if (Utils.getDistance(_ptoRecorrido.X, _ptoRecorrido.Z, this.getPosition().X, this.getPosition().Z) > 1)
                {
                    float angulo = Utils.calculateAngle(_mesh.Position.X, _mesh.Position.Z, _ptoRecorrido.X, _ptoRecorrido.Z);
                    Vector3 movementVector = Utils.movementVector(_velocidad * elapsedtime, angulo);
                    rotacion = -FastMath.PI_HALF - angulo;
                    float antirotar = _mesh.Rotation.Y;
                    _mesh.rotateY(rotacion - antirotar);
                    //obb.rotate(new Vector3(0, rotacion - antirotar, 0));
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
        }

    

        public void render()
        {
            _mesh.render();
            obb.updateValues();
            //Ver si hay que mostrar el obb
            if ((bool)GuiController.Instance.Modifiers.getValue("showBoundingBox"))
            {
               
                obb.render();
            }
        }
    }
}
