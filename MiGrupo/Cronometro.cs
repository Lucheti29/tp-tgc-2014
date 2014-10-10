using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer.Utils._2D;
namespace AlumnoEjemplos.MiGrupo
{
    public class Cronometro
    {

        static Cronometro _instance = new Cronometro();
        public float TiempoTotal { get; set; }
        public TgcText2d textTiempo { get; set; }
        private bool _activated = true;

        public void inicializar()
        {
            //seteo el timer
            _instance.textTiempo = new TgcText2d();

            _instance.textTiempo.Color = Color.Red;
            _instance.textTiempo.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));
        }


        public void controlarTiempo(float elapsedTime, bool llegaronTodos)
        {
            //Renderizo el timer
            if (_activated)
            {
                if (this.TiempoTotal > 0)
                {
                    this.TiempoTotal -= elapsedTime;
                    //tiempo -= elapsedTime;
                    int tiemposec = (int)this.TiempoTotal;
                    textTiempo.Text = String.Format("Tiempo Restante: {0:00}:{1:00}.", tiemposec / 60, tiemposec % 60);
                    if (llegaronTodos)
                    {
                        textTiempo.Text = String.Format("FELICITACIONES!! GANASTEEEE!!!!");
                        _activated = false;
                    }
                }
                else
                {
                    textTiempo.Text = String.Format("Tiempo Concluido!!! PERDISTEEEE");
                    _activated = false;
                }
            }
        }
        public void render()
        {
            textTiempo.render();
        }
        // --------------- Métodos estáticos ---------------
        public static Cronometro getInstance()
        {
            if (_instance == null)
            {
                _instance = new Cronometro();
            }
            return _instance;
        }

        public static void reset()
        {
            _instance = new Cronometro();
        }
        // --------------- Fin Métodos estáticos -------------

        public void incrementar(int p)
        {
            this.TiempoTotal += 10;
        }
    }

}
