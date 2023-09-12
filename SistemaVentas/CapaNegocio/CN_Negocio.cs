﻿using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Negocio
    {

        private CD_Negocio objcd_negocio = new CD_Negocio();

        public Negocio ObtenerDatos()
        {
            return objcd_negocio.ObtenerDatos();
        }

        public bool GuardarDatos(Negocio obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (obj.ruc == "")
            {
                Mensaje += "Debe registrar el numero RUC de la empresa\n";
            }

            if (obj.nombre == "")
            {
                Mensaje += "Es necesario el nombre de la empresa\n";
            }

            if (obj.direccion == "")
            {
                Mensaje += "Es necesario la direccion de la empresa\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_negocio.GuardarDatos(obj, out Mensaje);
            }
        }

       
        public byte[] ObtenerLogo(out bool obtenido)
        {
            return objcd_negocio.ObtenerLogo(out obtenido);
        }

        public bool ActualizarLogo(byte[] image,  out string mensaje)
        {
            return objcd_negocio.actualizarLogo(image, out mensaje);
        }
    }
}