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

        private static bool _right;
        private static bool _left;
        private static bool _up;
        private static bool _down;
        private static bool _space;

        public static void handlear()
        {
            //Inputs
            _right = input.keyDown(Key.Right) || input.keyDown(Key.D);
            _left = input.keyDown(Key.Left) || input.keyDown(Key.A);
            _up = input.keyDown(Key.Up) || input.keyDown(Key.W);
            _down = input.keyDown(Key.Down) || input.keyDown(Key.S);
            _space = input.keyDown(Key.Space);
        }

        public static bool getInput(InputType input)
        {
            bool result = false;

            switch(input)
            {
                case InputType.UP:
                    result = _up;
                    break;
                case InputType.DOWN:
                    result = _down;
                    break;
                case InputType.RIGHT:
                    result = _right;
                    break;
                case InputType.LEFT:
                    result = _left;
                    break;
                case InputType.SPACE:
                    result = _space;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
