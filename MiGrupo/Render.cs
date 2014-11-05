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
        /// <summary>
        /// Render: se encarga de determinar qué
        /// se va a renderizar en cada caso
        /// </summary>

        private static Quadtree _quadtree1; //Incluye todos los meshes de la ciudad
        private static Quadtree _quadtree2; //Incluye todos los meshes excepto la vereda y la calle (para el env map)
        private static TgcScene _scene;
        private static List<TgcMesh> _reduceMeshes;
        private static List<TgcMesh> _vereda;

        /// <summary>
        /// Crea 2 quadtree: uno con todos los modelos (174 meshes)
        /// de la ciudad y otro igual pero sin la vereda (150 meshes)
        /// (esta ultima no le sirve al EnvMap)
        /// </summary>
        public static void createGrid(TgcScene scene, bool debug)
        {
            _scene = scene;

            _scene.separeteMeshList(new string[] { "Vereda" }, out _vereda, out _reduceMeshes);

            _quadtree1 = new Quadtree();
            _quadtree1.create(_scene.Meshes, scene.BoundingBox);

            if(debug)
            {
                _quadtree1.createDebugQuadtreeMeshes();
            }

            _quadtree2 = new Quadtree();
            _quadtree2.create(_reduceMeshes, scene.BoundingBox);

            if (debug)
            {
                _quadtree2.createDebugQuadtreeMeshes();
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
            _quadtree1.render(GuiController.Instance.Frustum, false);

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
        /// En este caso, los edificios (sin el piso)
        /// </summary>
        public static void envRender()
        {
            _quadtree2.render(GuiController.Instance.Frustum, false);
        }
    }
}
