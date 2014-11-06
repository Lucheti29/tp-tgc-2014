using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX.DirectInput;
using AlumnoEjemplos.MiGrupo.Entities;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.Sound;
using System.Windows.Forms;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.MiGrupo
{
    public class EjemploAlumno : TgcExample
    {
        TgcScene taxiScene;
        TgcScene ciudadScene;
        EnviromentMap envMap;

        ParticulasChoque choque;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Grupo LOS_BARTO";
        }

        public override string getDescription()
        {
            return "CrazyTaxi - Un taxi que debe llevar pasajeros de un punto de la ciudad a otro en un tiempo establecido al menos 5 veces. Teclas W/A/S/D/Espacio";
        }

        public override void init()
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Carpeta de archivos Media del alumno

            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;
            //primero cargamos una escena 3D entera.
            TgcSceneLoader loader = new TgcSceneLoader();

            //Luego cargamos otro modelo aparte que va a hacer el taxi
            taxiScene = loader.loadSceneFromFile(alumnoMediaFolder + "LOS_BARTO\\taxi\\taxi-TgcScene.xml");
            //Cargamos la ciudad
            ciudadScene = loader.loadSceneFromFile(alumnoMediaFolder + "LOS_BARTO\\ciudad\\ciudad2-TgcScene.xml");

            //solo nos interesa el taxi
            Auto.getInstance().inicializar(taxiScene.Meshes[0]);
            foreach (TgcMesh mesh in ciudadScene.Meshes){
                 Auto.getInstance().getObstaculos().Add(TgcObb.computeFromAABB(mesh.BoundingBox));
            }
        
            Flecha.getInstance().inicializar();
            Flecha.getInstance().hide();

            Camara.inicializar(Auto.getInstance().getPosicion());
            Camara.getObstaculos().AddRange(ciudadScene.Meshes);
            //Modifiers para modificar propiedades de la camara. pueden servir para ajustar el env map
            GuiController.Instance.Modifiers.addFloat("offsetHeight", 0, 300, 160);
            GuiController.Instance.Modifiers.addFloat("offsetForward", -400, 400, -300);
            GuiController.Instance.Modifiers.addVertex3f("displacement", new Vector3(0, 0, 0), new Vector3(100, 200, 500), new Vector3(0, 100, 300));

            EntitiesControl.getInstance().inicializar();

            GuiController.Instance.UserVars.addVar("velocidad");
            
            //Modifier para habilitar o no el renderizado del BoundingBox del personaje
            GuiController.Instance.Modifiers.addBoolean("showBoundingBox", "Bouding Box", false);

            //Crear Grid Regular
            Render.createGrid(ciudadScene, false);

            //Inicializacion de Shader
            envMap = new EnviromentMap();

            Skybox.inicializar();
        }

        public override void render(float elapsedTime)
        {
            //Sonido ambiente
            GuiController.Instance.Mp3Player.FileName = GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTO\\citty_ambiance.mp3";

            //Calculos de movimiento previos
            Teclado.handlear();
            Flecha.getInstance().calculate(elapsedTime);
            EntitiesControl.getInstance().calculate(elapsedTime);
            if (Auto.getInstance().checkCollision())
            {
                choque = new ParticulasChoque(Auto.getInstance().getMesh().Position + new Vector3(0, 15, 0));
            }

            Auto.getInstance().calculate(elapsedTime);

            /*TgcMp3Player player = GuiController.Instance.Mp3Player;
            TgcMp3Player.States currentState = player.getStatus();
            if (currentState == TgcMp3Player.States.Open)
            {
                //Reproducir MP3
                player.play(true);
            }
            if (currentState == TgcMp3Player.States.Stopped)
            {
                //Parar y reproducir MP3
                player.closeFile();
                player.play(true);
            }*/

            //El Shader llama al render
            envMap.calculate(elapsedTime);

            if (choque != null)
            {
                choque.render(elapsedTime);
            }
        }

        public override void close()
        {
            Auto.getInstance().getMesh().dispose();
            ciudadScene.disposeAll();
            EntitiesControl.getInstance().disposeAll();
            Flecha.getInstance().dispose();
            envMap.dispose();
            Skybox.dispose();
        }
    }
}