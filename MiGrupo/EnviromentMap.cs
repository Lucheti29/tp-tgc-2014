using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    public class EnviromentMap
    {
      /* aca iria toda la logica del env map*/



        static EnviromentMap _instance = new EnviromentMap();
        private Effect _effect;
        private float kc, kx;


        public void inicializar(TgcMesh taxi)
        {
            // Arreglo las normales del taxi
            {
                int[] adj = new int[Auto.getInstance().getMesh().D3dMesh.NumberFaces * 3];
                Auto.getInstance().getMesh().D3dMesh.GenerateAdjacency(0, adj);
                Auto.getInstance().getMesh().D3dMesh.ComputeNormals(adj);
            }
            //Cargar Shader personalizado
            _effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Shaders\\EnvMap.fx");

            // le asigno el efecto a la malla 
            Auto.getInstance().getMesh().Effect = _effect;
            kx = kc = 0.5f;
            GuiController.Instance.Modifiers.addFloat("Reflexion", 0, 1, kx);
            GuiController.Instance.Modifiers.addFloat("Refraccion", 0, 1, kc);
            GuiController.Instance.Modifiers.addBoolean("Fresnel", "fresnel", true);
            GuiController.Instance.Modifiers.addBoolean("EnviromentMap", "EnvMap", true);
        }

        



        // --------------- Fin de métodos de instancia ---------------


        // --------------- Métodos estáticos ---------------
        public static EnviromentMap getInstance()
        {
            if (_instance == null)
            {
                _instance = new EnviromentMap();
            }
            return _instance;
        }

        public static void reset()
        {
            _instance = new EnviromentMap();
        }
        // --------------- Fin Métodos estáticos ---------------
    }
}
