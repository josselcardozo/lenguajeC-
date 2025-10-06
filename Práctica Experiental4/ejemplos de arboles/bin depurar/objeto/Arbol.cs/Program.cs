using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Practico4
{
    class Program
    {
        // Entrada principal de la aplicación
        static void Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("    PRACTICO 4 - GRAFICAS DE ARBOLES");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            try
            {
                // Crea el directorio para almacenar los ejemplos
                string directorioEjemplos = "EjemplosArboles";
                CrearDirectorioSeguro(directorioEjemplos);

                // Muestra el menú principal al usuario
                MostrarMenuPrincipal(directorioEjemplos);
            }
            catch (Exception ex)
            {
                // Muestra errores no controlados
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                // Pausa antes de cerrar la aplicación
                Console.WriteLine("\nPresiona cualquier tecla para salir...");
                Console.ReadKey();
            }
        }

        // Crea un directorio si no existe, con manejo de errores
        static void CrearDirectorioSeguro(string directorio)
        {
            try
            {
                // Verifica si el directorio no existe
                if (!Directory.Exists(directorio))
                {
                    // Crea el directorio
                    Directory.CreateDirectory(directorio);
                    Console.WriteLine("Directorio creado: " + directorio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo crear el directorio " + directorio + ": " + ex.Message);
            }
        }

        // Muestra el menú principal y maneja la navegación del usuario
        static void MostrarMenuPrincipal(string directorio)
        {
            int opcion;
            do
            {
                // Limpia la consola y muestra opciones
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("          MENU PRINCIPAL - ARBOLES");
                Console.WriteLine("==========================================");
                Console.WriteLine("1. Generar Estructura Organizacional");
                Console.WriteLine("2. Generar Arbol de Decision de Creditos");
                Console.WriteLine("3. Cargar datos desde archivo TXT existente");
                Console.WriteLine("4. Ver archivos generados");
                Console.WriteLine("5. Salir");
                Console.WriteLine("==========================================");
                Console.Write("Selecciona una opcion (1-5): ");

                // Lee y valida la opción del usuario
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    // Ejecuta la acción según la opción seleccionada
                    switch (opcion)
                    {
                        case 1:
                            MenuEstructuraOrganizacional(directorio);
                            break;
                        case 2:
                            MenuArbolDecision(directorio);
                            break;
                        case 3:
                            CargarDesdeArchivoExistente(directorio);
                            break;
                        case 4:
                            MostrarArchivosGenerados(directorio);
                            break;
                        case 5:
                            Console.WriteLine("Hasta pronto!");
                            break;
                        default:
                            Console.WriteLine("Opcion no valida.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no valida.");
                }

                if (opcion != 5)
                {
                    Console.WriteLine("Presiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 5); // Repite el buclehasta que el usuario elija salir
        }

        // Muestra el menú para estructura organizacional
        static void MenuEstructuraOrganizacional(string directorio)
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("     ESTRUCTURA ORGANIZACIONAL");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Usar ejemplo predefinido");
            Console.WriteLine("2. Ingresar datos manualmente");
            Console.WriteLine("3. Volver al menu principal");
            Console.Write("Selecciona una opcion (1-3): ");

            // Lee y valida la opción del usuario
            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        GenerarEjemploOrganizacionalPredefinido(directorio);
                        break;
                    case 2:
                        GenerarEstructuraOrganizacionalManual(directorio);
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Opcion no valida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada no valida.");
            }
        }

        // Genera un ejemplo predefinido de estructura organizacional
        static void GenerarEjemploOrganizacionalPredefinido(string directorio)
        {
            Console.WriteLine("\nGENERANDO EJEMPLO PREDEFINIDO...");
            Console.WriteLine("----------------------------------------");

            // Datos predefinidos para estructura organizacional
            string datosOrganizacional = @"CEO: Director TI, Director Marketing, Director Ventas
Director TI: Gerente Desarrollo, Gerente Infraestructura
Director Marketing: Gerente Publicidad, Gerente Investigacion
Director Ventas: Gerente Regional Norte, Gerente Regional Sur
Gerente Desarrollo: Team Lead Frontend, Team Lead Backend
Gerente Infraestructura: Admin Redes, Admin Servidores
Gerente Publicidad: Especialista SEO, Disenador Grafico
Gerente Investigacion: Analista Mercado
Gerente Regional Norte: Vendedor A, Vendedor B
Gerente Regional Sur: Vendedor C, Vendedor D
Team Lead Frontend: Desarrollador React, Desarrollador Angular
Team Lead Backend: Desarrollador C#, Desarrollador Java";

            // Ruta del archivo donde se guardarán los datos
            string archivoDatos = Path.Combine(directorio, "datos_organizacional_predefinido.txt");
            
            // Guardar datos en archivo
            File.WriteAllText(archivoDatos, datosOrganizacional);
            Console.WriteLine("Datos guardados en: " + archivoDatos);

            // Generar la imagen del árbol
            GenerarImagenDesdeArchivo(archivoDatos, "Estructura Organizacional Predefinida", 
                                    Path.Combine(directorio, "arbol_organizacional_predefinido.png"));
        }

        // Permite al usuario ingresar datos manualmente para estructura organizacional
        static void GenerarEstructuraOrganizacionalManual(string directorio)
        {
            Console.WriteLine("\nINGRESO MANUAL DE ESTRUCTURA ORGANIZACIONAL");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Instrucciones:");
            Console.WriteLine("- Ingresa cada relacion en formato: 'puesto: subordinado1, subordinado2'");
            Console.WriteLine("- Ejemplo: 'CEO: Director TI, Director Marketing'");
            Console.WriteLine("- Escribe 'FIN' para terminar la entrada");
            Console.WriteLine("-----------------------------------------");

            // Lista para almacenar las relaciones ingresadas
            List<string> relaciones = new List<string>();
            string entrada;

            // Bucle para capturar todas las relaciones
            do
            {
                Console.Write("Ingresa relacion (o 'FIN' para terminar): ");
                entrada = Console.ReadLine()?.Trim();

                // Valida que la entrada no esté vacía y no sea "FIN"
                if (!string.IsNullOrEmpty(entrada) && entrada.ToUpper() != "FIN")
                {
                    // Verifica el formato válido
                    if (entrada.Contains(':'))
                    {
                        relaciones.Add(entrada);
                        Console.WriteLine("Relacion agregada: " + entrada);
                    }
                    else
                    {
                        Console.WriteLine("Formato incorrecto. Usa: 'puesto: subordinado1, subordinado2'");
                    }
                }

            } while (entrada?.ToUpper() != "FIN");

            // Verifica que se hayan ingresado relaciones
            if (relaciones.Count > 0)
            {
                // Une todas las relaciones con saltos de línea
                string datosCompletos = string.Join(Environment.NewLine, relaciones);
                
                // Ruta para guardar los datos manuales
                string archivoDatos = Path.Combine(directorio, "datos_organizacional_manual.txt");
                File.WriteAllText(archivoDatos, datosCompletos);
                
                Console.WriteLine("Datos guardados en: " + archivoDatos);
                Console.WriteLine("Total de relaciones ingresadas: " + relaciones.Count);

                // Genera la imagen del árbol
                GenerarImagenDesdeArchivo(archivoDatos, "Estructura Organizacional Personalizada", 
                                        Path.Combine(directorio, "arbol_organizacional_manual.png"));
            }
            else
            {
                Console.WriteLine("No se ingresaron datos. No se genero ningun arbol.");
            }
        }

        // Muestra el menú para árbol de decisión
        static void MenuArbolDecision(string directorio)
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("     ARBOL DE DECISION - CREDITOS");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Usar ejemplo predefinido");
            Console.WriteLine("2. Ingresar datos manualmente");
            Console.WriteLine("3. Volver al menu principal");
            Console.Write("Selecciona una opcion (1-3): ");

            // Lee y valida la opción del usuario
            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        GenerarEjemploDecisionPredefinido(directorio);
                        break;
                    case 2:
                        GenerarArbolDecisionManual(directorio);
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Opcion no valida.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada no valida.");
            }
        }

        // Genera un ejemplo predefinido de árbol de decisión
        static void GenerarEjemploDecisionPredefinido(string directorio)
        {
            Console.WriteLine("\nGENERANDO ARBOL DE DECISION PREDEFINIDO...");
            Console.WriteLine("--------------------------------");

            // Datos predefinidos para árbol de decisión
            string datosDecision = @"Solicitar Prestamo: Ingresos Altos, Ingresos Bajos
Ingresos Altos: Buen Credito, Mal Credito
Ingresos Bajos: Garantia Suficiente, Sin Garantia
Buen Credito: APROBADO
Mal Credito: RECHAZADO
Garantia Suficiente: APROBADO con Garantia
Sin Garantia: RECHAZADO";

            // Ruta del archivo donde se guardarán los datos
            string archivoDatos = Path.Combine(directorio, "datos_decision_predefinido.txt");
            
            // Guardar datos en archivo
            File.WriteAllText(archivoDatos, datosDecision);
            Console.WriteLine("Datos guardados en: " + archivoDatos);

            // Genera la imagen del árbol
            GenerarImagenDesdeArchivo(archivoDatos, "Arbol de Decision - Evaluacion de Creditos", 
                                    Path.Combine(directorio, "arbol_decision_predefinido.png"));
        }

        // Permite al usuario ingresar datos manualmente para árbol de decisión
        static void GenerarArbolDecisionManual(string directorio)
        {
            Console.WriteLine("\nINGRESO MANUAL DE ARBOL DE DECISION");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Instrucciones:");
            Console.WriteLine("- Ingresa cada decision en formato: 'condicion: resultado1, resultado2'");
            Console.WriteLine("- Ejemplo: 'Solicitar Prestamo: Ingresos Altos, Ingresos Bajos'");
            Console.WriteLine("- Para resultados finales usa: 'condicion: APROBADO, RECHAZADO'");
            Console.WriteLine("- Escribe 'FIN' para terminar la entrada");
            Console.WriteLine("-----------------------------------------");

            // Lista para almacenar las decisiones ingresadas
            List<string> decisiones = new List<string>();
            string entrada;

            // Bucle para capturar todas las decisiones
            do
            {
                Console.Write("Ingresa decision (o 'FIN' para terminar): ");
                entrada = Console.ReadLine()?.Trim();

                // Valida que la entrada no esté vacía y no sea "FIN"
                if (!string.IsNullOrEmpty(entrada) && entrada.ToUpper() != "FIN")
                {
                    // Verifica el formato válido
                    if (entrada.Contains(':'))
                    {
                        decisiones.Add(entrada);
                        Console.WriteLine("Decision agregada: " + entrada);
                    }
                    else
                    {
                        Console.WriteLine("Formato incorrecto. Usa: 'condicion: opcion1, opcion2'");
                    }
                }

            } while (entrada?.ToUpper() != "FIN");

            // Verifica que se hayan ingresado decisiones
            if (decisiones.Count > 0)
            {
                // Une todas las decisiones con saltos de línea
                string datosCompletos = string.Join(Environment.NewLine, decisiones);
                
                // Ruta para guardar los datos manuales
                string archivoDatos = Path.Combine(directorio, "datos_decision_manual.txt");
                File.WriteAllText(archivoDatos, datosCompletos);
                
                Console.WriteLine("Datos guardados en: " + archivoDatos);
                Console.WriteLine("Total de decisiones ingresadas: " + decisiones.Count);

                // Genera la imagen del árbol
                GenerarImagenDesdeArchivo(archivoDatos, "Arbol de Decision Personalizado", 
                                        Path.Combine(directorio, "arbol_decision_manual.png"));
            }
            else
            {
                Console.WriteLine("No se ingresaron datos. No se genero ningun arbol.");
            }
        }

        // Permite cargar datos desde un archivo TXT existente
        static void CargarDesdeArchivoExistente(string directorio)
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("     CARGAR DATOS DESDE ARCHIVO TXT");
            Console.WriteLine("==========================================");
            
            // Muestra archivos TXT disponibles en el directorio
            string[] archivosTxt = Directory.GetFiles(directorio, "*.txt");
            
            if (archivosTxt.Length == 0)
            {
                Console.WriteLine("No se encontraron archivos TXT en el directorio.");
                Console.WriteLine("Directorio: " + directorio);
                return;
            }

            Console.WriteLine("Archivos TXT disponibles:");
            for (int i = 0; i < archivosTxt.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + Path.GetFileName(archivosTxt[i]));
            }

            Console.Write("\nSelecciona el numero del archivo a cargar (1-" + archivosTxt.Length + "): ");

            // Lee y valida la selección del usuario
            if (int.TryParse(Console.ReadLine(), out int seleccion) && seleccion >= 1 && seleccion <= archivosTxt.Length)
            {
                string archivoSeleccionado = archivosTxt[seleccion - 1];
                Console.WriteLine("Archivo seleccionado: " + Path.GetFileName(archivoSeleccionado));

                // Pide al usuario un nombre para el árbol
                Console.Write("Ingresa un nombre para el arbol: ");
                string nombreArbol = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(nombreArbol))
                {
                    nombreArbol = "Arbol desde " + Path.GetFileName(archivoSeleccionado);
                }

                // Genera un nombre para el archivo de imagen
                string nombreArchivoImagen = Path.GetFileNameWithoutExtension(archivoSeleccionado) + "_generado.png";
                string archivoImagen = Path.Combine(directorio, nombreArchivoImagen);

                // Genera la imagen del árbol
                GenerarImagenDesdeArchivo(archivoSeleccionado, nombreArbol, archivoImagen);
            }
            else
            {
                Console.WriteLine("Seleccion no valida.");
            }
        }

        // Método reutilizable para generar imágenes desde archivos de datos
        static void GenerarImagenDesdeArchivo(string archivoDatos, string nombreArbol, string archivoImagen)
        {
            try
            {
                Console.WriteLine("Cargando datos desde: " + Path.GetFileName(archivoDatos));

                // Carga el árbol desde archivo usando el CargadorDatosArbol
                var arbol = CargadorDatosArbol.CargarDesdeArchivo(archivoDatos, nombreArbol);

                // Crea un generador de gráficos
                var generador = new GeneradorArbolGrafico();
                
                // Genera la imagen
                var imagen = generador.GenerarImagenArbol(arbol, 1200, 800);
                
                // Guarda la imagen en formato PNG
                imagen.Save(archivoImagen, System.Drawing.Imaging.ImageFormat.Png);

                Console.WriteLine("Arbol generado: " + Path.GetFileName(archivoImagen));
                Console.WriteLine("Estadisticas: " + arbol.ContarNodos() + " nodos, altura " + arbol.CalcularAltura());
            }
            catch (Exception ex)
            {
                // Muestra un error detallado si falla la generación
                Console.WriteLine("Error al generar el arbol: " + ex.Message);
            }
        }

        // Método para mostrar archivos generados en el directorio
        static void MostrarArchivosGenerados(string directorio)
        {
            Console.Clear();
            Console.WriteLine("==========================================");
            Console.WriteLine("        ARCHIVOS GENERADOS");
            Console.WriteLine("==========================================");

            // Verifica si el directorio existe
            if (Directory.Exists(directorio))
            {
                // Obtiene todos los archivos del directorio
                string[] archivos = Directory.GetFiles(directorio);
                
                if (archivos.Length > 0)
                {
                    // Muestra cada archivo con su información
                    foreach (string archivo in archivos)
                    {
                        FileInfo info = new FileInfo(archivo);
                        string tipo = archivo.EndsWith(".png") ? "[IMAGEN] " : "[TEXTO]  ";
                        Console.WriteLine(tipo + info.Name + " (" + info.Length / 1024 + " KB)");
                    }
                    
                    Console.WriteLine("\nUbicacion: " + Path.GetFullPath(directorio));
                }
                else
                {
                    Console.WriteLine("No se encontraron archivos generados.");
                }
            }
            else
            {
                Console.WriteLine("El directorio no existe.");
            }
        }
    }
}
