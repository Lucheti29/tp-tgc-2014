using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Input;

namespace AlumnoEjemplos.MiGrupo
{
    public class Teclado
    {
        Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
        static TgcD3dInput input = GuiController.Instance.D3dInput;

        public static void handlear()
        {
            //Inputs
            bool right = input.keyDown(Key.Right) || input.keyDown(Key.D);
            bool left = input.keyDown(Key.Left) || input.keyDown(Key.A);
            bool up = input.keyDown(Key.Up) || input.keyDown(Key.W);
            bool down = input.keyDown(Key.Down) || input.keyDown(Key.S);
            bool brake = input.keyDown(Key.Space);

            Auto.getInstance().aplicarMovimiento(right, left, up, down, brake);
        }
    }
}
