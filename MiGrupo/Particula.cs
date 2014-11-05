using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    public class Particula
    {
        static float _velocidad;
        static Vector3 _direccion;
        static TgcBox _mesh;

        public static void crear(Vector3 posicion)
        {
            Vector3 size = new Vector3(5, 10, 5);
            Color color = Color.Green;
            _mesh = TgcBox.fromSize(posicion, size, color);

            _velocidad = 10f;
            _direccion = new Vector3(0,1,0);
        }

        public void actualizar()
        {

        }

        public static void render()
        {
            _mesh.render();
        }
    }
}
