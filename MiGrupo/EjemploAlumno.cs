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
        }

        public override void render(float elapsedTime)
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcD3dInput input = GuiController.Instance.D3dInput;

            //Vector movimiento que se inicializa en cada loop
            Vector3 movement = new Vector3(0, 0, 0);

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
                //TODO: Acelerar
            }
            if (input.keyDown(Key.Down) || input.keyDown(Key.S))
            {
                //TODO: Frenar
            }

            //Loggear por consola del Framework
            GuiController.Instance.Logger.log("Ejemplo de log");

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
