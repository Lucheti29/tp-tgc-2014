using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
namespace AlumnoEjemplos.MiGrupo
{
    public class Cronometro
    {

        static Cronometro _instance = new Cronometro();
        public float TiempoTotal { get; set; }
        public TgcText2d textTiempo { get; set; }
        private bool _activated = true;

        TgcSprite _spritePrimerDig = new TgcSprite();
        TgcSprite _spriteSegundoDig = new TgcSprite();
        TgcSprite _spriteDosPuntos = new TgcSprite();
        TgcSprite _spriteTercerDig = new TgcSprite();
        TgcSprite _spriteCuartoDig = new TgcSprite();

        string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

        public void inicializar()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //seteo el timer
            _instance.textTiempo = new TgcText2d();

            _instance.textTiempo.Color = Color.Red;
            _instance.textTiempo.changeFont(new System.Drawing.Font("TimesNewRoman", 25, FontStyle.Bold));

            _spritePrimerDig = new TgcSprite();
            _spriteSegundoDig = new TgcSprite();
            _spriteDosPuntos = new TgcSprite();
            _spriteTercerDig = new TgcSprite();
            _spriteCuartoDig = new TgcSprite();

            //Los dos puntos son estáticos
            _spritePrimerDig.Texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\dospuntos.png");

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = _spritePrimerDig.Texture.Size;
            _spritePrimerDig.Position = new Vector2(FastMath.Max(screenSize.Width / 2 - textureSize.Width / 2, 0), FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));
        }


        public void controlarTiempo(float elapsedTime, bool llegaronTodos)
        {
            //Renderizo el timer
            if (_activated)
            {
                if (this.TiempoTotal > 0)
                {
                    this.TiempoTotal -= elapsedTime;
                    int tiemposec = (int)this.TiempoTotal;
                    textTiempo.Text = String.Format("Tiempo Restante: {0:00}:{1:00}.", tiemposec / 60, tiemposec % 60);

                    int min, seg;

                    min = tiemposec / 60;
                    seg = tiemposec % 60;

                    if (min == 1)
                    {
                        _spritePrimerDig.Texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\uno.png");
                    }
                    else
                    {
                        _spritePrimerDig.Texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cero.png");
                    }

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

            //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
            GuiController.Instance.Drawer2D.beginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
            _spritePrimerDig.render();

            //Finalizar el dibujado de Sprites
            GuiController.Instance.Drawer2D.endDrawSprite();
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
