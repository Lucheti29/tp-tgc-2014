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
        //Objeto que va a hacer a modo de auto

        TgcScene taxiScene;
        // float tiempoRestante = 60;
        TgcScene ciudadScene;

        Pasajero pas1;
        Pasajero pas2;
        Pasajero pas3;
        Pasajero pas4;
        Pasajero pas5;
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

            Cronometro.getInstance().inicializar();
            Cronometro.getInstance().TiempoTotal = 120;

            pas1 = new Pasajero();
            pas1.posicionar(new Vector3(100, 5, -850));
            pas1.cargarDestino(new Vector3(700, 5, -1850));

            pas2 = new Pasajero();
            pas2.posicionar(new Vector3(-100, 5, -850));
            pas2.cargarDestino(new Vector3(-1170, 15, 703));


            pas3 = new Pasajero();
            pas3.posicionar(new Vector3(335, 15, 1193));
            pas3.cargarDestino(new Vector3(-4181, 15, -3235));

            pas4 = new Pasajero();
            pas4.posicionar(new Vector3(-6738, 15, -1069));
            pas4.cargarDestino(new Vector3(-6746, 15, 1171));

            pas5 = new Pasajero();
            pas5.posicionar(new Vector3(-1990, 15, -3174));
            pas5.cargarDestino(new Vector3(849, 15, 1160));

            listaPas = new List<Pasajero>() { pas1, pas2, pas3, pas4, pas5 };

            GuiController.Instance.UserVars.addVar("posPas", pas1.posicion);
            GuiController.Instance.UserVars.addVar("posTaxi", Auto.getInstance().getMesh().Position);
            GuiController.Instance.UserVars.addVar("posDest", pas1.destino);
            GuiController.Instance.UserVars.addVar("velocidad");
            GuiController.Instance.UserVars.addVar("DistTaxi");
            GuiController.Instance.UserVars.addVar("distDest");
        }

        public override void render(float elapsedTime)
        {
            Teclado.handlear();
            Auto.getInstance().checkCollision(ciudadScene);
            Auto.getInstance().render(elapsedTime);
            Flecha.getInstance().render(elapsedTime);

            ciudadScene.renderAll();



            Cronometro.getInstance().controlarTiempo(elapsedTime, listaPas.TrueForAll(llegaronTodos));
            Cronometro.getInstance().render();

            foreach (Pasajero pas in listaPas)
            {

                GuiController.Instance.UserVars.setValue("posPas", pas.posicion);
                GuiController.Instance.UserVars.setValue("posTaxi", Auto.getInstance().getMesh().Position);

                pas.movePasajero(elapsedTime, Auto.getInstance());

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
            pas1.dispose();


        }

    }
}