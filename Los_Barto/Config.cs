using AlumnoEjemplos.Los_Barto.Entities;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;

namespace AlumnoEjemplos.Los_Barto
{
    public class Config
    {
        /// <summary>
        /// Config: se encarga de guardar toda la información
        /// de entidades para su inicialización (pasajeros, autos, peatones), 
        /// permitiendo agregar nuevas de manera rápida
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
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(335, 5, 1193), new Vector3(-4181, 5, -3235)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-6738, 5, -1069), new Vector3(-6746, 5, 1171)));
            _pasajeros.Add(new PasajeroInfo("SkeletalAnimations\\BasicHuman\\WomanJeans-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-1990, 5, -3174), new Vector3(849, 5, 1160)));
            
            //load peatones
            _peatones.Add(new PeatonInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
                "SkeletalAnimations\\BasicHuman\\", new Vector3(-5180, 5, -3223),new List<Vector3> (){new Vector3( -5180, 5,-833),new Vector3(-7187, 5, -833),new Vector3( -7188, 5,-3203),new Vector3( -5180, 5, -3223)}));
            _peatones.Add(new PeatonInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
               "SkeletalAnimations\\BasicHuman\\", new Vector3( -1201, 5,-809), new List<Vector3>() { new Vector3( -2718,5,-809), new Vector3( -2718, 5,-1234), new Vector3( -2829,5, -3203), new Vector3(-1228,5, -3203) }));
            
            _peatones.Add(new PeatonInfo("SkeletalAnimations\\BasicHuman\\BasicHuman-TgcSkeletalMesh.xml",
              "SkeletalAnimations\\BasicHuman\\", new Vector3(-1199, 15, 1201), new List<Vector3>() { new Vector3(-4828, 15, 1200), new Vector3(-4828, 15, -3181), new Vector3(-1199, 15, -3180), new Vector3(-1199, 15, 1179) }));          
            //Load autos
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3(910, 25, -689), new List<Vector3>() { new Vector3(970, 25, -2848), new Vector3(713, 25, -2928), new Vector3(-778, 25, -2924), new Vector3(-819, 25, -2918), new Vector3(-883, 25, -2844), new Vector3(-899, 25, -2778), new Vector3(-922, 25, -1384), new Vector3(-922, 25, -1245), new Vector3(-919, 25, -1211), new Vector3(-902, 25, -1159), new Vector3(-873, 25, -1111), new Vector3(-762, 25, -1086), new Vector3(650, 25, -1069), new Vector3(816, 25, -1065), new Vector3(913, 25, -1131), new Vector3(939, 25, -1266) }));
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3(1105, 25, -830),
                new List<Vector3>() { new Vector3(1095, 25, 750),new Vector3(1085, 25, 800), new Vector3(1040,25,895), new Vector3(990,25,970),new Vector3(930,25,1020), new Vector3(855,25,1055),// puntos para el primer giro
                                  new Vector3(749, 25, 1055), new Vector3(-700, 25, 1055), new Vector3(-805, 25, 1055), new Vector3(-900, 25, 1010), new Vector3(-975, 25, 960), new Vector3(-1025, 25, 900), new Vector3(-1065, 25, 820),
                                  new Vector3(-1065, 25, -2842),new Vector3(-1162,15, -3128),new Vector3( -849, 15, -3070),// 3 giro
                                  new Vector3(  832, 15, -3062), new Vector3( 1039, 15, -3050), new Vector3 ( 1090, 15, -2716) }));  //4giro                          
            _autos.Add(new AutoInfo(alumnoMediaFolder + "LOS_BARTO\\auto\\auto-TgcScene.xml", new Vector3 (-2883, 15, -800),
               new List<Vector3>() { new Vector3( -2936, 15, 827),new Vector3( -3000, 15, 953), new Vector3(-3221, 15, 1055), //1giro
                                    new Vector3( -6784, 15, 1057),new Vector3(-7015, 15, 992),new Vector3( -7048, 15, 659), //2 giro
                                    new Vector3( -7051, 15, -2807), new Vector3 ( -7025, 15, -3041), new Vector3( -6853, 15, -3086),//3giro
               new Vector3( -3278, 15, -3064),new Vector3( -3015, 15, -2993),new Vector3( -2918, 15,-2828)    }  )     );           
             
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
