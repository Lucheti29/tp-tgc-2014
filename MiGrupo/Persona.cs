using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.MiGrupo
{
    public class Persona
    {
       protected TgcSkeletalMesh pasajeroMesh;
       protected string[] animationList;
       //---propiedades
       protected string animacionActual { get; set; }
     
       public Vector3 posicion
       {//donde se encuentra el pasajero actualmente 
           get { return pasajeroMesh.Position; }
       }
       public Persona(string mesh, string textura)
       {          
          //Paths para archivo XML de la malla
           string pathMesh = GuiController.Instance.ExamplesMediaDir + mesh;
           //Path para carpeta de texturas de la malla
           string mediaPath = GuiController.Instance.ExamplesMediaDir + textura;
         

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
       protected void parar()
       {
           if (this.animacionActual != animationList[0])//me fijo si ya la animacion actual es diferente a la q quiero  
           {
               pasajeroMesh.stopAnimation();// detengo la animacion actual
           }
           this.animacionActual = animationList[0];//ANIMACION STANDBY en la lista de animaciones
           pasajeroMesh.playAnimation(this.animacionActual, true);
       }



       protected void caminar()
       {
           if (this.animacionActual != animationList[1])//me fijo si la animacion actual es diferente a la q quiero  
           {
               pasajeroMesh.stopAnimation();// detengo la animacion actual
           }
           //le asigno la animacion de caminar y la ejecuto
           this.animacionActual = animationList[1];
           pasajeroMesh.playAnimation(this.animacionActual, true);
       }
       protected TgcSkeletalMesh getMesh()
       {
           return pasajeroMesh;
       }



       public void move(){


       }
       public virtual void render()
       {
           pasajeroMesh.animateAndRender();
           
       }

       public virtual void dispose()
       {
           pasajeroMesh.dispose();
           
       }

    }
}
