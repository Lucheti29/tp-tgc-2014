using AlumnoEjemplos.Los_Barto.Particulas;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.Los_Barto
{
    public class ParticulasChoque
    {
        /// <summary>
        /// ParticulasChoque: emisor de particulas que se
        /// genera por cada choque que tiene el auto
        /// </summary>

        protected bool ejecutando;
        protected EmisorDeParticulas emisorParticulas;
        protected float time;
        protected Vector3 posiction;
        protected string name;

        public ParticulasChoque(Vector3 posicion)
        {
            this.posiction = posicion;
            Device device = GuiController.Instance.D3dDevice;

            ejecutando = true;
            emisorParticulas = new EmisorDeParticulas(GuiController.Instance.AlumnoEjemplosMediaDir + "LOS_BARTO\\Particulas\\humo.png", 200, device);
            emisorParticulas.Posicion = posicion;
            emisorParticulas.GradoDeDispersion = 1000;
            emisorParticulas.TiempoDeVidaParticula = 15.0f;
            emisorParticulas.TiempoFrecuenciaDeCreacion = 0.01f;
            emisorParticulas.MinTamañoParticula = 5.0f;
            emisorParticulas.MaxTamañoParticula = 10.0f;
            emisorParticulas.FactorDeVelocidad = 5.0f;
            emisorParticulas.ColorInicialParticula = System.Drawing.Color.FromArgb(0xff, 0xff, 0xff, 0xff);
            emisorParticulas.ColorFinalParticula   = System.Drawing.Color.FromArgb(0x00, 0xff, 0xff, 0xff);
            time = GuiController.Instance.ElapsedTime;
        }

        public bool Alive
        {
            get { return ejecutando; }
        }

        public string Name
        {
            get { return name; }
        }

        public void render(float elapsedTime)
        {
            time += elapsedTime;
            if (time > 1)
            {
                emisorParticulas.detener();
                ejecutando = false;
            }
            else
            {
                emisorParticulas.render(elapsedTime);
            }
        }

        public void dispose()
        {
            emisorParticulas.detener();
        }

    }
}
