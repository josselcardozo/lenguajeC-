using System;

namespace Practico4
{
    // Clase representa la estructura completa del árbol
    public class Arbol
    {
        // Propiedad que almacena el nodo raíz
        public NodoArbol Raiz { get; set; }
        
        // Nombre del árbol
        public string Nombre { get; set; }

        // Constructor, crea un nuevo árbol con un nombre específico
        public Arbol(string nombre)
        {
            Nombre = nombre;  // Asigna el nombre del árbol
        }

        // Método público calcula la altura total del árbol
        public int CalcularAltura()
        {
            return CalcularAltura(Raiz);  // Llama al método recursivo privado
        }

        // Método privado calcula la altura desde un nodo específico
        private int CalcularAltura(NodoArbol nodo)
        {
            if (nodo == null) return 0;                    // nodo nulo
            if (nodo.Hijos.Count == 0) return 1;           // nodo hoja (altura 1)

            int maxAltura = 0;                             // Variable para altura máxima
            foreach (var hijo in nodo.Hijos)               // Recorrer todos los hijos
            {
                // Calcula la altura de cada hijo y mantener la máxima
                maxAltura = Math.Max(maxAltura, CalcularAltura(hijo));
            }
            return maxAltura + 1;  // Altura = altura del hijo más alto + 1 (este nivel)
        }

        // Método público cuenta todos los nodos del árbol
        public int ContarNodos()
        {
            return ContarNodos(Raiz);  // Llama al método recursivo privado
        }

        // Método para contar nodos desde un nodo específico
        private int ContarNodos(NodoArbol nodo)
        {
            if (nodo == null) return 0;  // Caso base: nodo nulo
            
            int count = 1;  // Contar este nodo (1)
            
            foreach (var hijo in nodo.Hijos)  // Recorrer todos los hijos
            {
                count += ContarNodos(hijo);  // Sumar nodos de cada subárbol
            }
            
            return count;  // Retornar total acumulado
        }
    }
}
