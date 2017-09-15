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
        public static int[] ships;
        public static int height, width;
        public static int player;
        static void Main(string[] args)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient("localhost", 2307);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo conectar al servidor adios");
                Console.ReadLine();
                return;
            }

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            try
            {
                Console.WriteLine("Esperando respuesta del servidor");

                // Lee los tamaños de los barcos
                String line = reader.ReadLine();
                String []parts = line.Split(' ');
                ships = new int[parts.Length];
                for (int i=0;i<parts.Length; i++)
                {
                    ships[i] = Int32.Parse(parts[i]);
                }
                
                // Lee las dimensiones del barco
                line = reader.ReadLine();
                parts = line.Split(' ');

                width = Int32.Parse(parts[0]);
                height = Int32.Parse(parts[1]);

                // Lee que jugador somos
                line = reader.ReadLine();
                player = Int32.Parse(line);

                Game myGame = new Game(width, height, ships);

                int turn = player;

                while (!myGame.isGameOver() && !myGame.isVictory() && client.Connected)
                {
                    if (turn % 2 == 0) // tu turno
                    {
                        Coordinate c = myGame.obtainMyShot();
                        line = c.ToString();
                        writer.WriteLine(line);
                        writer.Flush();
                        Console.WriteLine("Esperando la respuesta del otro jugador.");
                        line = reader.ReadLine();
                        if (line.CompareTo("Hit") == 0)
                        {
                            Console.WriteLine("Tu tiro fue exitoso");
                            myGame.successfulShot();
                        } else
                        {
                            Console.WriteLine("Tu tiro falló");
                        }
                    } else // otro turno
                    {
                        Console.WriteLine("Esperando la respuesta del otro jugador.");
                        line = reader.ReadLine();
                        parts = line.Split(' ');
                        int row = Int32.Parse(parts[0]), column = Int32.Parse(parts[1]);
                        Console.WriteLine("Recibiste un tiro en la posición {0},{1}", row + 1, column + 1);
                        if (myGame.enemyShot(row, column))
                        {
                            Console.WriteLine("El tiro fue exitoso");
                            writer.WriteLine("Hit");
                        } else
                        {
                            Console.WriteLine("El tiro fallo");
                            writer.WriteLine("Miss");
                        }
                        writer.Flush();
                    }
                    turn++;
                }
                if (myGame.isGameOver())
                {
                    myGame.gameOver();
                } else if (myGame.isVictory())
                {
                    myGame.victory();
                } else
                {
                    Console.WriteLine("Se ha desconectado del servidor");
                }
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
