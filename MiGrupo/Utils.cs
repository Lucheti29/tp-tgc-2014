using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    public class Utils
    {
        /// <summary>
        /// Retorna la distancia entre dos posiciones
        /// </summary>
        public static float getDistance(float x1, float z1, float x2, float z2)
        {
            return FastMath.Sqrt(FastMath.Pow2(x2 - x1) + FastMath.Pow2(z2 - z1));
        }

        /// <summary>
        /// Retorna el ángulo formado por ambos puntos
        /// </summary>
        public static float calculateAngle(float pos1X, float pos1Z, float pos2X, float pos2Z)
        {
            float cathetus_o;
            float cathetus_a;
            float angle = 0.0f;
            bool up = true;
            bool equal = false;

            if (pos1Z < pos2Z)
                up = true;
            else if (pos1Z > pos2Z)
                up = false;
            else
                equal = true;

            cathetus_a = pos2X - pos1X;
            cathetus_o = pos2Z - pos1Z;


            if (up)
            {

                angle = FastMath.Atan((float)(cathetus_o / cathetus_a));

                if (angle < 0.0f)
                    angle = FastMath.PI + angle;

            }
            else if (!up)
            {

                angle = FastMath.Atan((float)(cathetus_o / cathetus_a));

                if (angle >= 0.0f)
                    angle = FastMath.PI + angle;
                else
                    angle = FastMath.PI * 2 + angle;

            }

            if (equal)
            {
                if (cathetus_a > 0)
                    angle = 0.0f;
                else
                    angle = FastMath.PI;
            }

            return angle;
        }
    }
}
