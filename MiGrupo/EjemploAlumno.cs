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
        float tiempoRestante = 60;
        TgcScene ciudadScene;
        TgcBox box;
        Pasajero pas;

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

            pas = new Pasajero();
            Vector3 pos = new Vector3(100, 5, -200);

            pas.parar(pos);
            pas.destino = new Vector3(100, 5, -450);


            Vector3 size = new Vector3(50, 70, 80);
            Vector3 center = pas.destino;

            Color color = Color.Gray;

            box = TgcBox.fromSize(center, size);
        }

        public override void render(float elapsedTime)
        {
            Auto.getInstance().checkCollision(ciudadScene);
            Auto.getInstance().render(elapsedTime);

            ciudadScene.renderAll();

            float distancia = Vector3.Length(Auto.getInstance().getPosicion() - pas.posicion);


            tiempoRestante = Cronometro.getInstance().controlarTiempo(tiempoRestante, elapsedTime);
            Cronometro.getInstance().render();

            /*     if (pas.getDistancia(Auto.getInstance().getPosicion().X,Auto.getInstance().getPosicion().Z)< 100)
                     {
                
                         pas.caminarHacia(Auto.getInstance().getPosicion());
                     }*/
            if (distancia < 200)
            {
                pas.movePasajero(elapsedTime, Auto.getInstance());
            }
            float distanciaDestino = Vector3.Length(Auto.getInstance().getPosicion() - pas.destino);
            if (distanciaDestino < 100)
            {
                pas.bajarseDelTaxi(Auto.getInstance());
                pas.getMesh().Enabled = true;
            }
            pas.render();
            box.render();
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