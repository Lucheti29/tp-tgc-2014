using AlumnoEjemplos.MiGrupo.Entities;
using AlumnoEjemplos.MiGrupo.Enums;
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
        private bool foward; //indica si va para adelante
        //private Vector3 _direccionDerrape;
        private Velocity _velocidad;
        private float _currentElapsedTime;
        private bool _collisionFound;
        private bool _llevaPasajero;
        private TgcObb _obb;
        private Vector3 _objetivo;
        public List<Vector3> _pasajeros;// posiciones de los pasajeros
        private int _nroPasaj = 0;
        private List<TgcObb> _obstaculos;
        // --------------- Fin variables de instancia ---------------

        // --------------- Métodos de instancia ---------------
        public void inicializar(TgcMesh mesh)
        {
            _mesh = mesh;         
            _direccion = new Vector3(0, 0, -1);
            _velocidad = new Velocity();

            //Para que no esté en contacto con el suelo
            _mesh.move(0, 15, 0);
            //Computar OBB a partir del AABB del mesh.
            _obb = TgcObb.computeFromAABB(mesh.BoundingBox);
            _pasajeros = new List<Vector3>();//lista con la ubicacion de cada pasajero q debe llevar
            _obstaculos = new List<TgcObb>();
        }

        public TgcMesh getMesh()
        {
            return _mesh;
        }

        public Vector3 getPosicion()
        {
            return _mesh.Position;
        }
        public TgcObb orientedBB()
        {
            return _obb;
        }
        public List<TgcObb> getObstaculos()
        {
            return _obstaculos;
        }
        public Vector3 getDireccion()
        {
            return _direccion;
        }
        public Vector3 getObjetivo()
        {
            return _objetivo;
        }
        public float getVelocity()
        {
            return _velocidad.getAmount();
        }

        public bool llevaPasajero()
        {
            return _llevaPasajero;
        }

        public void subePasajero(Vector3 destino)
        {
            _llevaPasajero = true;
            _objetivo = destino;
        }

        public void bajaPasajero()
        {
            _llevaPasajero = false;
            if (_pasajeros.Count > 0)
            {
                _objetivo = _pasajeros[_nroPasaj++];
            }
        }
        #region movimiento
        private void derrapar(Boolean right, Boolean left)
        {
            //TODO: hacer
        }

        public void aplicarMovimiento()
        {
            bool right = Teclado.getInput(InputType.RIGHT);
            bool left = Teclado.getInput(InputType.LEFT);
            bool up = Teclado.getInput(InputType.UP);
            bool down = Teclado.getInput(InputType.DOWN);
            bool brake = Teclado.getInput(InputType.SPACE);

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
                    int yaw = 55;
                    int sign = 1;

                    if (left)
                    {
                        yaw *= -1;
                        sign = -1;
                    }

                    _direccion = Movement.doblar(_direccion, _currentElapsedTime, yaw);
                    _direccion = Vector3.Normalize(_direccion);

                    //Calcular ángulo entre movimiento anterior y el nuevo
                    float angle = FastMath.Acos(Vector3.Dot(_direccion, direccionAuxiliar)) * sign;

                    //Rotar auto y camara en Y
                    _mesh.rotateY(angle);

                    _obb.rotate(new Vector3(0, angle, 0));

                    Camara.rotar(angle);
                }
            }

            if (up)
            {
                if (_velocidad.getAmount() == 0)
                {
                    Sonido.getInstance().play(GuiController.Instance.AlumnoEjemplosMediaDir +"LOS_BARTO\\sonidos\\auto-encendido.wav",false);
                }
                _velocidad.acelerar(_currentElapsedTime);
                
                foward = true;
            }
            else if (down)
            {
                _velocidad.desacelerar(_currentElapsedTime);
                foward = false;
            }
            GuiController.Instance.UserVars.setValue("velocidad", this.getVelocity());
        }
        #endregion movimiento

        public void checkCollision()
        { 
            _collisionFound = false;
            foreach (TgcObb obstaculo in _obstaculos)
            {
                if (TgcCollisionUtils.testObbObb(this.orientedBB(), obstaculo))
                {
                    _collisionFound = true;
                    break;
                }
            }
        }

        public void calculate(float elapsedTime)
        {
            _currentElapsedTime = elapsedTime;
            this.aplicarMovimiento();
            _velocidad.friccion(_currentElapsedTime);
            Camara.setearPosicion(getPosicion());
            _mesh.move(_direccion * _velocidad.getAmount());
            _obb.move(_direccion * _velocidad.getAmount());

        
         
            if (_collisionFound)
            {
                _velocidad = new Velocity();
                Sonido.getInstance().play(GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTO\\sonidos\\auto-choquePequeño.wav",false);
                if (!foward)//puede haber problemas con el rebote
                {
                    _velocidad.setAmount(120f, elapsedTime);
                    foward = true;
                }
                else
                {
                    _velocidad.setAmount(120f * -1, elapsedTime);
                }
            }
        }

        public void render()
        {
            _mesh.render();
            //Ver si hay que mostrar el BoundingBox
            if ((bool)GuiController.Instance.Modifiers.getValue("showBoundingBox"))
            {
                _obb.render();
            }
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
