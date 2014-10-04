using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.Entities
{
    public class Velocity
    {
        const float MAX_SPEED = 500f;
        const float MIN_SPEED = -200f;
        const float ACCELERATION = 0.5f;

        float _amount = 0;

        public void acelerar()
        {
            _amount += ACCELERATION;
        }

        public void desacelerar()
        {
            //Está frenando
            //Es más violento
            if (_amount > 0)
            {
                _amount -= 0.5f;
            }
            //Esta yendo marcha atrás
            //Es más suave
            else if (_amount <= 0)
            {
                _amount -= 0.1f;
            }
        }

        public void frenar()
        {
            //Está frenando
            //Es más violento
            if (_amount > 0)
            {
                _amount -= 0.2f;
            }
        }

        public void friccion()
        {
            //Desaceleración por fricción con el piso
            if (_amount > 0)
            {
                _amount -= 0.025f;
            }
            else if (_amount < 0)
            {
                _amount += 0.025f;
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

        public void setAmount(float amount)
        {
            _amount = amount;
        }
    }
}
