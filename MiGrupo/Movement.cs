using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo
{
    public class Movement
    {
        const float sensibilidad = 0.0015f;

        public static Vector3 rightMove(Vector3 movement)
        {
            if (movement.Z > 0 && movement.X < 0)
            {
                movement.Z = movement.Z + sensibilidad;
                movement.X = movement.X + sensibilidad;
            }
            else if (movement.Z < 0 && movement.X < 0)
            {
                movement.Z = movement.Z + sensibilidad;
                movement.X = movement.X - sensibilidad;
            }
            else if (movement.Z < 0 && movement.X > 0)
            {
                movement.Z = movement.Z - sensibilidad;
                movement.X = movement.X - sensibilidad;
            }
            else if (movement.Z > 0 && movement.X > 0)
            {
                movement.Z = movement.Z - sensibilidad;
                movement.X = movement.X + sensibilidad;
            }
            else
            {
                movement.Z = movement.Z + sensibilidad;
                movement.X = movement.X - sensibilidad;
            }

            return movement;
        }

        public static Vector3 leftMove(Vector3 movement)
        {
            if (movement.Z > 0 && movement.X < 0)
            {
                movement.Z = movement.Z - sensibilidad;
                movement.X = movement.X - sensibilidad;
            }
            else if (movement.Z < 0 && movement.X < 0)
            {
                movement.Z = movement.Z - sensibilidad;
                movement.X = movement.X + sensibilidad;
            }
            else if (movement.Z < 0 && movement.X > 0)
            {
                movement.Z = movement.Z + sensibilidad;
                movement.X = movement.X + sensibilidad;
            }
            else if (movement.Z > 0 && movement.X > 0)
            {
                movement.Z = movement.Z + sensibilidad;
                movement.X = movement.X - sensibilidad;
            }
            else
            {
                movement.Z = movement.Z - sensibilidad;
                movement.X = movement.X - sensibilidad;
            }

            return movement;
        }
    }
}
