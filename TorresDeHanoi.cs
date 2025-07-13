using System;
using System.Collections.Generic;

class TorreDeHanoi
{
    static Stack<int> origen = new Stack<int>();
    static Stack<int> auxiliar = new Stack<int>();
    static Stack<int> destino = new Stack<int>();

    /// <summary>
    /// Mueve los discos entre torres utilizando recursión y pilas.
    /// </summary>
    static void MoverDiscos(int n, Stack<int> desde, Stack<int> hacia, Stack<int> apoyo, string nombreDesde, string nombreHacia, string nombreApoyo)
    {
        if (n == 1)
        {
            int disco = desde.Pop();
            hacia.Push(disco);
            Console.WriteLine($"Mover disco {disco} de {nombreDesde} a {nombreHacia}");
        }
        else
        {
            MoverDiscos(n - 1, desde, apoyo, hacia, nombreDesde, nombreApoyo, nombreHacia);
            MoverDiscos(1, desde, hacia, apoyo, nombreDesde, nombreHacia, nombreApoyo);
            MoverDiscos(n - 1, apoyo, hacia, desde, nombreApoyo, nombreHacia, nombreDesde);
        }
    }

    static void Main()
    {
        int n = 3; // Puedes cambiar el número de discos
        for (int i = n; i >= 1; i--) origen.Push(i);

        Console.WriteLine("Pasos para resolver las Torres de Hanoi:");
        MoverDiscos(n, origen, destino, auxiliar, "Origen", "Destino", "Auxiliar");
    }
}
