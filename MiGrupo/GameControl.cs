using AlumnoEjemplos.MiGrupo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo
{
    public class GameControl
    {
        /// <summary>
        /// GameControl: (por el momento) se encarga de
        /// instanciar, controlar y eliminar todas las
        /// entidades (autos, pasajeros, peatones)
        /// </summary>

        private static GameControl _instance;

        private static int VIEW_DISTANCE = 1250;

        private List<Pasajero> _listaPas = new List<Pasajero>();
        private List<AutoComun> _listaAutoComun = new List<AutoComun>();

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

            foreach (AutoInfo auto in Config.getAutos())
            {
                auto.getAuto().setPosition(auto.getPosicion());
                auto.getAuto().setRecorrido(auto.getRecorrido());

                _listaAutoComun.Add(auto.getAuto());
            }

            Cronometro.getInstance().TiempoTotal = 120;
        }

        public List<Pasajero> getListaPasajeros()
        {
            return _listaPas;
        }

        public List<AutoComun> getListaAutosComunes()
        {
            return _listaAutoComun;
        }

        public void render(float elapsedTime)
        {
            Cronometro.getInstance().controlarTiempo(elapsedTime, GameControl.getInstance().getListaPasajeros().TrueForAll(llego));
            Cronometro.getInstance().render();

            foreach (AutoComun auto in _listaAutoComun)
            {
                //TODO ver si se quiere q se mueva el auto mientras no se esta viendo! 

                if (Utils.getDistance(auto.getPosition().X, auto.getPosition().Z, Auto.getInstance().getPosicion().X, Auto.getInstance().getPosicion().Z) < VIEW_DISTANCE)
                {
                    // auto.checkCollision();
                    auto.render(elapsedTime);
                }
            }

            foreach (Pasajero pas in _listaPas)
            {
                GuiController.Instance.UserVars.setValue("posPas", pas.posicion);
                GuiController.Instance.UserVars.setValue("posTaxi", Auto.getInstance().getMesh().Position);
                pas.move(elapsedTime);

                if (Utils.getDistance(pas.posicion.X, pas.posicion.Z, Auto.getInstance().getPosicion().X, Auto.getInstance().getPosicion().Z) < VIEW_DISTANCE)
                {
                    pas.render();
                }
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

        public static GameControl getInstance()
        {
            if (_instance == null)
            {
                _instance = new GameControl();
            }

            return _instance;
        }
    }
}
