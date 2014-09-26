using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.Entities
{
    public class Velocity
    {
        const float MAX_SPEED = 50f;
        const float MIN_SPEED = -20f;

        float amount = 0;

        public void acelerar()
        {
            amount += 0.2f;
        }

        public void desacelerar()
        {
            //Está frenando
            //Es más violento
            if (amount > 0)
            {
                amount -= 0.5f;
            }
            //Esta yendo marcha atrás
            //Es más suave
            else if (amount <= 0)
            {
                amount -= 0.1f;
            }
        }

        public void friccion()
        {
            //Desaceleración por fricción con el piso
            if (amount > 0)
            {
                amount -= 0.025f;
            }
            else if (amount < 0)
            {
                amount += 0.025f;
            }
        }

        public float getAmount()
        {
            if (amount > MAX_SPEED)
            {
                amount = MAX_SPEED;
            }
            else if (amount < MIN_SPEED)
            {
                amount = MIN_SPEED;
            }

            return amount;
        }
    }
}
