using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace clienteBattleship
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] ships = { 5, 4, 3, 3, 2 };
            Game myGame = new Game(10, 10, ships);

            TcpClient client = null;
            try
            {
                client = new TcpClient("localhost", 2307);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo conectar al cliente adios");
                return;
            }

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            try
            {
                writer.WriteLine("hola");
                writer.Flush();

                Console.WriteLine("Dijo" + reader.ReadLine());

                writer.WriteLine("adios");
                writer.Flush();
            }
            catch(Exception ex)
            {
                throw;
            }

            reader.Close();
            writer.Close();
            stream.Close();
        }
    }
}
