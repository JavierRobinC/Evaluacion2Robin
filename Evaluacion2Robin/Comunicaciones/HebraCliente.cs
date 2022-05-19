using EvaluacionSocketR.Comunicaciones;
using MedidorModelo.DAL;
using MedidorModelo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluacion2Robin.Comunicaciones
{
    class HebraCliente
    {
        private static ILecturaDAL lecturaDAL = LecturaDALArchivos.GetInstacia();
        private IMedidorDAL medidorDAL = MedidorDALObjeto.GetInstancia();
        private ClienteCom clienteCom;

        public HebraCliente(ClienteCom clienteCom)
        {
            this.clienteCom = clienteCom;
        }

        public HebraCliente()
        {

        }

        public void Ejecutar()
        {
            clienteCom.Escribir("Porfavor, ingrese numero del medidor");
            int nroMedidor = int.Parse(clienteCom.Leer());
            clienteCom.Escribir("Porvafor, ingrese valor de consumo");
            double valorConsumo = double.Parse(clienteCom.Leer());
            DateTime fecha = DateTime.Now;
            string fechaIngresada = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", fecha);
            Lectura lectura = new Lectura()
            {
                NroMedidor = nroMedidor,
                ValorConsumo = valorConsumo,
                Fecha = fechaIngresada
            };

            lock (lecturaDAL)
            {
                lecturaDAL.AgregarLectura(lectura);
                Console.WriteLine("¡Se ha ingresado correctamente!");
            }
            
            clienteCom.Desconectar();
        }

        public Boolean validarMedidor(int nroMedidor)
        {
            List<Medidor> medidores = new List<Medidor>();
            medidores = medidorDAL.ObtenerMedidores();
            if (medidores.Any(i => i.NroMedidor == nroMedidor))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
