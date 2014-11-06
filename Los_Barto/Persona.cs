using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcSkeletalAnimation;

namespace AlumnoEjemplos.Los_Barto
{
    public class Persona
    {
        /// <summary>
        /// Persona: clase padre de Pasajero y Peaton
        /// Describe la lógica común a estos dos (caminar, moverse, parar)
        /// </summary>

        protected TgcSkeletalMesh _mesh;
        protected string[] animationList;
        //---propiedades
        protected string animacionActual { get; set; }
        protected static float VELOCIDAD = 30.0f;

        protected bool _collisionFound;

        public Vector3 posicion
        {//donde se encuentra el pasajero actualmente 
            get { return _mesh.Position; }
        }
        public void posicionar(Vector3 pos)//POSCIONA EL PASAJERO EN LA POS PASADA POR PARAMETRO
        {
            _mesh.move(pos);
            this.parar();

        }

        public void posicionar(float posX, float posZ)//POSCIONA EL PASAJERO EN LA POS PASADA POR PARAMETRO
        {
            posicionar(new Vector3(posX, 5, posZ));
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
            _mesh = loaderSkeletal.loadMeshAndAnimationsFromFile(pathMesh, mediaPath, animationsPath);


        }
        //metodos
        protected void parar()
        {
            if (this.animacionActual != animationList[0])//me fijo si ya la animacion actual es diferente a la q quiero  
            {
                _mesh.stopAnimation();// detengo la animacion actual
            }
            this.animacionActual = animationList[0];//ANIMACION STANDBY en la lista de animaciones
            _mesh.playAnimation(this.animacionActual, true);
        }

        protected void caminar()
        {
            if (this.animacionActual != animationList[1])//me fijo si la animacion actual es diferente a la q quiero  
            {
                _mesh.stopAnimation();// detengo la animacion actual
            }
            //le asigno la animacion de caminar y la ejecuto
            this.animacionActual = animationList[1];
            _mesh.playAnimation(this.animacionActual, true);
        }
        protected TgcSkeletalMesh getMesh()
        {
            return _mesh;
        }
        /* posible implementacion de la sangre
       /// <summary>
       /// cuando el taxi atropella a la persona
       /// </summary>
       public void colision()
       {
           _mesh.Enabled = false;
           //creo la la caja para marcar el destino
           Vector3 size = new Vector3(30, 0, 30);
           TgcBox sangre = TgcBox.fromSize(_mesh.Position, size);
           sangre.setTexture(TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTOS\\blood1.png"));
       }
        */
        public virtual void move(float elapsedTime) { }
        public void checkCollision()
        { //el objeto con el q puede colisionar el AutoComun es el taxi
            _collisionFound = false;
            if (TgcCollisionUtils.testObbAABB(Auto.getInstance().orientedBB(), _mesh.BoundingBox))
            {
                _collisionFound = true;
            }


        }


        public virtual void render()
        {
            _mesh.updateAnimation();
            Vector3 lastpos = new Vector3();
            _mesh.getPosition(lastpos);
            if (_collisionFound)
            {
                this.posicionar(lastpos);
            }
            _mesh.render();
            if ((bool)GuiController.Instance.Modifiers.getValue("showBoundingBox"))
            {
                _mesh.BoundingBox.render();
            }

        }

        public virtual void dispose()
        {
            _mesh.dispose();

        }

    }
}
