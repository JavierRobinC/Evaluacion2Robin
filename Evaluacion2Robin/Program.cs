using Evaluacion2Robin.Comunicaciones;
using MedidorModelo.DAL;
using MedidorModelo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evaluacion2Robin
{
    class Program
    {

        private static ILecturaDAL lecturaDAL = LecturaDALArchivos.GetInstacia();
        private static IMedidorDAL medidorDAL = MedidorDALObjeto.GetInstancia();

        static bool Menu()
        {
            bool continuar = true;
            Console.WriteLine("\nBienvenido al menú");
            Console.WriteLine("1. Ingresar Lectura");
            Console.WriteLine("2. Mostrar Lectura");
            Console.WriteLine("3. Medidores");
            Console.WriteLine("0. Salir");
            switch (Console.ReadLine().Trim())
            {
                case "1":
                    IngresarLectura();
                    break;
                case "2":
                    MostrarLectura();
                    break;
                case "3":
                    MostrarMedidores();
                    break;
                case "0":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Ingrese una opcion valida");
                    break;
            }
            return continuar;
        }

        static void IniciarServidor()
        {

        }

        static void Main(string[] args)
        {
            HebraServidor hebra = new HebraServidor();
            Thread t = new Thread(new ThreadStart(hebra.Ejecutar));
            t.IsBackground = true;
            t.Start();
            while (Menu()) ;
        }

        static void IngresarLectura()
        {
            
            string txtnroMedidor;
            DateTime fecha = DateTime.Now;
            double valorConsumo;
            HebraCliente test = new HebraCliente();

            Console.WriteLine("Ingresando datos de la lectura...");
            bool esValido;

            string fechaIngresada = string.Format("{0:yyyy-MM-dd-HH-mm-ss}", fecha);

            Console.WriteLine("Ingrese el numero del medidor");
            txtnroMedidor = Console.ReadLine().Trim();

            if (test.validarMedidor(Convert.ToInt32(txtnroMedidor)))
            {
                do
                {
                    Console.WriteLine("Ingrese el valor del consumo");
                    esValido = double.TryParse(Console.ReadLine().Trim(), out valorConsumo);
                } while (!esValido);

                int nroMedidor = Convert.ToInt32(txtnroMedidor);
                Lectura lectura = new Lectura()
                {
                    NroMedidor = nroMedidor,
                    ValorConsumo = valorConsumo,
                    Fecha = fechaIngresada
                };
                lock (lecturaDAL)
                {
                    lecturaDAL.AgregarLectura(lectura);
                    Console.WriteLine("Se ha agregado correctamente...");
                }
            }
            else
            {
                Console.WriteLine("ERROR");
            }
        }

        static void MostrarLectura()
        {
            List<Lectura> lecturas = null;
            lock (lecturaDAL)
            {
                lecturas = lecturaDAL.ObtenerLectura();
            }               
            for (int i = 0; i < lecturas.Count(); i++)
            {
                Lectura actual = lecturas[i];
                Console.WriteLine("Numero del medidor : {1}, fecha: {2}, valor de consumo: {3}", i, actual.NroMedidor, actual.Fecha, actual.ValorConsumo);
            }
        }

        static void MostrarMedidores()
        {
            List<Medidor> medidores = null;
            lock (medidorDAL)
            {
                medidores = medidorDAL.ObtenerMedidores();

            };
            Console.WriteLine("Medidores existentes:");
            for (int i = 0; i < medidores.Count(); ++i)
            {
                Medidor actual = medidores[i];
                Console.WriteLine("\nNumero medidor:{1}",
                    i + 1, actual.NroMedidor);
            }
        }
    }
}
