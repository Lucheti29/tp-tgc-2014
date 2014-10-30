using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.MiGrupo.Entities
{
    public class AutoInfo
    {
        private AutoComun _auto;
        private Vector3 _posicion;
        private List<Vector3> _recorrido;

        public AutoInfo(string archXML, Vector3 posicion, List<Vector3> recorrido)
        {
            _auto = new AutoComun(archXML);
            _posicion = posicion;
            _recorrido = recorrido;
        }

        public AutoComun getAuto()
        {
            return _auto;
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
