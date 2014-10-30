using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.Entities
{
    public class Velocity
    {
        const float MAX_SPEED = 120f;
        const float MIN_SPEED = -60f;
        const float ACCELERATION = 5f;
        const float ACCELERATION_BACK = 3f;
        const float ACCELERATION_BACK_BRAKE = 9f;
        const float DESACCELERATION_BRAKE = 20f;
        const float FRICTION = 1f;

        float _amount = 0;

        public void acelerar(float currentElapsedTime)
        {
            _amount += (ACCELERATION * currentElapsedTime);
        }

        public void desacelerar(float currentElapsedTime)
        {
            //Está frenando
            //Es más violento
            if (_amount > 0)
            {
                _amount -= (ACCELERATION_BACK_BRAKE * currentElapsedTime);
            }
            //Esta yendo marcha atrás
            //Es más suave
            else if (_amount <= 0)
            {
                _amount -= (ACCELERATION_BACK * currentElapsedTime);
            }
        }

        public void frenar(float currentElapsedTime)
        {
            //Está frenando
            //Es más violento
            if (_amount > 0)
            {
                _amount -= (DESACCELERATION_BRAKE * currentElapsedTime);

                //Para que no vaya para atrás
                if (_amount < 0)
                {
                    _amount = 0;
                }
            }
            else if (_amount < 0)
            {
                _amount += (DESACCELERATION_BRAKE * currentElapsedTime);

                //Para que no vaya para adelante
                if (_amount > 0)
                {
                    _amount = 0;
                }
            }
        }

        public void friccion(float currentElapsedTime)
        {
            //Desaceleración por fricción con el piso
            if (_amount > 0)
            {
                _amount -= (FRICTION * currentElapsedTime);
            }
            else if (_amount < 0)
            {
                _amount += (FRICTION * currentElapsedTime);
            }
        }

        public float getAmount()
        {
            if (_amount > MAX_SPEED)
            {
                _amount = MAX_SPEED;
            }
            else if (_amount < MIN_SPEED)
            {
                _amount = MIN_SPEED;
            }

            return _amount;
        }

        public void setAmount(float amount, float currentElapsedTime)
        {
            _amount = (amount * currentElapsedTime);
        }

    }
}
