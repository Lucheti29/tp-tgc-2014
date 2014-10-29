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

        string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

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

                    GuiController.Instance.UserVars.setValue("Minuto", min);
                    GuiController.Instance.UserVars.setValue("Segundo", seg);

                    GuiController.Instance.UserVars.setValue("MinutoUno", minDecena);
                    GuiController.Instance.UserVars.setValue("MinutoDos", minUnidad);

                    GuiController.Instance.UserVars.setValue("SegundoUno", segDecimo);
                    GuiController.Instance.UserVars.setValue("SegundoDos", segCentesimo);

                    Sprites.getInstance().setTexture(selectTexture(minDecena), selectTexture(minUnidad), selectTexture(segDecimo), selectTexture(segCentesimo));

                    if (llegaronTodos)
                    {
                        //Gano
                        _activated = false;
                    }
                }
                else
                {
                    //Perdio
                    _activated = false;
                }
            }
        }

        public TgcTexture selectTexture(int num)
        {
            TgcTexture texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cero.png");

            switch(num)
            {
                case 0:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cero.png");
                    break;
                case 1:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\uno.png");
                    break;
                case 2:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\dos.png");
                    break;
                case 3:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\tres.png");
                    break;
                case 4:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cuatro.png");
                    break;
                case 5:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\cinco.png");
                    break;
                case 6:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\seis.png");
                    break;
                case 7:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\siete.png");
                    break;
                case 8:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\ocho.png");
                    break;
                case 9:
                    texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\nueve.png");
                    break;
            }

            return texture;
        }

        public void incrementar(int p)
        {
            this.TiempoTotal += 10;
        }

        public void render()
        {
            Sprites.getInstance().render();
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

        public class Sprites
        {
            private static Sprites _instance;

            private TgcSprite _minDecenaSprite = new TgcSprite();
            private TgcSprite _minUnidadSprite = new TgcSprite();
            private TgcSprite _dosPuntosSprite = new TgcSprite();
            private TgcSprite _segDecimoSprite = new TgcSprite();
            private TgcSprite _segCentesimoSprite = new TgcSprite();

            public Sprites()
            {
                string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;
                Size screenSize = GuiController.Instance.Panel3d.Size;

                _dosPuntosSprite.Texture = TgcTexture.createTexture(alumnoMediaFolder + "LOS_BARTO\\cronometro\\dospuntos.png");

                this.setPosition();
            }

            public void setTexture(TgcTexture minDecena, TgcTexture minUnidad, TgcTexture segDecimo, TgcTexture segCentesimo)
            {
                _minDecenaSprite.Texture = minDecena;
                _minUnidadSprite.Texture = minUnidad;
                _segDecimoSprite.Texture = segDecimo;
                _segCentesimoSprite.Texture = segCentesimo;
            }

            private void setPosition()
            {
                Size screenSize = GuiController.Instance.Panel3d.Size;

                //Todas las texturas tienen el mismo tamaño
                Size textureSize = _dosPuntosSprite.Texture.Size;

                _minDecenaSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) - (2 * textureSize.Width) - 10, 0), FastMath.Max((-screenSize.Height - 10) - textureSize.Height / 8, 0));
                _minUnidadSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) - textureSize.Width - 10, 0), FastMath.Max((-screenSize.Height - 10) - textureSize.Height / 8, 0));
                _dosPuntosSprite.Position = new Vector2(FastMath.Max(screenSize.Width / 2 - textureSize.Width / 2, 0), FastMath.Max((-screenSize.Height - 10) - textureSize.Height / 8, 0));
                _segDecimoSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) + textureSize.Width - 10, 0), FastMath.Max((-screenSize.Height - 10) - textureSize.Height / 8, 0));
                _segCentesimoSprite.Position = new Vector2(FastMath.Max((screenSize.Width / 2) + (2 * textureSize.Width) - 10, 0), FastMath.Max((-screenSize.Height - 10) - textureSize.Height / 8, 0));
            }

            public void render()
            {
                GuiController.Instance.Drawer2D.beginDrawSprite();

                _minDecenaSprite.render();
                _minUnidadSprite.render();
                _dosPuntosSprite.render();
                _segDecimoSprite.render();
                _segCentesimoSprite.render();

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
