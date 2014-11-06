using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    public class Camara
    {    

        private static List<TgcMesh> _obstaculos;
        public static void inicializar(Vector3 posicion)
        {
            GuiController.Instance.ThirdPersonCamera.Enable = true;
            GuiController.Instance.ThirdPersonCamera.rotateY(Geometry.DegreeToRadian(180f));
            GuiController.Instance.ThirdPersonCamera.setCamera(posicion, 150, -300);

            _obstaculos = new List<TgcMesh>();
        }

        public static void rotar(float angle)
        {
            GuiController.Instance.ThirdPersonCamera.rotateY(angle);
        }

        public static void setearPosicion(Vector3 posicion)
        {
            GuiController.Instance.ThirdPersonCamera.Target = posicion;
            ajustarPosiocionDeCamara(Auto.getInstance().getMesh(), getObstaculos());
        }

        /// <summary>
        /// Ajustar la posicion de la camara segun la colision con los objetos del escenario.
        /// Acerca la distancia entre el persona y la camara si hay colisiones de objetos
        /// en el medio
        /// </summary>
        private static void ajustarPosiocionDeCamara(TgcMesh tgcMesh, List<TgcMesh> list)
        {

            TgcThirdPersonCamera camera = GuiController.Instance.ThirdPersonCamera;
            camera.OffsetHeight = (float)GuiController.Instance.Modifiers["offsetHeight"];
            camera.OffsetForward = (float)GuiController.Instance.Modifiers["offsetForward"];
            
            //Pedirle a la camara cual va a ser su proxima posicion
            Vector3 segmentA;
            Vector3 segmentB;
            camera.generateViewMatrix(out segmentA, out segmentB);

            //Detectar colisiones entre el segmento de recta camara-taxi y todos los objetos del escenario
            Vector3 q;
            float minDistSq = FastMath.Pow2(camera.OffsetForward);
            foreach (TgcMesh obstaculo in list)
            {
                //Hay colision del segmento camara-taxi y el objeto
                if (TgcCollisionUtils.intersectSegmentAABB(segmentB, segmentA, obstaculo.BoundingBox, out q))
                {
                    //Si hay colision, guardar la que tenga menor distancia
                    float distSq = (Vector3.Subtract(q, segmentB)).LengthSq();
                    if (distSq < minDistSq)
                    {
                        minDistSq = distSq;

                        //Le restamos un poco para que no se acerque tanto
                        minDistSq /= 2;
                    }
                }
            }

            //Acercar la camara hasta la minima distancia de colision encontrada (pero ponemos un umbral maximo de cercania)
            float newOffsetForward = -FastMath.Sqrt(minDistSq);

            if (newOffsetForward > -100)
            {
                newOffsetForward = -300;
            }

            camera.OffsetForward = newOffsetForward;

        }

        public static List<TgcMesh> getObstaculos()
        {
            return _obstaculos;
        }
    }

}
