using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX.DirectInput;
using AlumnoEjemplos.MiGrupo.Entities;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Shaders;
using System.Windows.Forms;
using TgcViewer.Utils.Terrain;
using AlumnoEjemplos.MiGrupo.Optimizacion.GrillaRegular;

namespace AlumnoEjemplos.MiGrupo
{
    public class EjemploAlumno : TgcExample
    {
        TgcScene taxiScene;
        TgcScene ciudadScene;
        Microsoft.DirectX.Direct3D.Effect effect;
        float kx;       // coef. de reflexion
        float kc;       // coef. de refraccion
        bool fresnel = true;

        GrillaRegular grilla;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Grupo LOS_BARTO";
        }

        public override string getDescription()
        {
            return "CrazyTaxi - Un taxi que debe llevar pasajeros de un punto de la ciudad a otro en un tiempo establecido al menos 5 veces.";
        }

        public override void init()
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Carpeta de archivos Media del alumno

            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;
            //primero cargamos una escena 3D entera.
            TgcSceneLoader loader = new TgcSceneLoader();

            //Luego cargamos otro modelo aparte que va a hacer el taxi
            taxiScene = loader.loadSceneFromFile(alumnoMediaFolder + "LOS_BARTO\\taxi\\taxi-TgcScene.xml");
            //Cargamos la ciudad
            ciudadScene = loader.loadSceneFromFile(alumnoMediaFolder + "LOS_BARTO\\ciudad\\ciudad-TgcScene.xml");

            //solo nos interesa el taxi
            Auto.getInstance().inicializar(taxiScene.Meshes[0]);
            Flecha.getInstance().inicializar();
            Flecha.getInstance().show();

            Camara.inicializar(Auto.getInstance().getPosicion());

            GameControl.getInstance().inicializar();

           GuiController.Instance.UserVars.addVar("posPas");
            GuiController.Instance.UserVars.addVar("posTaxi", Auto.getInstance().getMesh().Position);
            GuiController.Instance.UserVars.addVar("posDest");
            GuiController.Instance.UserVars.addVar("velocidad");
            GuiController.Instance.UserVars.addVar("DistTaxi");
            GuiController.Instance.UserVars.addVar("distDest");
            //GuiController.Instance.UserVars.addVar("ptosRec");// punto en el q esta el recorrido del auto
            GuiController.Instance.UserVars.addVar("DistpRec");
            
            //Modifier para habilitar o no el renderizado del BoundingBox del personaje
            GuiController.Instance.Modifiers.addBoolean("showBoundingBox", "Bouding Box", false);

            //Crear Grid Regular
            //TODO: habría que dividir el piso del resto de los mesh
            grilla = new GrillaRegular();
            grilla.create(ciudadScene.Meshes, ciudadScene.BoundingBox);
            grilla.createDebugMeshes();

