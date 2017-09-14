using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clienteBattleship
{
    class Game
    {
        public int [,] gameArea;
        public bool [,] myShots;
        public bool[,] enemyShots;
        public int [] shipSizes;
        public int[] shipHits;
        public int shipNumber;
        public int width, height;

        public Game(int width, int height, int [] ships)
        {
            this.width = width;
            this.height = height;
            gameArea = new int[height, width];
            myShots = new bool[height, width];
            enemyShots = new bool[height, width];
            shipSizes = ships;
            shipNumber = ships.Length;
            shipHits = new int[shipNumber];

            obtainGameArea();

        }

        public void printGameArea()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j=0; j < width; j++)
                {
                    if (gameArea[i, j] == -1)
                    {
                        Console.Write('.');
                    } else
                    {
                        Console.Write('#');
                    }
                }
                Console.WriteLine();
            }
        }

        // Request ship position to the user and puts it in the game area
        public void obtainGameArea() 
        {

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    gameArea[i,j] = -1;
                }
            }

            bool[] placed = new bool[shipNumber];
            int placedNumber = 0;

            while (placedNumber < shipNumber)
            {

                printGameArea();

                Console.WriteLine("Seleccione un barco y un recuadro para colocarlo." +
                    " Así como si debe colocarse horizontal o vertical");
                Console.WriteLine("Escriba el numero de barco que quiere colocar," +
                    " luego la fila y columna donde debe colocarse, luego H o V" +
                    " si se quiere colocar horizontal o vertical respectivamente." +
                    " Separados por espacios");

                for (int i = 1; i <= shipNumber; i++)
                {
                    if (!placed[i - 1])
                        Console.WriteLine("Barco numero " + i + " de tamaño " + shipSizes[i - 1]);
                }

                try
                {
                    String line = Console.ReadLine();
                    String[] parts = line.Split(' ');
                    if (parts.Length != 4)
                    {
                        throw new Exception("Entrada no valida");
                    }
                    int ship = Int32.Parse(parts[0]);
                    int row = Int32.Parse(parts[1]);
                    int column = Int32.Parse(parts[2]);
                    char orientation = Char.Parse(parts[3]);

                    row--;
                    column--;
                    ship--;

                    if (orientation != 'H' && orientation != 'V')
                    {
                        throw new Exception("Entrada no valida");
                    }

                    if (ship < 0 || ship >= shipNumber)
                    {
                        throw new Exception("Entrada no valida. El numero de barco no es valido");
                    }

                    if (row < 0 || row >= height || column < 0 || column >= width)
                    {
                        throw new Exception("Entrada no valida. Coordenadas no validas");
                    }

                    
                    if (placed[ship])
                    {
                        Console.WriteLine("Este barco ya ha sido colocado");
                        continue;
                    }


                    int finalRow = row, finalColumn = column;

                    if (orientation == 'H')
                    {
                        finalColumn += shipSizes[ship] - 1;
                    } else
                    {
                        finalRow += shipSizes[ship] - 1;
                    }

                    if (finalRow >= height || finalColumn >= width)
                    {
                        Console.WriteLine("No es posible colocar ese barco en esa posición.");
                        return;
                    }

                    bool placeable = true;

                    for (int i=row; i<= finalRow && placeable; i++)
                    {
                        for (int j = column; j <= finalColumn && placeable; j++)
                        {
                            if (gameArea[i, j] != -1)
                            {
                                Console.WriteLine("No es posible colocar ese barco en esa posición.");
                                placeable = false;
                            }
                        }
                    }
                    if (placeable)
                    {
                        for (int i = row; i <= finalRow && placeable; i++)
                        {
                            for (int j = column; j <= finalColumn && placeable; j++)
                            {
                                gameArea[i, j] = ship;
                            }
                        }
                        placed[ship] = true;
                        placedNumber++;
                    }

                } catch (Exception)
                {
                    Console.WriteLine("Entrada no valida.");
                }

            }

        }

        public int shipsLeft()
        {
            int ans = 0;
            for (int i=0; i<shipNumber; i++)
            {
                if (shipHits[i] < shipSizes[i])
                {
                    ans++;
                }
            }
            return ans;
        }

        // returns if the shot was a hit
        public bool enemyShot(int row, int column) 
        {
            if (row >= 0 && row < height && column >= 0 && column < width)
            {
                if (enemyShots[row,column])
                {
                    return false;
                }
                else
                {
                    if (gameArea[row, column] == -1)
                    {
                        return false;
                    } else
                    {
                        if (!enemyShots[row, column])
                        {
                            int ship = gameArea[row, column];
                            shipHits[ship]++;
                            enemyShots[row, column] = true;

                            return true;
                        } else
                        {
                            return false;
                        }
                    }
                }
            } else
            {
                throw new Exception("Shot index out of bounds");
            }
        }

        public Coordinate obtainMyShot()
        {
            while (true) {

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++) {
                        if (!myShots[i, j])
                        {
                            Console.Write(".");
                        } else
                        {
                            Console.Write("X");
                        }
                    }
                    Console.WriteLine();
                }

                Console.WriteLine("Escriba la posición donde quiera lanzar un disparo," +
                    " fila y columna, separados por espacio.");

                try
                {
                    String line = Console.ReadLine();
                    String[] parts = line.Split(' ');
                    if (parts.Length != 2)
                    {
                        throw new Exception("Entrada no valida");
                    }
                    int row = Int32.Parse(parts[0]), column = Int32.Parse(parts[1]);
                    if (row < 0 || column < 0 || row >= height || column >= width)
                    {
                        throw new Exception("Entrada no valida");
                    }

                    myShots[row, column] = true;
                    Coordinate ans = new Coordinate();
                    ans.Row = row;
                    ans.Column = column;
                    return ans;
    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Entrada no valida");
                }
            }
        }

        public void gameOver()
        {
            Console.WriteLine("Usted ha perdido");
        }

        public void victory()
        {
            Console.WriteLine("Usted ha ganado");
        }
        
    }
}
