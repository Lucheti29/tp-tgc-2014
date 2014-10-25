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



        //_--para el metodo moverPasajero 
        private int t;
        private static float DISTANCIA = 200f;
        private float rotacion = 0;
        private static float VELOCIDAD = 30.0f;
        private bool bajo = false;
        public Vector3 destino { get; set; }
        public bool llego { set; get; }
        private bool viajando { set; get; }
        //---para el metodo moverPasajero
        TgcBox marcaDestino;
        //constructor
        public Pasajero(string mesh, string textura)
            : base(mesh, textura)
        {

            this.viajando = false;
            this.llego = false;
        }

        public void cargarDestino(Vector3 destino)
        {
            //creo la la caja para marcar el destino
            Vector3 size = new Vector3(30, 0, 30);
            this.destino = destino;
            this.marcaDestino = TgcBox.fromSize(this.destino, size, Color.Green);
            this.marcaDestino.Enabled = false;
        }


        public void posicionar(Vector3 pos)//POSCIONA EL PASAJERO EN LA POS PASADA POR PARAMETRO
        {
            pasajeroMesh.move(pos);
            this.parar();
        }

        public void posicionar(float posX, float posZ)//POSCIONA EL PASAJERO EN LA POS PASADA POR PARAMETRO
        {
            pasajeroMesh.Position = new Vector3(posX, 5, posZ);
            this.parar();
        }

        //distancia entre el (_x,_z) pasajero, y el (x,z) pasados como parametro
        private float getDistancia(float x, float z)
        {
            return FastMath.Sqrt(FastMath.Pow2(x - pasajeroMesh.Position.X) + FastMath.Pow2(z - pasajeroMesh.Position.Z));
        }



        public void move(float elapsedTime)
        {
            Vector3 movementVector = new Vector3(0, 0, 0);
            Auto taxi = Auto.getInstance();
            if (!this.viajando && !this.llego && !this.bajo)
            {//EL PASAJERO ESPERA EL TAXI
                Vector3 posTaxi = new Vector3(taxi.getMesh().Position.X, taxi.getMesh().Position.Y, taxi.getMesh().Position.Z);

                t++; //t es un contador de frames
                /* 
                 * I.A. del Pasajero
                 *(1) por 140 frames el pasajero va hacia el taxi si estas cerca, si no se queda quieto 
                 
                 *
                 */
                if (t < 140)
                {

                    float distanciaAlTaxi = getDistancia(taxi.getMesh().Position.X, taxi.getMesh().Position.Z);

                    GuiController.Instance.UserVars.setValue("DistTaxi", distanciaAlTaxi);

                    if (distanciaAlTaxi < DISTANCIA && !taxi.llevaPasajero())
                    {
                        if (distanciaAlTaxi >= 70)
                        {//EL TAXI ESTA CERCA -> el pasaj intenta subirse
                            movementVector = acercarse(taxi.getMesh().Position.X, taxi.getMesh().Position.Z, VELOCIDAD * elapsedTime);
                            rotacion = -FastMath.PI_HALF - calcular_angulo(pasajeroMesh.Position.X, pasajeroMesh.Position.Z, taxi.getMesh().Position.X, taxi.getMesh().Position.Z);
                            this.caminar();
                        }
                        else
                        {//EL PASAJERO SE SUBIO AL TAXI-> se deshabilita el renderizado del pasajeroMesh
                            this.parar();
                            pasajeroMesh.Enabled = false;
                            this.marcaDestino.Enabled = true;
                            pasajeroMesh.Position = taxi.getMesh().Position;
                            this.viajando = true;
                            taxi.subePasajero();
                            GuiController.Instance.UserVars.setValue("posDest", this.destino);
                        }
                    }
                    else
                    {
                        this.parar();
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
                        pasajeroMesh.Position = taxi.getMesh().Position;

                    }

                    //t es un contador de frames
                    t++;

                    if (t < 140)
                    {

                        float distanciaDest = getDistancia(this.destino.X, this.destino.Z);//la distancia del pasajero(dentro del taxi) al destino

                        GuiController.Instance.UserVars.setValue("distDest", distanciaDest);

                        if (distanciaDest < 200 && this.viajando && (taxi.getVelocity() < 5 && taxi.getVelocity() > -5))
                        {//EL PASAJERO DEBE BAJARSE DEL TAXI ->se habilita el mesh del pasajero  
                            pasajeroMesh.Enabled = true;
                            this.posicionar(taxi.getPosicion().X + 10, taxi.getPosicion().Z + 10);
                            this.viajando = false;
                            this.bajo = true;
                            Cronometro.getInstance().incrementar(10);
                            taxi.bajaPasajero(); //el taxi no lleva mas un pasajero

                        }
                        if (!this.viajando)
                        {
                            if (distanciaDest > 10)
                            {
                                movementVector = acercarse(destino.X, destino.Z, VELOCIDAD * elapsedTime);
                                rotacion = -FastMath.PI_HALF - calcular_angulo(pasajeroMesh.Position.X, pasajeroMesh.Position.Z, destino.X, destino.Z);
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
            float antirotar = pasajeroMesh.Rotation.Y;
            pasajeroMesh.rotateY(rotacion - antirotar);
            pasajeroMesh.move(movementVector);
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
            float angulo = calcular_angulo(pasajeroMesh.Position.X, pasajeroMesh.Position.Z, x, z);

            return new Vector3(FastMath.Cos(angulo) * velocidad, 0, FastMath.Sin(angulo) * velocidad);

        }

        public override void render()
        {
            pasajeroMesh.animateAndRender();
            marcaDestino.render();
        }

        public override void dispose()
        {
            pasajeroMesh.dispose();
            marcaDestino.dispose();
        }





    }
}

