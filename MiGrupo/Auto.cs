using AlumnoEjemplos.MiGrupo.Entities;
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
    public class Auto
    {
        static Auto _instance = new Auto();

        // --------------- Variables de instancia ---------------

        private TgcMesh _mesh;
        private Vector3 _direccion;
        //private Vector3 _direccionDerrape;
        private Velocity _velocidad;
        private float _currentElapsedTime;
        private bool _collisionFound;

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

        public float getVelocity()
        {
            return _velocidad.getAmount();
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
                        _velocidad.frenar(_currentElapsedTime);
                }
                else if (left || right)
                {
                    int yaw = 1;
                    int sign = 1;

                    if (left)
                    {
                        yaw = -1;
                        sign = -1;
                    }

                    _direccion = Movement.doblar(_direccion, _currentElapsedTime, yaw);
                    _direccion = Vector3.Normalize(_direccion);

                    //Calcular ángulo entre movimiento anterior y el nuevo
                    float angle = FastMath.Acos(Vector3.Dot(_direccion, direccionAuxiliar)) * sign;

                    //Rotar auto y camara en Y
                    _mesh.rotateY(angle);
                    Camara.rotar(angle);
                }
            }

            if (up)
                _velocidad.acelerar(_currentElapsedTime);
            else if (down)
                _velocidad.desacelerar(_currentElapsedTime);
            GuiController.Instance.UserVars.setValue("velocidad", _velocidad.getAmount());
        }

        public void checkCollision(TgcScene scene)
        {
            _collisionFound = false;
            foreach (TgcMesh mesh in scene.Meshes)
            {
                //Los dos BoundingBox que vamos a testear
                TgcBoundingBox mainMeshBoundingBox = _mesh.BoundingBox;
                TgcBoundingBox sceneMeshBoundingBox = mesh.BoundingBox;

                //Ejecutar algoritmo de detección de colisiones
                TgcCollisionUtils.BoxBoxResult collisionResult = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, sceneMeshBoundingBox);

                //Hubo colisión con un objeto. Guardar resultado y abortar loop.
                if (collisionResult == TgcCollisionUtils.BoxBoxResult.Adentro || collisionResult == TgcCollisionUtils.BoxBoxResult.Atravesando)
                {
                    _collisionFound = true;
                    break;
                }
            }
        }

        public void render(float elapsedTime)
        {
            Vector3 lastPosicion = getPosicion();
            _currentElapsedTime = elapsedTime;
            Teclado.handlear();
            _velocidad.friccion(_currentElapsedTime);
            Camara.setearPosicion(getPosicion());
            _mesh.move(_direccion * _velocidad.getAmount() * elapsedTime);

            //Si NO hay colision entonces movemos el taxi
            if (_collisionFound)
            {
                _mesh.Position = new Vector3(-980, 15, -3000);
                _velocidad = new Velocity();
                
            }
            
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
