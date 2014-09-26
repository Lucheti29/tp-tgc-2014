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

namespace AlumnoEjemplos.MiGrupo
{
    public class EjemploAlumno : TgcExample
    {
        //Objeto que va a hacer a modo de auto
        TgcBox box;
        TgcBox suelo;

        TgcMesh taxiMesh;
        TgcScene taxiScene;

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

            //Textura para la caja
            TgcTexture textura = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "Texturas\\baldosaFacultad.jpg");

            //Vectores posición inicial y tamaño
            Vector3 center = new Vector3(0, 1, 0);
            Vector3 size = new Vector3(5, 10, 5);

            //Seteo de la caja
            box = TgcBox.fromSize(center, size, textura);
            //Carpeta de archivos Media del alumno

            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;
            //primero cargamos una escena 3D entera.
            TgcSceneLoader loader = new TgcSceneLoader();

            //Luego cargamos otro modelo aparte que va a hacer el taxi
            taxiScene = loader.loadSceneFromFile(alumnoMediaFolder + "LOS_BARTO\\taxi\\taxi-TgcScene.xml");

            //solo nos interesa el taxi
            taxiMesh = taxiScene.Meshes[0];

            //Cámara en 3era persona que sigue al auto
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            //GuiController.Instance.ThirdPersonCamera.setCamera(box.Position, 70, 150);
            GuiController.Instance.ThirdPersonCamera.setCamera(taxiMesh.Position, 200, 300);

            //Crear suelo
            TgcTexture pisoTexture = TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "Texturas\\pasto.jpg");
            suelo = TgcBox.fromSize(new Vector3(500, -10, 500), new Vector3(2000, -10, 2000), pisoTexture);

            //User var
            GuiController.Instance.UserVars.addVar("velocidadAcumulada");
            GuiController.Instance.UserVars.setValue("velocidadAcumulada", new Velocity());

            GuiController.Instance.UserVars.addVar("tendenciaMovimiento");
            GuiController.Instance.UserVars.setValue("tendenciaMovimiento", new Vector3(0, 0, -1));
        }

        public override void render(float elapsedTime)
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcD3dInput input = GuiController.Instance.D3dInput;

            //Inputs
            bool right = input.keyDown(Key.Right) || input.keyDown(Key.D);
            bool left = input.keyDown(Key.Left) || input.keyDown(Key.A);
            bool up = input.keyDown(Key.Up) || input.keyDown(Key.W);
            bool down = input.keyDown(Key.Down) || input.keyDown(Key.S);

            //Getters
            Velocity velocidad = (Velocity)GuiController.Instance.UserVars.getValue("velocidadAcumulada");
            Vector3 movement = (Vector3)GuiController.Instance.UserVars.getValue("tendenciaMovimiento");
            Vector3 originalMovement = (Vector3)GuiController.Instance.UserVars.getValue("tendenciaMovimiento");

            // ---------------- Handlear Inputs ----------------
            if (left || right)
            {
                int signAngle = 1;

                if (left)
                {
                    movement = Movement.leftMove(movement);
                    signAngle *= -1;
                }
                else if (right)
                {
                    movement = Movement.rightMove(movement);
                }

                movement = Vector3.Normalize(movement);

                //Calcular ángulo entre movement original y el nuevo
                float angle = FastMath.Acos(Vector3.Dot(movement, originalMovement));

                //Rotar auto y camara en Y
                // box.rotateY(signAngle * angle);
                taxiMesh.rotateY(signAngle * angle);
                GuiController.Instance.ThirdPersonCamera.rotateY(signAngle * angle);

            }

            if (up)
            {
                velocidad.acelerar();
            }
            else if (down)
            {
                velocidad.desacelerar();
            }
            // ---------------- Fin handler Inputs ----------------

            //Siempre hay friccion
            velocidad.friccion();

            //Guardar variables
            GuiController.Instance.UserVars.setValue("velocidadAcumulada", velocidad);
            GuiController.Instance.UserVars.setValue("tendenciaMovimiento", movement);

            //Actualización de la posición de cámara
            //  GuiController.Instance.ThirdPersonCamera.Target = box.Position;
            GuiController.Instance.ThirdPersonCamera.Target = taxiMesh.Position;
            //GuiController.Instance.Logger.log("Mov " + movement.X + " " + movement.Y + " " + movement.Z);
            //GuiController.Instance.Logger.log("Pos " + box.Position.X + " " + box.Position.Y + " " + box.Position.Z);

            //Aplicar movimiento precalculado
            //  box.move(movement * velocidad.getAmount() * elapsedTime); 
            taxiMesh.move(movement * velocidad.getAmount() * elapsedTime);
            //Dibujar caja y suelo
            // box.render();
            taxiMesh.render();
            suelo.render();
        }

        public override void close()
        {
            //box.dispose();
            taxiMesh.dispose();
        }

    }
}
