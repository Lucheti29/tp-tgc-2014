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

       public void inicializar()
       {
           //seteo el timer
           _instance.textTiempo = new TgcText2d();
          // _instance.textTiempo.Text = "Tiempo Restante: ";
           _instance.textTiempo.Color = Color.Red;
           _instance.textTiempo.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));
       }
 

       public float controlarTiempo(float tiempo, float elapsedTime)
       {
                 //Renderizo el timer
            if (tiempo > 0)
            {
                tiempo-= elapsedTime;
                int tiemposec = (int)tiempo;
                textTiempo.Text = String.Format("Tiempo Restante: {0:00}:{1:00}.", tiemposec / 60, tiemposec % 60);

            }
            else
            {
                textTiempo.Text = String.Format("Tiempo Concluido!!!");

            }
            return tiempo;
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
   }

}
