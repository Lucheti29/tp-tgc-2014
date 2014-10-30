using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.Entities
{
    public class PasajeroInfo
    {
        private Pasajero _pasajero;
        private Vector3 _posicion;
        private Vector3 _destino;

        public PasajeroInfo(string mesh, string textura, Vector3 posicion, Vector3 destino)
        {
            _pasajero = new Pasajero(mesh, textura);
            _posicion = posicion;
            _destino = destino;
        }

        public Pasajero getPasajero()
        {
            return _pasajero;
        }

        public Vector3 getPosicion()
        {
            return _posicion;
        }

        public Vector3 getDestino()
        {
            return _destino;
        }
    }
}
