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

namespace AlumnoEjemplos.MiGrupo
{
    public class EjemploAlumno : TgcExample
    {
        //Objeto que va a hacer a modo de auto
        TgcBox box;

        const float MAX_SPEED = 50f;
        const float MIN_SPEED = -20f;

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
            //Textura para la caja
            TgcTexture textura = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "Texturas\\baldosaFacultad.jpg");

            //Vectores posición inicial y tamaño
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 size = new Vector3(5, 10, 5);

            //Seteo de la caja
            box = TgcBox.fromSize(center, size, textura);

            //Seteo de cámara
            GuiController.Instance.RotCamera.targetObject(box.BoundingBox);

            //User var
            GuiController.Instance.UserVars.addVar("velocidadAcumulada");
            GuiController.Instance.UserVars.setValue("velocidadAcumulada", 0f);
        }

        public override void render(float elapsedTime)
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcD3dInput input = GuiController.Instance.D3dInput;

            //Vector movimiento que se inicializa en cada loop
            Vector3 movement = new Vector3(0, 0, 0);
            
            //Get velocidad
            float velocidad = (float)GuiController.Instance.UserVars.getValue("velocidadAcumulada");

            movement.Z = 1;

            if (input.keyDown(Key.Left) || input.keyDown(Key.A))
            {
                //TODO: Doblar a la izquierda
            }
            if (input.keyDown(Key.Right) || input.keyDown(Key.D))
            {
                //TODO: Doblar a la derecha
            }
            if (input.keyDown(Key.Up) || input.keyDown(Key.W))
            {
                //Acelerar
                //TODO: deshardcodear aceleracion

                velocidad += 0.2f;
            }
            if (input.keyDown(Key.Down) || input.keyDown(Key.S))
            {
                //Frenar
                //TODO: deshardcodear aceleracion

                //Está frenando
                //Es más violento
                if (velocidad > 0)
                {
                    velocidad -= 0.5f;
                }
                //Esta yendo marcha atrás
                //Es más suave
                else if (velocidad <= 0)
                {
                    velocidad -= 0.1f;
                }
            }
            else
            {
                //Desaceleración por fricción con el piso
                if (velocidad > 0)
                {
                    velocidad -= 0.05f;
                }
                else if (velocidad < 0)
                {
                    velocidad += 0.05f;
                }
            }

            //Chequeo de que la velocidad este entre los límites permitidos
            if (velocidad > MAX_SPEED)
            {
                velocidad = MAX_SPEED;
            }
            else if (velocidad < MIN_SPEED)
            {
                velocidad = MIN_SPEED;
            }

            //Aplicar velocidad al vector movimiento
            movement *= velocidad * elapsedTime;

            //Guardar velocidad
            GuiController.Instance.UserVars.setValue("velocidadAcumulada", velocidad);

            //Loguear velocidad para debug
            GuiController.Instance.Logger.log("La velocidad acumulada es " + velocidad.ToString());

            //Aplicar movimiento precalculado
            box.move(movement);

            //Dibujar caja
            box.render();
        }

        public override void close()
        {
            box.dispose();
        }

    }
}
