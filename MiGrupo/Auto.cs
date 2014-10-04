﻿using AlumnoEjemplos.MiGrupo.Entities;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.MiGrupo
{
    public class Auto
    {
        static Auto _instance = new Auto();

        // --------------- Variables de instancia ---------------

        private TgcMesh _mesh;
        private Vector3 _direccion;
        //private Vector3 _direccionDerrape;
        private Velocity _velocidad;
        private float _currentElapsedTime;
        private bool _girando = false;
        private int _cuentaRegresiva = 20;

        // --------------- Fin variables de instancia ---------------

        // --------------- Métodos de instancia ---------------
        public void inicializar(TgcMesh mesh)
        {
            _mesh = mesh;
            _direccion = new Vector3(0,0,-1);
            _velocidad = new Velocity();
        }

        public TgcMesh getMesh()
        {
           return _mesh;
        }

        public Vector3 getPosicion()
        {
            return _mesh.Position;
        }

        private float getVelocity()
        {
            return _velocidad.getAmount();
        }

        private void derrapar(Boolean right, Boolean left)
        {
            //TODO: hacer
        }

        public void aplicarMovimiento(Boolean right, Boolean left, Boolean up, Boolean down, Boolean brake)
        {
            Vector3 direccionAuxiliar = new Vector3(_direccion.X, _direccion.Y, _direccion.Z);

            if (!(_velocidad.getAmount() < 0.5f && _velocidad.getAmount() > -0.5f))
            {
                if (brake)
                {
                    if (right)
                        derrapar(true, false);
                    else if (left)
                        derrapar(false, true);
                    else
                        _velocidad.frenar();
                }
                else if (left || right)
                {
                    if (!_girando)
                    {
                        int yaw = 1;
                        int sign = 1;

                        if (left)
                        {
                            yaw = -1;
                            sign = -1;
                        }

                        _direccion = Movement.doblar(_direccion, _currentElapsedTime, yaw);
                        _direccion = Vector3.Normalize(_direccion);

                        //Calcular ángulo entre movimiento anterior y el nuevo
                        float angle = FastMath.Acos(Vector3.Dot(_direccion, direccionAuxiliar)) * sign;

                        //Rotar auto y camara en Y
                        _mesh.rotateY(angle);
                        Camara.rotar(angle);
                        _girando = true;
                    }
                    //TODO: deshardcodear esto por el amor a Alah
                    else
                    {
                        if (_cuentaRegresiva <= 0)
                        {
                            _girando = false;
                            _cuentaRegresiva = 20;
                        }
                        else
                        {
                            _cuentaRegresiva = _cuentaRegresiva - 1;
                        }
                    }
                }
            }

            if (up)
                _velocidad.acelerar();
            else if (down)
                _velocidad.desacelerar();
        }

        public void render(float elapsedTime)
        {
            _currentElapsedTime = elapsedTime;
            Teclado.handlear();
            _velocidad.friccion();
            Camara.setearPosicion(getPosicion());
            _mesh.move(_direccion * _velocidad.getAmount() * elapsedTime);
            _mesh.render();
        }
        // --------------- Fin de métodos de instancia ---------------

        // --------------- Métodos estáticos ---------------
        public static Auto getInstance()
        {
            if (_instance == null)
            {
                _instance = new Auto();
            }
            return _instance;
        }

        public static void reset()
        {
            _instance = new Auto();
        }
        // --------------- Fin Métodos estáticos ---------------



    }
}