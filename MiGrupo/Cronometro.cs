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
        private bool _activated = true;

        public void controlarTiempo(float elapsedTime, bool llegaronTodos)
        {
            //Renderizo el timer
            if (_activated)
            {
                if (this.TiempoTotal > 0)
                {
                    this.TiempoTotal -= elapsedTime;
                    int tiemposec = (int)this.TiempoTotal;

                    int min, minDecena, minUnidad, seg, segDecimo, segCentesimo;

                    min = tiemposec / 60;
                    seg = tiemposec % 60;

                    minDecena = min / 10;
                    minUnidad = min % 10;
                    segDecimo = seg / 10;
                    segCentesimo = seg % 10;

                    Sprites.getInstance().setTextureNumbers(minDecena, minUnidad, segDecimo, segCentesimo);

                    if (llegaronTodos)
                    {
                        //Gano
                        _activated = false;
                        Sprites.getInstance().setTextureWinOrLose(true);
                    }
                }
                else
                {
                    //Perdio
                    _activated = false;
                    Sprites.getInstance().setTextureWinOrLose(false);
                }
            }
        }

        public void incrementar(int p)
        {
            this.TiempoTotal += 10;
        }

        public void render()
        {
            Sprites.getInstance().render(_activated);
        }

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

        /// <summary>
        /// Clase Sprites:
        /// Gestiona la posición en pantalla y las texturas
        /// de los Sprites del cronómetro y los mensajes de
        /// ganaste o perdiste
        /// </summary>

        public class Sprites
        {
            private static Sprites _instance;

            private TgcSprite _minDecenaSprite = new TgcSprite();
            private TgcSprite _minUnidadSprite = new TgcSprite();
            private TgcSprite _dosPuntosSprite = new TgcSprite();
            private TgcSprite _segDecimoSprite = new TgcSprite();
            private TgcSprite _segCentesimoSprite = new TgcSprite();

            private TgcSprite _winOrLose = new TgcSprite();

            private TgcTexture[] _numberTextures = new TgcTexture[10];
            private TgcTexture _win;
            private TgcTexture _lose;

            public Sprites()
            {
                string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;
                Size screenSize = GuiController.Instance.Panel3d.Size;

                _dosPuntosSprite.Texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\dospuntos.png");

                //Se crean una sola vez las texturas
                _numberTextures[0] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cero.png");
                _numberTextures[1] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\uno.png");
                _numberTextures[2] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\dos.png");
                _numberTextures[3] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\tres.png");
                _numberTextures[4] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cuatro.png");
                _numberTextures[5] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cinco.png");
                _numberTextures[6] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\seis.png");
                _numberTextures[7] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\siete.png");
                _numberTextures[8] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\ocho.png");
                _numberTextures[9] = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\nueve.png");

                _win = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\win.png");
                _lose = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\lose.png");

                //Para evitar NPEs
                this.setTextureNumbers(0, 0, 0, 0);
                _winOrLose.Texture = _win;
                this.setPosition();
            }

            public void setTextureNumbers(int minDecena, int minUnidad, int segDecimo, int segCentesimo)
            {
                _minDecenaSprite.Texture = selectTexture(minDecena);
                _minUnidadSprite.Texture = selectTexture(minUnidad);
                _segDecimoSprite.Texture = selectTexture(segDecimo);
                _segCentesimoSprite.Texture = selectTexture(segCentesimo);
            }

            public void setTextureWinOrLose(bool result)
            {
                if (result)
                {
                    _winOrLose.Texture = _win;
                }
                else
                {
                    _winOrLose.Texture = _lose;
                }
            }

            private void setPosition()
            {
                Size screenSize = GuiController.Instance.Panel3d.Size;
                Size textureSizeDosPuntos = _dosPuntosSprite.Texture.Size;
                //Todas las texturas tienen el mismo tamaño
                Size textureSizeDefault = _minDecenaSprite.Texture.Size;

                _minDecenaSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) - (2 * textureSizeDosPuntos.Width) - textureSizeDosPuntos.Width / 4, 0), FastMath.Max((-screenSize.Height - 15) - textureSizeDosPuntos.Height / 8, 0));
                _minUnidadSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) - textureSizeDosPuntos.Width - textureSizeDosPuntos.Width / 4, 0), FastMath.Max((-screenSize.Height - 15) - textureSizeDosPuntos.Height / 8, 0));
                _dosPuntosSprite.Position = new Vector2(FastMath.Max(screenSize.Width / 2 - textureSizeDosPuntos.Width / 4, 0), FastMath.Max((-screenSize.Height - 15) - textureSizeDosPuntos.Height / 8, 0));
                _segDecimoSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) + textureSizeDosPuntos.Width, 0), FastMath.Max((-screenSize.Height - 15) - textureSizeDosPuntos.Height / 8, 0));
                _segCentesimoSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) + (2 * textureSizeDosPuntos.Width), 0), FastMath.Max((-screenSize.Height - 15) - textureSizeDosPuntos.Height / 8, 0));

                _winOrLose.Position = new Vector2(screenSize.Width / 2 - _winOrLose.Texture.Size.Width / 2, screenSize.Height / 2 - _winOrLose.Texture.Size.Height / 2);
            }

            public TgcTexture selectTexture(int num)
            {
                TgcTexture texture = _numberTextures[0];

                switch(num)
                {
                    case 0:
                        texture = _numberTextures[0];
                        break;
                    case 1:
                        texture = _numberTextures[1];
                        break;
                    case 2:
                        texture = _numberTextures[2];
                        break;
                    case 3:
                        texture = _numberTextures[3];
                        break;
                    case 4:
                        texture = _numberTextures[4];
                        break;
                    case 5:
                        texture = _numberTextures[5];
                        break;
                    case 6:
                        texture = _numberTextures[6];
                        break;
                    case 7:
                        texture = _numberTextures[7];
                        break;
                    case 8:
                        texture = _numberTextures[8];
                        break;
                    case 9:
                        texture = _numberTextures[9];
                        break;
                }

                return texture;
            }

            public void render(bool activated)
            {
                GuiController.Instance.Drawer2D.beginDrawSprite();

                if (activated)
                {
                    _minDecenaSprite.render();
                    _minUnidadSprite.render();
                    _dosPuntosSprite.render();
                    _segDecimoSprite.render();
                    _segCentesimoSprite.render();
                }
                else
                {
                    _winOrLose.render();
                }

                GuiController.Instance.Drawer2D.endDrawSprite();
            }

            public static Sprites getInstance()
            {
                if (_instance == null)
                {
                    _instance = new Sprites();
                }

                return _instance;
            }
        }
    }

}
