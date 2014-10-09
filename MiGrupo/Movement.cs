using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    public class Movement
    {
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