            #region inicializo EnvMap
            // Arreglo las normales del taxi
            {
                int[] adj = new int[Auto.getInstance().getMesh().D3dMesh.NumberFaces * 3];
                Auto.getInstance().getMesh().D3dMesh.GenerateAdjacency(0, adj);
                Auto.getInstance().getMesh().D3dMesh.ComputeNormals(adj);
            }
            //Cargar Shader personalizado
            effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesDir + "Shaders\\WorkshopShaders\\Shaders\\EnvMap.fx");

            // le asigno el efecto a la malla del taxi
            Auto.getInstance().getMesh().Effect = effect;
           //seteo los niveles iniciales de kx y kc
            kx=
            kc = 0.5f;
            GuiController.Instance.Modifiers.addFloat("Reflexion", 0, 1, kx);
            GuiController.Instance.Modifiers.addFloat("Refraccion", 0, 1, kc);
            GuiController.Instance.Modifiers.addBoolean("Fresnel", "fresnel", true);
            GuiController.Instance.Modifiers.addBoolean("cubicMap", "CboMap", true);
            #endregion inicializo EnvMap
        }

        public override void render(float elapsedTime)
        {

           //este if envita q el  cubeMap se actualice.
           //Para q el env Map funcione bien el cubeMap debe actualizarse en cada frame pero puse esto para q vean como varian los FPS
            if ((bool)GuiController.Instance.Modifiers.getValue("cubicMap"))
            {

                //--------------------inicio del hardcodeo del enviroment map 
                Microsoft.DirectX.Direct3D.Device device = GuiController.Instance.D3dDevice;
                Control panel3d = GuiController.Instance.Panel3d;
                float aspectRatio = (float)panel3d.Width / (float)panel3d.Height;

                //Cargar variables de shader
                effect.SetValue("fvLightPosition", new Vector4(0, 400, 0, 0));
                effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.RotCamera.getPosition()));
                effect.SetValue("kx", (float)GuiController.Instance.Modifiers["Reflexion"]);
                effect.SetValue("kc", (float)GuiController.Instance.Modifiers["Refraccion"]);
                effect.SetValue("usar_fresnel", (bool)GuiController.Instance.Modifiers["Fresnel"]);

                // --------------------------------------------------------------------
                device.EndScene();
                CubeTexture g_pCubeMap = new CubeTexture(device, 256, 1, Usage.RenderTarget, Format.A16B16G16R16F, Pool.Default);
                Surface pOldRT = device.GetRenderTarget(0);
                // ojo: es fundamental que el fov sea de 90 grados.
                // asi que re-genero la matriz de proyeccion
                device.Transform.Projection =
                    Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(90.0f),
                        1f, 1f, 10000f);

                // Genero las caras del enviroment map
                for (CubeMapFace nFace = CubeMapFace.PositiveX; nFace <= CubeMapFace.NegativeZ; ++nFace)
                {
                    Surface pFace = g_pCubeMap.GetCubeMapSurface(nFace, 0);
                    device.SetRenderTarget(0, pFace);
                    Vector3 Dir, VUP;
                    Color color;
                    switch (nFace)
                    {
                        default:
                        case CubeMapFace.PositiveX:
                            // Left
                            Dir = new Vector3(1, 0, 0);
                            VUP = new Vector3(0, 1, 0);
                            color = Color.Black;
                            break;
                        case CubeMapFace.NegativeX:
                            // Right
                            Dir = new Vector3(-1, 0, 0);
                            VUP = new Vector3(0, 1, 0);
                            color = Color.Red;
                            break;
                        case CubeMapFace.PositiveY:
                            // Up
                            Dir = new Vector3(0, 1, 0);
                            VUP = new Vector3(0, 0, -1);
                            color = Color.Gray;
                            break;
                        case CubeMapFace.NegativeY:
                            // Down
                            Dir = new Vector3(0, -1, 0);
                            VUP = new Vector3(0, 0, 1);
                            color = Color.Yellow;
                            break;
                        case CubeMapFace.PositiveZ:
                            // Front
                            Dir = new Vector3(0, 0, 1);
                            VUP = new Vector3(0, 1, 0);
                            color = Color.Green;
                            break;
                        case CubeMapFace.NegativeZ:
                            // Back
                            Dir = new Vector3(0, 0, -1);
                            VUP = new Vector3(0, 1, 0);
                            color = Color.Blue;
                            break;
                    }

                    //Obtener ViewMatrix haciendo un LookAt desde la posicion final anterior al centro de la camara
                    Vector3 Pos = Auto.getInstance().getMesh().Position;
                    device.Transform.View = Matrix.LookAtLH(Pos, Pos + Dir, VUP);

                    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1.0f, 0);
                    device.BeginScene();

                    //Renderizar 
                    renderScene(elapsedTime, true);

                    device.EndScene();
                    //string fname = string.Format("face{0:D}.bmp", nFace);
                    //SurfaceLoader.Save(fname, ImageFileFormat.Bmp, pFace);


                }
                // restuaro el render target
                device.SetRenderTarget(0, pOldRT);
                //TextureLoader.Save("test.bmp", ImageFileFormat.Bmp, g_pCubeMap);

                // Restauro el estado de las transformaciones
                GuiController.Instance.CurrentCamera.updateViewMatrix(device);
                device.Transform.Projection =
                    Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f),
                        aspectRatio, 1f, 10000f);

                // dibujo pp dicho
                device.BeginScene();
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

                effect.SetValue("g_txCubeMap", g_pCubeMap);
                renderScene(elapsedTime, false);
                g_pCubeMap.Dispose();
            //----fin del hardcodeo del envMap
            }
            else {
                renderScene(elapsedTime, false);
            }
        }


        public void renderScene(float elapsedTime, bool cubemap)
        {//renderizo toda la escena (Autos, taxi,  pasajeros,etc)
           
            //Ver si hay que mostrar el BoundingBox
            bool showBB = (bool)GuiController.Instance.Modifiers.getValue("showBoundingBox");
            //ciudadScene.renderAll();

            grilla.render(GuiController.Instance.Frustum, false);

            if (showBB)
            {
                foreach (TgcMesh mesh in ciudadScene.Meshes)
                {
                    mesh.BoundingBox.render();
                }
            }
  
            Teclado.handlear();
           
            Flecha.getInstance().render(elapsedTime);

            GameControl.getInstance().render(elapsedTime);

            if (!cubemap)
            {
                // dibujo el mesh
                
                Auto.getInstance().getMesh().Technique = "RenderCubeMap";
                Auto.getInstance().checkCollision(ciudadScene); 
                Auto.getInstance().render(elapsedTime);
            }
        }

       
      

        public override void close()
        {
            Auto.getInstance().getMesh().dispose();
            ciudadScene.disposeAll();
            GameControl.getInstance().disposeAll();
        }
    }
}