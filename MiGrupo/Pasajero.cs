using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    public class Pasajero : Persona
    {
        /// <summary>
        /// Pasajero: es la persona que se sube al taxi
        /// cuando se encuentra a una distancia determinada.
        /// Al subir, se crea un triángulo en su destino y al llegar
        /// se baja y camina hasta el mismo
        /// </summary>

        private int t;
        private static float DISTANCIA = 200f;
        private float rotacion = 0;

        private bool bajo = false;
        public Vector3 destino { get; set; }
        public bool llego { set; get; }
        private bool viajando { set; get; }
        //---para el metodo moverPasajero
        TgcBox marcaDestino;
        private bool llamoAltaxi;
        //constructor
        public Pasajero(string mesh, string textura)
            : base(mesh, textura)
        {

            this.viajando = false;
            this.llego = false;
            this.llamoAltaxi = false;
        }

        public void cargarDestino(Vector3 destino)
        {
            //creo la la caja para marcar el destino
            Vector3 size = new Vector3(30, 0, 30);
            this.destino = destino;
            this.marcaDestino = TgcBox.fromSize(this.destino, size, Color.Green);
            this.marcaDestino.Enabled = false;
        }




        public override void move(float elapsedTime)
        {
            Vector3 movementVector = new Vector3(0, 0, 0);
            Auto taxi = Auto.getInstance();
            if (!this.viajando && !this.llego && !this.bajo)
            {//EL PASAJERO ESPERA EL TAXI
                t++; //t es un contador de frames
                /* 
                 * I.A. del Pasajero
                 *(1) por 140 frames el pasajero va hacia el taxi si estas cerca, si no se queda quieto 
                 
                 *
                 */
                if (t < 140)
                {

                    float distanciaAlTaxi = Utils.getDistance(_mesh.Position.X, _mesh.Position.Z, taxi.getMesh().Position.X, taxi.getMesh().Position.Z);

                    if (distanciaAlTaxi < DISTANCIA && !taxi.llevaPasajero())
                    {
                        if (distanciaAlTaxi >= 70)
                        {//EL TAXI ESTA CERCA -> el pasaj intenta subirse
                            if (!llamoAltaxi)
                            {
                                Sonido.getInstance().play(GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTO\\sonidos\\taxi.wav", false);
                                llamoAltaxi = true;
                            }
                            float angulo = Utils.calculateAngle(_mesh.Position.X, _mesh.Position.Z, taxi.getMesh().Position.X, taxi.getMesh().Position.Z);
                            movementVector = Utils.movementVector(VELOCIDAD * elapsedTime, angulo);
                            rotacion = -FastMath.PI_HALF - angulo;
                            this.caminar();
                        }
                        else
                        {
                            if (taxi.getVelocity() < 5 && taxi.getVelocity() > -5)
                            {

                                if (_collisionFound)
                                { //EL PASAJERO SE SUBIO AL TAXI-> se deshabilita el renderizado del pasajeroMesh
                                    this.parar();
                                    _mesh.Enabled = false;

                                    this.marcaDestino.Enabled = true;
                                    _mesh.Position = taxi.getMesh().Position;
                                    this.viajando = true;
                                    taxi.subePasajero(this.destino);

                                }
                            }
                        }
                    }
                    else
                    {
                        this.parar();
                        llamoAltaxi = false;
                    }

                }
                else
                    t = 0;

            }
            else
            {
                if (!this.llego)
                {//EL PASAJERO ESTA VIAJANDO EN EL TAXI-> la posicion del pasajero es la del taxi
                    if (this.viajando)
                    {
                        _mesh.Position = taxi.getMesh().Position;

                    }

                    //t es un contador de frames
                    t++;

                    if (t < 140)
                    {

                        float distanciaDest = Utils.getDistance(_mesh.Position.X, _mesh.Position.Z, this.destino.X, this.destino.Z);//la distancia del pasajero(dentro del taxi) al destino

                        if (distanciaDest < 200 && this.viajando && (taxi.getVelocity() < 5 && taxi.getVelocity() > -5))
                        {//EL PASAJERO DEBE BAJARSE DEL TAXI ->se habilita el mesh del pasajero  


                            this._mesh.Position = new Vector3(taxi.getPosicion().X, 5, taxi.getPosicion().Z);
                            this.viajando = false;
                            this.bajo = true;
                            Cronometro.getInstance().incrementar(10);
                            taxi.bajaPasajero(); //el taxi no lleva mas un pasajero

                        }
                        if (!this.viajando)
                        {// el pasajero se bajo del taxi ahora camina hacia el destino
                            if (!_collisionFound)
                            {
                                _mesh.Enabled = true;
                            }
                            if (distanciaDest > 10)
                            {
                              
                                float angulo = Utils.calculateAngle(_mesh.Position.X, _mesh.Position.Z, destino.X, destino.Z);
                                movementVector = Utils.movementVector(VELOCIDAD * elapsedTime, angulo);
                                rotacion = -FastMath.PI_HALF - angulo;
                                this.caminar();
                            }
                            else
                            {
                                this.llego = true;
                                this.parar();
                                marcaDestino.Enabled = false;

                            }
                        }


                    }
                    else
                        t = 0;
                }
            }
            float antirotar = _mesh.Rotation.Y;
            _mesh.rotateY(rotacion - antirotar);
            _mesh.move(movementVector);

        }



        public override void render()
        {
            _mesh.updateAnimation();
            /* Vector3 lastpos = new Vector3();
             _mesh.getPosition(lastpos);
             if (_collisionFound)
             {
                 this.posicionar(lastpos);
             }*/
            _mesh.render();

            if ((bool)GuiController.Instance.Modifiers.getValue("showBoundingBox"))
            {

                _mesh.BoundingBox.render();
            }
            marcaDestino.render();
        }

        public override void dispose()
        {
            _mesh.dispose();

            marcaDestino.dispose();
        }
    }
}


