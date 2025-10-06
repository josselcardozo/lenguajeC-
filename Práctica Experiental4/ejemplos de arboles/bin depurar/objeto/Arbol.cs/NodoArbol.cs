using System.Collections.Generic;
using System.Drawing;

namespace Practico4
{
    // Clase que representa un nodo individual en el árbol
    public class NodoArbol
    {
        // Propiedad que almacena el valor/texto del nodo
        public string Valor { get; set; }
        
        // Lista de nodos hijos (las conexiones desde este nodo)
        public List<NodoArbol> Hijos { get; set; }
        
        // Posición (coordenadas X,Y) donde se dibujará el nodo en la imagen
        public Point Posicion { get; set; }
        
        // Área rectangular que ocupa el nodo (para colisiones y dibujo)
        public Rectangle Bounds { get; set; }

        // Constructor inicializa un nuevo nodo con un valor específico
        public NodoArbol(string valor)
        {
            Valor = valor;                          // Asigna el valor del nodo
            Hijos = new List<NodoArbol>();          // Inicializa lista vacía de hijos
        }

        // Método para agregar un nodo hijo a este nodo
        public void AgregarHijo(NodoArbol hijo)
        {
            Hijos.Add(hijo);  // Añade el nodo a la lista de hijos
        }

        // Método para representación del nodo
        public override string ToString()
        {
            return $"Nodo: {Valor}, Hijos: {Hijos.Count}";
        }
    }
}
