using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TgcViewer;
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    public class EnviromentMap
    {
      /* aca iria toda la logica del env map*/
        Microsoft.DirectX.Direct3D.Effect _effect;
        private float kc, kx;

        public EnviromentMap()
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
            GuiController.Instance.Modifiers.addBoolean("cubicMap", "CboMap", true);
        }

        public void calculate(float elapsedTime)
        {
            if ((bool)GuiController.Instance.Modifiers.getValue("cubicMap"))
            {
                //--------------------inicio del hardcodeo del enviroment map 
                Microsoft.DirectX.Direct3D.Device device = GuiController.Instance.D3dDevice;
                Control panel3d = GuiController.Instance.Panel3d;
                float aspectRatio = (float)panel3d.Width / (float)panel3d.Height;

                //Cargar variables de shader
                _effect.SetValue("fvLightPosition", new Vector4(0, 400, 0, 0));
                _effect.SetValue("fvEyePosition", TgcParserUtils.vector3ToFloat3Array(GuiController.Instance.RotCamera.getPosition()));
                _effect.SetValue("kx", (float)GuiController.Instance.Modifiers["Reflexion"]);
                _effect.SetValue("kc", (float)GuiController.Instance.Modifiers["Refraccion"]);
                _effect.SetValue("usar_fresnel", (bool)GuiController.Instance.Modifiers["Fresnel"]);

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
                    if (nFace != CubeMapFace.NegativeY)
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
                    Render.envRender();

                    device.EndScene();
                    //string fname = string.Format("face{0:D}.bmp", nFace);
                    //SurfaceLoader.Save(fname, ImageFileFormat.Bmp, pFace);
                    }
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

                _effect.SetValue("g_txCubeMap", g_pCubeMap);
                Render.renderScene(elapsedTime, false);
                g_pCubeMap.Dispose();
                //----fin del hardcodeo del envMap
            }
            else
            {
                Render.renderScene(elapsedTime, false);
            }
        }

        public void dispose()
        {
            _effect.Dispose();
        }
    }
}
