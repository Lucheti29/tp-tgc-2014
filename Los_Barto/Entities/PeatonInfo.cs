using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.Los_Barto.Entities
{
    public class PeatonInfo
    {

        private Peaton _peaton;
        private Vector3 _posicion;
        private List<Vector3> _recorrido;
        

        public PeatonInfo(string mesh, string textura, Vector3 posicion,List<Vector3> recorrido)
        {
           _peaton = new Peaton(mesh, textura);
            _posicion = posicion;
            _recorrido = recorrido;
        }

        public Peaton getPeaton()
        {
            return _peaton;
        }

        public Vector3 getPosicion()
        {
            return _posicion;
        }
        public List<Vector3> getRecorrido()
        {
            return _recorrido;
        }
    }
}


