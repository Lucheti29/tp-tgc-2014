using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Sound;

namespace AlumnoEjemplos.Los_Barto
{
    public class Sonido
    {  
        static Sonido _instance = new Sonido();
        string currentFile;
        TgcStaticSound sound;
        TgcMp3Player player = GuiController.Instance.Mp3Player;
        public void inicializar()
        {
            
        }
        /// <summary>
        /// Cargar un nuevo WAV si hubo una variacion
        /// </summary>
        public void loadSoundWAV(string filePath)
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
            loadSoundWAV(file);
            sound.play(loop);
        }
        public void playerMp3()
        {
            TgcMp3Player.States currentState = player.getStatus();
            if (currentState == TgcMp3Player.States.Open)
            {
                //Reproducir MP3
                player.play(true);
            }
            if (currentState == TgcMp3Player.States.Stopped)
            {
                //Parar y reproducir MP3
                player.closeFile();
                player.play(true);
            }
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



        internal void playAmbiente()
        {
            GuiController.Instance.Mp3Player.FileName = GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTO\\citty_ambiance.mp3";
            playerMp3();
        }
    }
}
