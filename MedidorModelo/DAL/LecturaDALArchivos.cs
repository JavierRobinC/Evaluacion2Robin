using MedidorModelo.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedidorModelo.DAL
{
    public class LecturaDALArchivos : ILecturaDAL
    {

        private LecturaDALArchivos()
        {

        }

        private static LecturaDALArchivos instancia;

        public static ILecturaDAL GetInstacia()
        {
            if (instancia == null)
            {
                instancia = new LecturaDALArchivos();
            }
            return instancia;
        }

        private static string archivo = "lecturas.txt";
        private static string url = Directory.GetCurrentDirectory() + "/" + archivo;

        public void AgregarLectura(Lectura lectura)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(url, true))
                {
                    string texto = lectura.NroMedidor + "|" + lectura.Fecha + "|" + lectura.ValorConsumo;
                    writer.WriteLine(texto);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error... No se pudo escribir en el archivo " + ex.Message);
            }
            finally
            {
            }
        }

        public List<Lectura> ObtenerLectura()
        {
            List<Lectura> lista = new List<Lectura>();
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    string texto = "";
                    do
                    {
                        texto = reader.ReadLine();
                        if (texto != null)
                        {
                            string[] arr = texto.Trim().Split('|');
                            int nroMedidor = Convert.ToInt32(arr[0]);
                            string fecha = arr[1];
                            double valorConsumo = Convert.ToDouble(arr[2]);
                            Lectura lectura = new Lectura()
                            {
                                NroMedidor = nroMedidor,
                                Fecha = fecha,
                                ValorConsumo = valorConsumo
                            };
                            lista.Add(lectura);
                        }

                    } while (texto != null);
                }
            }
            catch (Exception)
            {
                lista = null;
            }
            return lista;
        }
    }
}
