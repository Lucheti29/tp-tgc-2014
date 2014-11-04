using AlumnoEjemplos.MiGrupo.Optimizacion.GrillaRegular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    public class Render
    {
        private static GrillaRegular _grid;
        private static TgcScene _scene;

        public static void createGrid(TgcScene scene, bool debug)
        {
            //TODO: habria que separar los edificios del resto
            _scene = scene;
            _grid = new GrillaRegular();
            _grid.create(scene.Meshes, scene.BoundingBox);

            if(debug)
            {
                _grid.createDebugMeshes();
            }
        }

        public static void renderScene(float elapsedTime, bool cubemap)
        {//renderizo toda la escena (Autos, taxi,  pasajeros,etc)

            //Ver si hay que mostrar el BoundingBox
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            //ciudadScene.renderAll();

            //Render
            GameControl.getInstance().renderAll();
            Flecha.getInstance().render();
            _grid.render(GuiController.Instance.Frustum, false);

            //Muestra los Bounding Box de la escena (edificios)
            if (showBB)
            {
                foreach (TgcMesh mesh in _scene.Meshes)
                {
                    mesh.BoundingBox.render();
                }
            }

            if (!cubemap)
            {
                // dibujo el mesh
                Auto.getInstance().getMesh().Technique = "RenderCubeMap";
                Auto.getInstance().render();
            }
        }

        /// <summary>
        /// Sólo se renderiza lo que necesita el envMap
        /// En este caso, la ciudad
        /// </summary>
        public static void envRender()
        {
            _grid.render(GuiController.Instance.Frustum, false);
        }
    }
}
