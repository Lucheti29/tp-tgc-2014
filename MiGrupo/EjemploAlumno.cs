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

namespace AlumnoEjemplos.MiGrupo
{
    public class EjemploAlumno : TgcExample
    {
        TgcScene taxiScene;
        TgcScene ciudadScene;

        Pasajero pas1;
        Pasajero pas2;
        Pasajero pas3;
        Pasajero pas4;
        Pasajero pas5;
        AutoComun auto1;
        List<Pasajero> listaPas;
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
            return "CrazyTaxi - Un taxi que debe llevar pasajeros de un punto de la ciudad a otro en un tiempo establecido al menos 5 veces.";
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
            ciudadScene = loader.loadSceneFromFile(alumnoMediaFolder + "LOS_BARTO\\ciudad\\ciudad-TgcScene.xml");

            TgcMesh taxiMesh = taxiScene.Meshes[0];
            //Levantamos el mesh para que no este siempre en contacto con el suelo
            taxiMesh.move(0, 15, 0);

            //solo nos interesa el taxi
            Auto.getInstance().inicializar(taxiMesh);
            Flecha.getInstance().inicializar();
            Flecha.getInstance().show();

            Camara.inicializar(Auto.getInstance().getPosicion());

            Cronometro.getInstance().TiempoTotal = 120;
            #region seteoPasajero
            pas1 = new Pasajero("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml", "SkeletalAnimations\\BasicHuman\\");
            pas1.posicionar(new Vector3(100, 5, -850));
            pas1.cargarDestino(new Vector3(700, 5, -1850));

            pas2 = new Pasajero("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml", "SkeletalAnimations\\BasicHuman\\");
            pas2.posicionar(new Vector3(-100, 5, -850));
            pas2.cargarDestino(new Vector3(-1170, 15, 703));

            pas3 = new Pasajero("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml", "SkeletalAnimations\\BasicHuman\\");
            pas3.posicionar(new Vector3(335, 15, 1193));
            pas3.cargarDestino(new Vector3(-4181, 15, -3235));

            pas4 = new Pasajero("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml", "SkeletalAnimations\\BasicHuman\\");
            pas4.posicionar(new Vector3(-6738, 15, -1069));
            pas4.cargarDestino(new Vector3(-6746, 15, 1171));

            pas5 = new Pasajero("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml", "SkeletalAnimations\\BasicHuman\\");
            pas5.posicionar(new Vector3(-1990, 15, -3174));
            pas5.cargarDestino(new Vector3(849, 15, 1160));

            listaPas = new List<Pasajero>() { pas1, pas2, pas3, pas4, pas5 };

            GuiController.Instance.UserVars.addVar("posPas");
            GuiController.Instance.UserVars.addVar("posTaxi", Auto.getInstance().getMesh().Position);
            GuiController.Instance.UserVars.addVar("posDest");
            GuiController.Instance.UserVars.addVar("velocidad");
            GuiController.Instance.UserVars.addVar("DistTaxi");
            GuiController.Instance.UserVars.addVar("distDest");
            GuiController.Instance.UserVars.addVar("ptosRec");// punto en el q esta el recorrido del auto
            GuiController.Instance.UserVars.addVar("DistpRec");

            GuiController.Instance.UserVars.addVar("Minuto"); 
            GuiController.Instance.UserVars.addVar("Segundo");

            GuiController.Instance.UserVars.addVar("MinutoUno");
            GuiController.Instance.UserVars.addVar("MinutoDos");
            GuiController.Instance.UserVars.addVar("SegundoUno");
            GuiController.Instance.UserVars.addVar("SegundoDos");
            #endregion seteoPasajero
            //Modifier para habilitar o no el renderizado del BoundingBox del personaje
            GuiController.Instance.Modifiers.addBoolean("showBoundingBox", "Bouding Box", false);

            string ejemploMediaFolder = GuiController.Instance.ExamplesMediaDir;
            auto1 = new AutoComun(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml");
            auto1.setPosition(new Vector3(910, 15, -689));

            auto1.setRecorrido(new List<Vector3>() { new Vector3(970, 15, -2848), new Vector3(713, 15, -2928), new Vector3(-778, 15, -2924), new Vector3(-819, 15, -2918), new Vector3(-883, 15, -2844), new Vector3(-899, 15, -2778), new Vector3(-922, 15, -1384), new Vector3(-922, 15, -1245), new Vector3(-919, 15, -1211), new Vector3(-902, 15, -1159), new Vector3(-873, 15, -1111), new Vector3(-762, 15, -1086), new Vector3(650, 15, -1069), new Vector3(816, 15, -1065), new Vector3(913, 15, -1131), new Vector3(939, 15, -1266) });
        }

        public override void render(float elapsedTime)
        {
            //Ver si hay que mostrar el BoundingBox
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");

            Teclado.handlear();
            Auto.getInstance().checkCollision(ciudadScene);
            Auto.getInstance().render(elapsedTime);
            Flecha.getInstance().render(elapsedTime);

            ciudadScene.renderAll();

            Cronometro.getInstance().controlarTiempo(elapsedTime, listaPas.TrueForAll(llegaronTodos));
            Cronometro.getInstance().render();

            auto1.render();
            auto1.move(elapsedTime);

            foreach (Pasajero pas in listaPas)
            {
                GuiController.Instance.UserVars.setValue("posPas", pas.posicion);
                GuiController.Instance.UserVars.setValue("posTaxi", Auto.getInstance().getMesh().Position);
                pas.move(elapsedTime);
                pas.render();
            }
        }



        private bool llegaronTodos(Pasajero obj)
        {
            return obj.llego == true;
        }


        public override void close()
        {
            Auto.getInstance().getMesh().dispose();
            ciudadScene.disposeAll();
            foreach (Pasajero pas in listaPas)
            {
                pas.dispose();
            }
            auto1.dispose();
        }
    }
}