using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{

    class Pasajero
    {
       
        TgcSkeletalMesh pasajeroMesh;
        string[] animationList;
        //---propiedades
        private string animacionActual { get; set; }
        public Vector3 destino { get; set; }//la posicion donde quiere ir el pasajero
        public Vector3 posicion
        {//donde se encuentra el pasajero actualmente 
            get { return pasajeroMesh.Position; }
            set { pasajeroMesh.move(value); }
        }
        //--propiedades

        //constructor
        public Pasajero()
        {

            //Paths para archivo XML de la malla
            string pathMesh = GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml";
            //Path para carpeta de texturas de la malla
            string mediaPath = GuiController.Instance.ExamplesMediaDir + "SkeletalAnimations\\BasicHuman\\";

            //Lista de animaciones disponibles
            animationList = new string[]{
                "StandBy",
                "Walk",
            };

            //Crear rutas con cada animacion
            string[] animationsPath = new string[animationList.Length];
            for (int i = 0; i < animationList.Length; i++)
            {
                animationsPath[i] = mediaPath + "Animations\\" + animationList[i] + "-TgcSkeletalAnim.xml";
            }

            //Cargar mesh y animaciones
            TgcSkeletalLoader loaderSkeletal = new TgcSkeletalLoader();
            pasajeroMesh = loaderSkeletal.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);

        }

        //metodos
        public void parar()
        {
            if (this.animacionActual != animationList[0])//me fijo si ya la animacion actual es diferente a la q quiero  
            {
                pasajeroMesh.stopAnimation();// detengo la animacion actual
            }
            this.animacionActual = animationList[0];
            pasajeroMesh.playAnimation(this.animacionActual, true);
        }

        public void parar(Vector3 pos)
        {
            this.posicion = pos;
            this.parar();
        }

        private void caminar()
        {
            if (this.animacionActual != animationList[1])//me fijo si la animacion actual es diferente a la q quiero  
            {
                pasajeroMesh.stopAnimation();// detengo la animacion actual
            }
            //le asigno la animacion de caminar y la ejecuto
            this.animacionActual = animationList[1];
            pasajeroMesh.playAnimation(this.animacionActual, true);
        }

      

        //distancia entre el (_x,_z) enemigo, y el (x,z) pasados como parametro
        public float getDistancia(float x, float z)
        {
            return FastMath.Sqrt(FastMath.Pow2(x - pasajeroMesh.Position.X) + FastMath.Pow2(z - pasajeroMesh.Position.Z));
        }

        private int t;
        public static float DISTANCIA = 200f;
        private float rotacion = 0;
        public static float VELOCIDAD = 4.0f;

        public void movePasajero(float elapsedTime, Auto taxi)
        {
            Vector3 posTaxi = new Vector3(taxi.getMesh().Position.X, taxi.getMesh().Position.Y, taxi.getMesh().Position.Z);
            Vector3 movementVector = new Vector3(0, 0, 0);

            //t es un contador de frames
            t++;

            /*
             * 
             * I.A. del Pasajero
             *(1) por 140 frames el pasajero va hacia el taxi si estas cerca, si no se queda quieto 
             *(2) luego por 80 frames se queda quieto
             *vuelve a (1) y asi
             *
             */

            if (t < 140)
            {
                float distanciaAlTaxi = getDistancia(taxi.getMesh().Position.X, taxi.getMesh().Position.Z);
                if ((distanciaAlTaxi < DISTANCIA) && (distanciaAlTaxi >= 50))
                {
                    movementVector = acercarse(taxi.getMesh().Position.X, taxi.getMesh().Position.Z, VELOCIDAD);
                    rotacion = -FastMath.PI_HALF - calcular_angulo(pasajeroMesh.Position.X, pasajeroMesh.Position.Z, taxi.getMesh().Position.X, taxi.getMesh().Position.Z);
                    
                    this.caminar();
                }
                if (distanciaAlTaxi < 50)
                {
                    this.parar();
                    pasajeroMesh.Enabled = false;
                }

            }
            else if (t >= 140 && t < 220)
            {
                this.parar();
            }
            else
                t = 0;



            // collisionManager.SlideFactor = (float)TgcViewer.GuiController.Instance.Modifiers["SlideFactor"];


            float antirotar = pasajeroMesh.Rotation.Y;
            pasajeroMesh.rotateY(rotacion - antirotar);

            //Mover personaje con detección de colisiones, sliding y gravedad
            //   Vector3 realMovement = collisionManager.moveCharacter(characterSphere, movementVector, objetosColisionables);
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
        public void bajarseDelTaxi(Auto taxi)//FIJARSE PORQUE NO SE RENDERIZA DE NUEVO
        {
            if (Vector3.Length(Auto.getInstance().getPosicion() - this.destino) < 100)
            {
                pasajeroMesh.Enabled = true;//SIRVE PARA Q VUELVA A RENDERIZAR (ESTA PROBADO EN EL EJEMPLOALUMNO)
               /* pasajeroMesh.move(10, 5, 15);
                pasajeroMesh.Position(10, 5, 15);*/
                pasajeroMesh.Transform.Translate(10,5,15);
            }
        }
        public TgcSkeletalMesh getMesh()
        {
            return pasajeroMesh;
        }
        public void render()
        {
            pasajeroMesh.animateAndRender();
        }

        public void dispose()
        {
            pasajeroMesh.dispose();
        }


    }
}

