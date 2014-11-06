using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Terrain;

namespace AlumnoEjemplos.MiGrupo
{
    public class Skybox
    {
        private static TgcSkyBox skyBox;

        public static void inicializar()
        {
            //Crear SkyBox 
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(15000, 1000, 8000);

            //Configurar color
            //skyBox.Color = Color.OrangeRed;

            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTO\\skybox\\";

            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "rt.jpg");

            //Hay veces es necesario invertir las texturas Front y Back si se pasa de un sistema RightHanded a uno LeftHanded
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "ft.jpg");

            //Actualizar todos los valores para crear el SkyBox
            skyBox.updateValues();
        }

        public static void render()
        {
            skyBox.render();
        }

        //Liberar recursos del SkyBox
        public static void dispose()
        {
            skyBox.dispose();
        }
    }
}
