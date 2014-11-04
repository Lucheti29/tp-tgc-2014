using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.Sound;

namespace AlumnoEjemplos.MiGrupo
{
    public class Sonido
    {  
        static Sonido _instance = new Sonido();
        string currentFile;
        TgcStaticSound sound;
        public void inicializar()
        {
            
        }
        /// <summary>
        /// Cargar un nuevo WAV si hubo una variacion
        /// </summary>
        public void loadSound(string filePath)
        {
            if (currentFile == null || currentFile != filePath)
            {
                currentFile = filePath;

                //Borrar sonido anterior
                if (sound != null)
                {
                    sound.dispose();
                    sound = null;
                }

                //Cargar sonido
                sound = new TgcStaticSound();
                sound.loadSound(currentFile);

                
            }
        }

        internal void play(string file, bool loop)
        {
            loadSound(file);
            sound.play(loop);
        }
        // --------------- Métodos estáticos ---------------
        public static Sonido getInstance()
        {
            if (_instance == null)
            {
                _instance = new Sonido();
            }
            return _instance;
        }

        public static void reset()
        {
            _instance = new Sonido();
        }
        // --------------- Fin Métodos estáticos ---------------

       
    }
}
