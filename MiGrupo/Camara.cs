using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo
{
    public class Camara
    {
        public static void inicializar(Vector3 posicion)
        {
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.setCamera(posicion, 130, 300);
        }

        public static void rotar(float angle)
        {
            GuiController.Instance.ThirdPersonCamera.rotateY(angle);
        }

        public static void setearPosicion(Vector3 posicion)
        {
            GuiController.Instance.ThirdPersonCamera.Target = posicion;
        }
    }
}
