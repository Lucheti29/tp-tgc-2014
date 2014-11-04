using AlumnoEjemplos.MiGrupo.Optimizacion.Quadtree;
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
        private static Quadtree _quadtree;
        private static TgcScene _scene;

        public static void createGrid(TgcScene scene, bool debug)
        {
            _scene = scene;

            _quadtree = new Quadtree();
            _quadtree.create(scene.Meshes, scene.BoundingBox);

            if(debug)
            {
                _quadtree.createDebugQuadtreeMeshes();
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
            _quadtree.render(GuiController.Instance.Frustum, false);

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
            _quadtree.render(GuiController.Instance.Frustum, false);
        }
    }
}
