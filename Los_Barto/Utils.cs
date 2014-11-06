using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.Los_Barto
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
        
        
 
        /// <summary>      
        /// Retorna el vector movimiento para acercarse a un punto,con una velocidad y con un angulo dados  
        /// </summary>
        public static Vector3 movementVector(float velocidad,float angulo)
        {
            return new Vector3(FastMath.Cos(angulo) * velocidad, 0, FastMath.Sin(angulo) * velocidad);

        }

        /// <summary>
        /// Retorna una matriz de transformación que tiene
        /// la rotación aplicada
        /// </summary>
        public static Vector3 doblar(Vector3 movement, float elapsedTime, int angle)
        {
            float yaw = FastMath.ToRad(angle) * elapsedTime;
            Matrix rotation = Matrix.RotationYawPitchRoll(yaw, 0, 0);

            Vector4 transformedVec4 = Vector3.Transform(movement, rotation);
            Vector3 transformedVec3 = new Vector3(transformedVec4.X, transformedVec4.Y, transformedVec4.Z);

            return transformedVec3;
        }
    }
}
