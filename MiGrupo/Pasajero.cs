using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    class Pasajero
    {
        // string selectedAnim; LO ESTARIA REEMPLAZANDO POR ANIMACIONACTUAL
        TgcSkeletalMesh pasajeroMesh;
        string[] animationList;
//---propiedades
        private string animacionActual { get; set; }

        public Vector3 posicion
        {
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

        public void caminarHacia(Vector3 destino)
        {
            //pongo la animacion de caminar
            this.caminar();
            //tengo q trasladar el mesh hasta el destino
            //TODO-- HACER QUE EL PASAJERO VAYA HASTA EL TAXI--

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

