using AlumnoEjemplos.MiGrupo.Entities;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.MiGrupo
{
    public class Config
    {
        /// <summary>
        /// Config: se encarga de guardar toda la información
        /// de entidades (pasajeros, autos, peatones), permitiendo
        /// agregar nuevas de manera rápida
        /// </summary>

        private static string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

        public static List<PasajeroInfo> _pasajeros;
        public static List<AutoInfo> _autos;
        public static List<PeatonInfo> _peatones;

        public static void load()
        {
            _pasajeros = new List<PasajeroInfo>();
            _autos = new List<AutoInfo>();
            _peatones = new List<PeatonInfo>();

            //Load pasajeros
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(100, 5, -850), new Vector3(700, 5, -1850)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-100, 5, -850), new Vector3(-1170, 5, 703)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(335, 5, 1193), new Vector3(-4181, 5, -3235)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-6738, 5, -1069), new Vector3(-6746, 5, 1171)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-1990, 5, -3174), new Vector3(849, 5, 1160)));
            
            //load peatones
            _peatones.Add(new PeatonInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-5180, 5, -3223),new List<Vector3> (){new Vector3( -5180, 5,-833),new Vector3(-7187, 5, -833),new Vector3( -7188, 5,-3203),new Vector3( -5180, 5, -3223)}));

            //Load autos
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3(910, 25, -689), new List<Vector3>() { new Vector3(970, 25, -2848), new Vector3(713, 25, -2928), new Vector3(-778, 25, -2924), new Vector3(-819, 25, -2918), new Vector3(-883, 25, -2844), new Vector3(-899, 25, -2778), new Vector3(-922, 25, -1384), new Vector3(-922, 25, -1245), new Vector3(-919, 25, -1211), new Vector3(-902, 25, -1159), new Vector3(-873, 25, -1111), new Vector3(-762, 25, -1086), new Vector3(650, 25, -1069), new Vector3(816, 25, -1065), new Vector3(913, 25, -1131), new Vector3(939, 25, -1266) }));
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3(1105, 25, -830),
                new List<Vector3>() { new Vector3(1095, 25, 750),new Vector3(1085, 25, 800), new Vector3(1040,25,895), new Vector3(990,25,970),new Vector3(930,25,1020), new Vector3(855,25,1055),// puntos para el primer giro
                                  new Vector3(749, 25, 1055), new Vector3(-700, 25, 1055), new Vector3(-805, 25, 1055), new Vector3(-900, 25, 1010), new Vector3(-975, 25, 960), new Vector3(-1025, 25, 900), new Vector3(-1065, 25, 820), new Vector3(-1065, 25, 400)}));
           

        }

        public static List<PasajeroInfo> getPasajeros()
        {
            return _pasajeros;
        }
        public static List<PeatonInfo> getPeatones()
        {
            return _peatones;
        }

        public static List<AutoInfo> getAutos()
        {
            return _autos;
        }
    }
}
