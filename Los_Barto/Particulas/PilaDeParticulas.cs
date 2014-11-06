using System;

namespace AlumnoEjemplos.Los_Barto.Particulas
{
    class PilaDeParticulas
    {
        private Particula[] pila = null;
		private int i_cima;

        public PilaDeParticulas(int iMax)
		{
			pila = new Particula[iMax];
			i_cima = 0;
		}
	
		public bool insertar(Particula p)
		{
            //Esta llena la pila.
			if (i_cima == pila.Length)
				return false;

			pila[i_cima] = p;
			i_cima++;

			return true;
		}

		public bool sacar(out Particula p)
		{
			if (i_cima == 0)
			{
				p = null;
				return false;
			}

			i_cima--;
            p = pila[i_cima];

			return true;
		}
    }
}
