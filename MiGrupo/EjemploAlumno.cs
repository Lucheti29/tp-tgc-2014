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
        TgcBox box;
        Pasajero pas;
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

            Camara.inicializar(Auto.getInstance().getPosicion());

            Cronometro.getInstance().inicializar();
            Cronometro.getInstance().TiempoTotal = 60;
            pas = new Pasajero();
            Vector3 pos = new Vector3(100, 5, -850);

            pas.posicionar(pos);
            pas.destino = new Vector3(700, 5, -850);
            listaPas = new List<Pasajero>();
            listaPas.Add(pas);

            Vector3 size = new Vector3(30, 20, 30);
            Vector3 center = pas.destino;

            Color color = Color.Gray;

            box = TgcBox.fromSize(center, size);

            GuiController.Instance.UserVars.addVar("posPas", pas.posicion);
            GuiController.Instance.UserVars.addVar("posTaxi", Auto.getInstance().getMesh().Position);
            GuiController.Instance.UserVars.addVar("posDest", pas.destino);
            GuiController.Instance.UserVars.addVar("velocidad");
            GuiController.Instance.UserVars.addVar("DistTaxi");
            GuiController.Instance.UserVars.addVar("distDest");
        }

        public override void render(float elapsedTime)
        {
             Auto.getInstance().checkCollision(ciudadScene);
            Auto.getInstance().render(elapsedTime);

            ciudadScene.renderAll();

            float distancia = Vector3.Length(Auto.getInstance().getPosicion() - pas.posicion);
            GuiController.Instance.UserVars.setValue("posPas", pas.posicion);
            GuiController.Instance.UserVars.setValue("posTaxi", Auto.getInstance().getMesh().Position);


            Cronometro.getInstance().controlarTiempo(elapsedTime, listaPas.TrueForAll(llegaronTodos));
            Cronometro.getInstance().render();


            pas.movePasajero(elapsedTime, Auto.getInstance());

            pas.render();
            box.render();
        }

        private bool llegaronTodos(Pasajero obj)
        {
            return obj.llego == true;
        }

        public override void close()
        {
            Auto.getInstance().getMesh().dispose();
            ciudadScene.disposeAll();
            pas.dispose();
            box.dispose();

        }

    }
}