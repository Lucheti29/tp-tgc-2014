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
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-100, 5, -850), new Vector3(-1170, 15, 703)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(335, 15, 1193), new Vector3(-4181, 15, -3235)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-6738, 15, -1069), new Vector3(-6746, 15, 1171)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-1990, 15, -3174), new Vector3(849, 15, 1160)));
            
            //load peatones
            _peatones.Add(new PeatonInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-5180, 15, -3223),new List<Vector3> (){new Vector3( -5180, 15,-833),new Vector3(-7187, 15, -833),new Vector3( -7188, 15,-3203),new Vector3( -5180, 15, -3223)}));

            //Load autos
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3(910, 15, -689), new List<Vector3>() { new Vector3(970, 15, -2848), new Vector3(713, 15, -2928), new Vector3(-778, 15, -2924), new Vector3(-819, 15, -2918), new Vector3(-883, 15, -2844), new Vector3(-899, 15, -2778), new Vector3(-922, 15, -1384), new Vector3(-922, 15, -1245), new Vector3(-919, 15, -1211), new Vector3(-902, 15, -1159), new Vector3(-873, 15, -1111), new Vector3(-762, 15, -1086), new Vector3(650, 15, -1069), new Vector3(816, 15, -1065), new Vector3(913, 15, -1131), new Vector3(939, 15, -1266) }));
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3(1105, 25, -830), new List<Vector3>() { new Vector3(1105, 25, 1055), new Vector3(749, 30, 1055), new Vector3(-700, 30, 1055) }));
            

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
