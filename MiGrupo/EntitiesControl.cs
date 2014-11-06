using AlumnoEjemplos.MiGrupo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.MiGrupo
{
    public class EntitiesControl
    {
        /// <summary>
        /// GameControl: se encarga de instanciar
        /// controlar y eliminar todas las entidades
        /// (autos, pasajeros, peatones)
        /// </summary>

        private static EntitiesControl _instance;

        private List<Pasajero> _listaPas = new List<Pasajero>();
        private List<AutoComun> _listaAutoComun = new List<AutoComun>();
        private List<Peaton> _listaPeatones = new List<Peaton>();

        public void inicializar()
        {
            Config.load();

            foreach (PasajeroInfo pas in Config.getPasajeros())
            {
                pas.getPasajero().posicionar(pas.getPosicion());
                Auto.getInstance()._pasajeros.Add(pas.getPosicion());
                pas.getPasajero().cargarDestino(pas.getDestino());

                _listaPas.Add(pas.getPasajero());

            }
            foreach (PeatonInfo peaton in Config.getPeatones())
            {
                peaton.getPeaton().posicionar(peaton.getPosicion());
                peaton.getPeaton().setRecorrido(peaton.getRecorrido());    

                _listaPeatones.Add(peaton.getPeaton());

            }
            foreach (AutoInfo auto in Config.getAutos())
            {
                auto.getAuto().setPosition(auto.getPosicion());
                auto.getAuto().setRecorrido(auto.getRecorrido());
               
                _listaAutoComun.Add(auto.getAuto());
                Auto.getInstance().getObstaculos().Add(auto.getAuto().getOBB());
            }
            
            Cronometro.getInstance().TiempoTotal = 120;
        }

        public List<Pasajero> getListaPasajeros()
        {
            return _listaPas;
        }
        public List<Peaton> getListaPeatones()
        {
            return _listaPeatones;
        }
        public List<AutoComun> getListaAutosComunes()
        {
            return _listaAutoComun;
        }

        public void calculate(float elapsedTime)
        {
            Cronometro.getInstance().controlarTiempo(elapsedTime, EntitiesControl.getInstance().getListaPasajeros().TrueForAll(llego));

            foreach (AutoComun auto in _listaAutoComun)
            {
                auto.calculate(elapsedTime);
            }
            
            foreach ( Peaton peaton in _listaPeatones)
            {
                peaton.move(elapsedTime);
            }

            foreach (Pasajero pas in _listaPas)
            {
                pas.move(elapsedTime);
            }
        }

        public void renderAll()
        {
            Cronometro.getInstance().render();

            foreach (AutoComun auto in _listaAutoComun)
            {
                auto.checkCollision();
                auto.render();
            }
            foreach (Peaton peaton in _listaPeatones)
            {
                peaton.checkCollision();
                peaton.render();
            }

            foreach (Pasajero pas in _listaPas)
            {
                pas.checkCollision();
                pas.render();
            }
        }

        public void disposeAll()
        {
            foreach (AutoComun auto in _listaAutoComun)
            {
                auto.dispose();
            }

            foreach (Pasajero pas in _listaPas)
            {
                pas.dispose();
            }
        }

        private bool llego(Pasajero pas)
        {
            return pas.llego == true;
        }

        public static EntitiesControl getInstance()
        {
            if (_instance == null)
            {
                _instance = new EntitiesControl();
            }

            return _instance;
        }
    }
}
