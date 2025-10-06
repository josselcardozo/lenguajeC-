using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Practico4
{
    // Clase responsable de cargar datos desde archivos de texto y construir la estructura del árbol
    public class CargadorDatosArbol
    {
        // Carga un árbol completo desde un archivo de texto
        public static Arbol CargarDesdeArchivo(string rutaArchivo, string nombreArbol)
        {
            // Verifica que el archivo exista antes de intentar leerlo
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"No se encontró el archivo: {rutaArchivo}");
            }

            // Lee todas las líneas del archivo, ignorando líneas vacías o con solo espacios
            var lineas = File.ReadAllLines(rutaArchivo)
                           .Where(linea => !string.IsNullOrWhiteSpace(linea))
                           .ToArray();

            // Crea un nuevo árbol con el nombre especificado
            var arbol = new Arbol(nombreArbol);
            
            // Diccionario para almacenar todos los nodos por su nombre 
            var diccionarioNodos = new Dictionary<string, NodoArbol>();
            
            // Conjunto para identificar posibles nodos raíz 
            var posiblesRaices = new HashSet<string>();

            Console.WriteLine($"Cargando árbol desde: {Path.GetFileName(rutaArchivo)}");
            Console.WriteLine($"Líneas encontradas: {lineas.Length}");

            // Crea todos los nodos mencionados en el archivo
            foreach (var linea in lineas)
            {
                // Divide la línea en padre e hijos usando ':' como separador
                var partes = linea.Split(':');
                if (partes.Length == 2) // Verifica que tenga el formato correcto
                {
                    // Obtiene y limpia el nombre del padre 
                    string padre = partes[0].Trim();
                    
                    // Crea un nodo padre si no existe aún
                    if (!diccionarioNodos.ContainsKey(padre))
                    {
                        diccionarioNodos[padre] = new NodoArbol(padre);
                        posiblesRaices.Add(padre); 
                    }

                    // Obtiene y limpia los nombres de los hijos
                    var hijos = partes[1].Split(',')
                                      .Select(h => h.Trim())  // Elimina espacios
                                      .Where(h => !string.IsNullOrEmpty(h))  // Ignora vacíos
                                      .ToArray();

                    // Procesa cada hijo
                    foreach (var hijo in hijos)
                    {
                        // Crea un nodo hijo si no existe
                        if (!diccionarioNodos.ContainsKey(hijo))
                        {
                            diccionarioNodos[hijo] = new NodoArbol(hijo);
                        }
                        // Remueve hijo de posibles raíces 
                        posiblesRaices.Remove(hijo);
                    }
                }
                else
                {
                    // Advierte sobre líneas con formato incorrecto (sin ':' o con múltiples ':')
                    Console.WriteLine($"Línea ignorada (formato incorrecto): {linea}");
                }
            }

            // Establece relaciones padre-hijo
            foreach (var linea in lineas)
            {
                var partes = linea.Split(':');
                if (partes.Length == 2)
                {
                    string padre = partes[0].Trim();
                    
                    // Obtiene hijos de esta línea
                    var hijos = partes[1].Split(',')
                                      .Select(h => h.Trim())
                                      .Where(h => !string.IsNullOrEmpty(h))
                                      .ToArray();

                    // Obtiene nodo padre del diccionario
                    var nodoPadre = diccionarioNodos[padre];
                    
                    // Conecta cada hijo con su padre
                    foreach (var hijo in hijos)
                    {
                        var nodoHijo = diccionarioNodos[hijo];
                        nodoPadre.AgregarHijo(nodoHijo); // Establece relación padre-hijo
                        Console.WriteLine($"Conectado: {padre} -> {hijo}");
                    }
                }
            }

            // ESTABLECER LA RAÍZ DEL ÁRBOL 
            if (posiblesRaices.Count == 1)
            {
                // Solo una raíz posible encontrada
                arbol.Raiz = diccionarioNodos[posiblesRaices.First()];
                Console.WriteLine($"Raíz establecida: {arbol.Raiz.Valor}");
            }
            else if (posiblesRaices.Count > 1)
            {
                // Caso con múltiples raíces posibles 
                Console.WriteLine($"Advertencia: Múltiples raíces posibles. Usando: {posiblesRaices.First()}");
                arbol.Raiz = diccionarioNodos[posiblesRaices.First()];
            }
            else
            {
                // Error no se pudo determinar la raíz 
                throw new InvalidOperationException("No se pudo determinar la raíz del árbol. Verifica el formato del archivo.");
            }

            // Muestra estadísticas finales del árbol cargado
            Console.WriteLine($"Árbol cargado exitosamente. Nodos totales: {arbol.ContarNodos()}");
            return arbol;
        }

        // Método auxiliar para crear archivos de ejemplo con contenido predefinido
        public static void CrearArchivoEjemplo(string rutaArchivo, string contenido)
        {
            // Obtiene el nombre delirectorio del archivo
            var directorio = Path.GetDirectoryName(rutaArchivo);

            // Crea el directorio si no existe
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            // Escribe contenido en el archivo
            File.WriteAllText(rutaArchivo, contenido);
            Console.WriteLine($"Archivo de ejemplo creado: {Path.GetFileName(rutaArchivo)}");
        }
    }
}
