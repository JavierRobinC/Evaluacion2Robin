using EvaluacionSocketR.Comunicaciones;
using MedidorModelo.DAL;
using MedidorModelo.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evaluacion2Robin.Comunicaciones
{
    public class HebraServidor
    {
        private static ILecturaDAL lecturaDAL = LecturaDALArchivos.GetInstacia();

        public void Ejecutar()
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            
            Console.WriteLine("S: Iniciando servidor en puerto {0}", puerto);
            ServerSocket servidor = new ServerSocket(puerto);
            if (servidor.Iniciar())
            {
                Console.WriteLine("Servidor iniciado");
                while (true)
                {
                    Console.WriteLine("S: Esperando Cliente... ");
                    Socket cliente = servidor.ObtenerCliente();
                    Console.WriteLine("S: Cliente recibido");

                    ClienteCom clienteCom = new ClienteCom(cliente);

                    HebraCliente clienteHebra = new HebraCliente(clienteCom);
                    Thread t = new Thread(new ThreadStart(clienteHebra.Ejecutar));
                    t.IsBackground = true;
                    t.Start();
                    
                }
            }
            else
            {
                Console.WriteLine("Falló, no se puede conectar al servidor en el puerto {0}", puerto);
            }
        }

    }
}
