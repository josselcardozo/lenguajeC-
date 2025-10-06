using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Practico4
{
    // Clase responsable de generar la representación visual del árbol como imagen
    public class GeneradorArbolGrafico
    {
        // CONSTANTES DE DISEÑO definen el aspecto visual del árbol
        private const int ANCHO_NODO = 100;           // Ancho en píxeles de cada nodo
        private const int ALTO_NODO = 40;             // Alto en píxeles de cada nodo
        private const int ESPACIO_HORIZONTAL = 120;   // Espacio horizontal entre nodos hermanos
        private const int ESPACIO_VERTICAL = 80;      // Espacio vertical entre niveles del árbol
        private const int MARGEN = 50;                // Margen alrededor de la imagen

        // Método principal genera la imagen completa del árbol
        public Bitmap GenerarImagenArbol(Arbol arbol, int ancho = 1000, int alto = 700)
        {
            // Crea una nueva imagen con las dimensiones especificadas
            var bitmap = new Bitmap(ancho, alto);
            
            // Usa Graphics para dibujar en el bitmap
            using (var graphics = Graphics.FromImage(bitmap))
            {
                // Configurar gráficos para alta calidad
                graphics.Clear(Color.White);                          // Fondo blanco
                graphics.SmoothingMode = SmoothingMode.AntiAlias;     // Suavizado de bordes

                // Verificar que el árbol tenga una raíz
                if (arbol.Raiz != null)
                {
                    // Calcula posiciones de todos los nodos en el espacio 2D
                    CalcularPosiciones(arbol.Raiz, 0, ancho, 0);
                    
                    // Dibuja título del árbol en la parte superior
                    DibujarTitulo(graphics, arbol.Nombre, ancho);

                    // Dibuja conexiones entre nodos (líneas con flechas)
                    DibujarConexiones(graphics, arbol.Raiz);

                    // Dibuja los nodos (cajas con texto)
                    DibujarNodos(graphics, arbol.Raiz);

                    // Dibuja información del árbol
                    DibujarInformacion(graphics, arbol, ancho, alto);
                }
                else
                {
                    // Mostrar mensaje si el árbol está vacío
                    DibujarMensajeVacio(graphics, ancho, alto);
                }
            }

            // Retornar la imagen generada lista para guardar
            return bitmap;
        }

        // Método para calcular las posiciones de todos los nodos
        private void CalcularPosiciones(NodoArbol nodo, int nivel, int anchoDisponible, int xInicio)
        {
            // Si el nodo es nulo, terminar recursión
            if (nodo == null) return;

            // Calcular coordenada Y según el nivel del árbol
            int y = MARGEN + 80 + nivel * ESPACIO_VERTICAL;
            
            // Nodo hoja (sin hijos)
            if (nodo.Hijos.Count == 0)
            {
                // Posiciona elnodo hoja en el centro del espacio disponible
                int x = xInicio + anchoDisponible / 2;
                nodo.Posicion = new Point(x, y);  
                
                // Calcula el área rectangular del nodo
                nodo.Bounds = new Rectangle(x - ANCHO_NODO / 2, y - ALTO_NODO / 2, 
                                          ANCHO_NODO, ALTO_NODO);
                return;
            }

            // Nodo con hijos divide el espacio horizontal entre los hijos
            int espacioPorHijo = anchoDisponible / nodo.Hijos.Count;
            int xActual = xInicio; // Posición X inicial para el primer hijo

            // Calcula posiciones de todos los hijos recursivamente
            foreach (var hijo in nodo.Hijos)
            {
                CalcularPosiciones(hijo, nivel + 1, espacioPorHijo, xActual);
                xActual += espacioPorHijo; // Mueve a la siguienteposición para el siguiente hijo
            }

            // Posiciona el nodo padre CENTRADO sobre sus hijos
            if (nodo.Hijos.Count > 0)
            {
                // Calcula el punto medio entre el primer y último hijo
                int xPadre = (nodo.Hijos.First().Posicion.X + nodo.Hijos.Last().Posicion.X) / 2;
                nodo.Posicion = new Point(xPadre, y);
                nodo.Bounds = new Rectangle(xPadre - ANCHO_NODO / 2, y - ALTO_NODO / 2, 
                                          ANCHO_NODO, ALTO_NODO);
            }
        }

        // Dibuja el título del árbol en la parte superior de la imagen
        private void DibujarTitulo(Graphics graphics, string titulo, int ancho)
        {
            // Configura fuente y color para el título
            var fontTitulo = new Font("Arial", 16, FontStyle.Bold);
            var brushTitulo = new SolidBrush(Color.DarkBlue);
            
            // Mide el tamaño del texto para centrarlo
            var sizeTitulo = graphics.MeasureString(titulo, fontTitulo);

            // Dibuja el título centrado en la parte superior
            graphics.DrawString(titulo, fontTitulo, brushTitulo,
                new PointF(ancho / 2 - sizeTitulo.Width / 2, 20));

            // Libera recursos para evitar fugas de memoria
            fontTitulo.Dispose();
            brushTitulo.Dispose();
        }

        // Dibuja las líneas de conexión entre nodos padres e hijos
        private void DibujarConexiones(Graphics graphics, NodoArbol nodo)
        {
            // nodo nulo
            if (nodo == null) return;

            // Dibuja línea desde este nodo a cada uno de sus hijos
            foreach (var hijo in nodo.Hijos)
            {
                // Crea el lápiz para las líneas (color gris, grosor 2)
                var pen = new Pen(Color.Gray, 2)
                {
                    EndCap = LineCap.ArrowAnchor,  // Punta de flecha al final de la línea
                    CustomEndCap = new AdjustableArrowCap(4, 4) // Tamaño de la flecha
                };

                // Dibujar línea desde la parte inferior del padre hasta la superior del hijo
                graphics.DrawLine(pen, 
                    nodo.Posicion.X, nodo.Posicion.Y + ALTO_NODO / 2,  // Desde (parte inferior del padre)
                    hijo.Posicion.X, hijo.Posicion.Y - ALTO_NODO / 2); // Hasta (parte superior del hijo)

                // Libera recursos del lápiz
                pen.Dispose();

                // Llama recursivamente para dibujar conexiones de los hijos
                DibujarConexiones(graphics, hijo);
            }
        }

        // Dibuja todos los nodos del árbol 
        private void DibujarNodos(Graphics graphics, NodoArbol nodo)
        {
            // nodo nulo
            if (nodo == null) return;

            // Dibuja todos los hijos para que los padres queden sobre las líneas
            foreach (var hijo in nodo.Hijos)
            {
                DibujarNodos(graphics, hijo);
            }

            // Dibuja el nodo actual para que quede sobre las líneas de conexión
            DibujarNodoIndividual(graphics, nodo);
        }

        // Dibuja un nodo individual con gradiente y bordes redondeados
        private void DibujarNodoIndividual(Graphics graphics, NodoArbol nodo)
        {
            // Obtiene el área rectangular del nodo
            var rect = nodo.Bounds;
            
            // Crea efecto de gradiente para el fondo del nodo
            var gradientBrush = new LinearGradientBrush(
                rect,                           // Área a llenar
                Color.LightSkyBlue,             // Color inicial (azul claro)
                Color.DodgerBlue,               // Color final (azul más oscuro)
                45f);                           // Ángulo del gradiente (45 grados)

            // Dibuja el nodo con bordes redondeados
            DibujarNodoRedondeado(graphics, rect, gradientBrush, 10);

            // Configura el estilo para eltexto del nodo
            var font = new Font("Arial", 9, FontStyle.Bold);
            var brushTexto = new SolidBrush(Color.White);

            // Mide el tamaño del texto para centrarlo dentro del nodo
            var sizeTexto = graphics.MeasureString(nodo.Valor, font);
            
            // Calcula la posición centrada para el texto
            var posTexto = new PointF(
                nodo.Posicion.X - sizeTexto.Width / 2,  // Centrado horizontal
                nodo.Posicion.Y - sizeTexto.Height / 2  // Centrado vertical
            );

            // Dibuja el texto del nodo
            graphics.DrawString(nodo.Valor, font, brushTexto, posTexto);

            // Libera los recursos utilizados
            gradientBrush.Dispose();
            font.Dispose();
            brushTexto.Dispose();
        }

        // Dibuja un nodo con esquinas redondeadas usando GraphicsPath
        private void DibujarNodoRedondeado(Graphics graphics, Rectangle rect, Brush brush, int radius)
        {
            // Crea una ruta gráfica para la forma redondeada
            GraphicsPath path = new GraphicsPath();
            
            // Construye un rectángulo con esquinas redondeadas 
            path.AddLine(rect.Left + radius, rect.Top, rect.Right - radius, rect.Top);
            // Esquina superior derecha
            path.AddArc(rect.Right - radius, rect.Top, radius, radius, 270, 90);
            // Lado derecho
            path.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom - radius);
            // Esquina inferior derecha 
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            // Lado inferior
            path.AddLine(rect.Right - radius, rect.Bottom, rect.Left + radius, rect.Bottom);
            // Esquina inferior izquierda 
            path.AddArc(rect.Left, rect.Bottom - radius, radius, radius, 90, 90);
            // Lado izquierdo
            path.AddLine(rect.Left, rect.Bottom - radius, rect.Left, rect.Top + radius);
            // Esquina superior izquierda 
            path.AddArc(rect.Left, rect.Top, radius, radius, 180, 90);
            
            path.CloseFigure(); // Cierra la figura 

            // Rellena la forma con el brush (gradiente)
            graphics.FillPath(brush, path);

            // Dibuja el borde de la forma
            var pen = new Pen(Color.DarkBlue, 2);
            graphics.DrawPath(pen, path);

            // Libera los recursos
            pen.Dispose();
            path.Dispose();
        }

        // Dibuja la información del árbol en la esquina inferior
        private void DibujarInformacion(Graphics graphics, Arbol arbol, int ancho, int alto)
        {
            // Configura la fuente para información
            var fontInfo = new Font("Arial", 10, FontStyle.Regular);
            var brushInfo = new SolidBrush(Color.DarkGreen);
            
            // Texto con estadísticas del árbol
            string info = $"Total Nodos: {arbol.ContarNodos()} | Altura: {arbol.CalcularAltura()}";
            
            // Mide el texto para posicionarlo correctamente
            var sizeInfo = graphics.MeasureString(info, fontInfo);

            // Dibuja la información en la esquina inferior derecha
            graphics.DrawString(info, fontInfo, brushInfo,
                new PointF(ancho - sizeInfo.Width - 10, alto - 30));

            // Libera los recursos
            fontInfo.Dispose();
            brushInfo.Dispose();
        }

        // Muestra mensaje cuando el árbol está vacío
        private void DibujarMensajeVacio(Graphics graphics, int ancho, int alto)
        {
            // Configura el estilo para el mensaje de error
            var font = new Font("Arial", 12, FontStyle.Bold);
            var brush = new SolidBrush(Color.Red);
            
            string mensaje = "ÁRBOL VACÍO - No hay datos para mostrar";

            // Mide el texto para centrarlo en la imagen
            var size = graphics.MeasureString(mensaje, font);

            // Dibuja el mensaje centrado
            graphics.DrawString(mensaje, font, brush,
                new PointF(ancho / 2 - size.Width / 2, alto / 2 - size.Height / 2));

            // Libera los recursos
            font.Dispose();
            brush.Dispose();
        }
    }
}
