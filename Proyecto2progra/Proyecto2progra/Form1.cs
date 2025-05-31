using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Proyecto2progra
{
    public partial class Form1 : Form
    {
        private readonly string apiKeyClima = "INSERTAR CLAVE";
        private readonly string apiKeyOpenAI = "INSETAR CLAVE";
        private readonly string connectionString = "Server=MSI\\SQLEXPRESS;Database=RecomendadorCultivos;Trusted_Connection=True;";

        private int areaTerreno = 0;    // Guardo el área del terreno
        // Variables para el clima y la ubicación detectados por el sistema
        private string climaActual = "";
        private string temperaturaActual = "";
        private string ubicacionDetectadaCompleta = ""; // Para la API del clima
        private string ubicacionParaMostrar = "";      // Para mostrar al usuario

        public Form1()
        {
            InitializeComponent();
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            cmbAmbiente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAmbiente.Items.AddRange(new string[] { "Seco", "Húmedo", "Tropical", "Frío", "Cálido", "Montañoso", "Costero" });

            // El campo de ubicación siempre será de solo lectura, lo llena el sistema.
            textUbicacion.ReadOnly = true;

            // Configuro el TextBox para comentarios extra.
            txtComentarioExtra.Multiline = true;
            txtComentarioExtra.ScrollBars = ScrollBars.Vertical;
        }

        private async void btnDetectarUbicacion_Click(object sender, EventArgs e)
        {
            btnDetectarUbicacion.Enabled = false;
            btnDetectarUbicacion.Text = "Detectando...";
            textUbicacion.Text = "Detectando ubicación..."; // Muestro un mensaje mientras detecto

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // 1. Detección de ubicación por IP
                    string urlIpApi = "http://ip-api.com/json/";
                    string responseIp = await client.GetStringAsync(urlIpApi);
                    dynamic dataIp = JsonConvert.DeserializeObject(responseIp);

                    string status = dataIp.status;
                    if (status == "success")
                    {
                        string ciudad = dataIp.city;
                        string paisCode = dataIp.countryCode;
                        string regionName = dataIp.regionName;

                        // Guardo la ubicación para la API del clima
                        ubicacionDetectadaCompleta = $"{ciudad},{paisCode}";
                        // Guardo la ubicación para mostrarla
                        ubicacionParaMostrar = $"{ciudad}, {regionName}";
                        textUbicacion.Text = ubicacionParaMostrar; // Muestro la ubicación

                        // 2. Obtengo el clima para la ubicación detectada
                        string urlClima = $"https://api.openweathermap.org/data/2.5/weather?q={ubicacionDetectadaCompleta}&appid={apiKeyClima}&units=metric&lang=es";
                        string responseClima = await client.GetStringAsync(urlClima);
                        dynamic dataClima = JsonConvert.DeserializeObject(responseClima);

                        // Almaceno el clima y la temperatura
                        climaActual = dataClima.weather[0].description;
                        temperaturaActual = dataClima.main.temp + "°C";

                        MessageBox.Show($"Ubicación detectada: {textUbicacion.Text}\nClima: {climaActual}, Temp: {temperaturaActual}", "Detección Exitosa");
                    }
                    else
                    {
                        MessageBox.Show("No pude detectar la ubicación automáticamente.", "Error de Detección");
                        textUbicacion.Text = "No detectada";
                        // Mantengo textUbicacion.ReadOnly en true, no permito escribir manualmente.
                        // Limpio mis variables de clima si la detección falló
                        climaActual = "";
                        temperaturaActual = "";
                        ubicacionDetectadaCompleta = "";
                        ubicacionParaMostrar = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al detectar la ubicación o el clima: " + ex.Message + "\nPor favor, verifica tu conexión a internet.", "Error de Conexión/API");
                textUbicacion.Text = "Error al detectar";
                // Mantengo textUbicacion.ReadOnly en true, no permito escribir manualmente.
                // Limpio mis variables de clima si hubo un error
                climaActual = "";
                temperaturaActual = "";
                ubicacionDetectadaCompleta = "";
                ubicacionParaMostrar = "";
            }
            finally
            {
                btnDetectarUbicacion.Enabled = true;
                btnDetectarUbicacion.Text = "Detectar Ubicación";
            }
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            string ambiente = cmbAmbiente.Text;
            string fecha = dtpFecha.Value.ToShortDateString();
            string medidas = txtMedidasTerreno.Text.Trim();
            string comentarioExtra = txtComentarioExtra.Text.Trim();

            // Valido que la ubicación esté lista y no muestre mensajes de detección/error
            if (string.IsNullOrWhiteSpace(textUbicacion.Text) || textUbicacion.Text.Contains("Detectando") || textUbicacion.Text.Contains("Error"))
            {
                MessageBox.Show("Por favor, detecta tu ubicación antes de consultar.", "Ubicación Requerida");
                return;
            }

            if (string.IsNullOrWhiteSpace(ambiente))
            {
                MessageBox.Show("Por favor, selecciona el ambiente.", "Datos Incompletos");
                return;
            }

            if (dtpFecha.Value.Date < DateTime.Today)
            {
                MessageBox.Show("La fecha de siembra no puede ser en el pasado.", "Fecha Inválida");
                return;
            }

            string[] dimensiones = medidas.Split('x');
            if (dimensiones.Length != 2 ||
                !int.TryParse(dimensiones[0], out int largo) ||
                !int.TryParse(dimensiones[1], out int ancho))
            {
                MessageBox.Show("Ingresa las medidas del terreno en el formato correcto (Ej: 10x15). Primer número es largo, segundo es ancho.", "Formato Incorrecto");
                return;
            }

            areaTerreno = largo * ancho;

            btnConsultar.Enabled = false;
            btnConsultar.Text = "Consultando...";

            try
            {
                using HttpClient client = new();

                // Valido que el clima y la temperatura tengan valores.
                if (string.IsNullOrEmpty(climaActual) || string.IsNullOrEmpty(temperaturaActual))
                {
                    MessageBox.Show("No pude obtener el clima para tu ubicación. Haz clic en 'Detectar Ubicación' nuevamente.", "Clima No Disponible");
                    btnConsultar.Enabled = true;
                    btnConsultar.Text = "Consultar Recomendación";
                    return;
                }

                // Extrae el valor numérico de la temperatura para un mejor procesamiento por la IA
                double tempValue;
                string tempNumericPart = temperaturaActual.Replace("°C", "").Trim();
                if (!double.TryParse(tempNumericPart, out tempValue))
                {
                    Console.WriteLine("Advertencia: No se pudo parsear la temperatura numérica. Usando el formato de cadena completo.");
                    tempValue = -999;
                }

               
                string prompt = $"Estoy sembrando en {ubicacionParaMostrar}, Guatemala, el día {fecha}. El clima es {climaActual}, con una temperatura de {temperaturaActual}. " +
                                $"El ambiente clasificado por el usuario es '{ambiente}'. " +
                                $"El terreno mide {largo} metros de largo por {ancho} metros de ancho, con un área total de {areaTerreno} m². " +
                                "Basado en estas condiciones específicas de temperatura, clima, ambiente y tamaño del terreno en la región de Jutiapa, Guatemala, " +
                                "por favor, proporciona una **lista de 3 a 5 cultivos altamente recomendados y viables**. " +
                                "Para los **primeros 2 cultivos de la lista (los más recomendados)**, incluye la siguiente información detallada y estructurada en párrafos separados, con sus encabezados. Asegúrate de incluir cálculos aproximados de la cantidad de plantas y distancias de siembra basados en el área del terreno proporcionada:\n\n" +
                                "--- Cultivo [Nombre del Cultivo 1] ---\n\n" +
                                "Cultivo Recomendado: [Nombre del Cultivo 1]\n\n" +
                                "Cantidad Aproximada de Plantas: [Cálculo y estimación basado en {areaTerreno} m² y la distancia de siembra recomendada]\n\n" +
                                "Distancia de Siembra: [Ej. XX cm entre plantas, YY cm entre surcos]\n\n" +
                                "Condiciones Ideales del Suelo: [Descripción]\n\n" +
                                "Cuidados Específicos (Paso a Paso Detallado): [Instrucciones paso a paso]\n\n" +
                                "Plagas y Enfermedades Comunes y su Control: [Descripción y soluciones]\n\n" +
                                "Productos Recomendados (con marca y cuáles son los mejores en calidad, si es posible): [Ej. Fertilizante X (Marca Y), Plaguicida Z (Marca W)]\n\n" +
                                "Tiempo de Cosecha Estimado: [Estimación en días/meses]\n\n" +
                                "Consejos Adicionales para el Éxito: [Recomendaciones extra]\n\n" +
                                "--- Cultivo [Nombre del Cultivo 2] ---\n\n" +
                                "Cultivo Recomendado: [Nombre del Cultivo 2]\n\n" +
                                "Cantidad Aproximada de Plantas: [Cálculo y estimación basado en {areaTerreno} m² y la distancia de siembra recomendada]\n\n" +
                                "Distancia de Siembra: [Ej. XX cm entre plantas, YY cm entre surcos]\n\n" +
                                "Condiciones Ideales del Suelo: [Descripción]\n\n" +
                                "Cuidados Específicos (Paso a Paso Detallado): [Instrucciones paso a paso]\n\n" +
                                "Plagas y Enfermedades Comunes y su Control: [Descripción y soluciones]\n\n" +
                                "Productos Recomendados (con marca y cuáles son los mejores en calidad, si es posible): [Ej. Fertilizante X (Marca Y), Plaguicida Z (Marca W)]\n\n" +
                                "Tiempo de Cosecha Estimado: [Estimación en días/meses]\n\n" +
                                "Consejos Adicionales para el Éxito: [Recomendaciones extra]\n\n" +
                                "--- Otros Cultivos Viables ---\n\n" +
                                "Además de los cultivos anteriores, podrías considerar:\n" +
                                "- [Cultivo 3]: [Breve razón de por qué es viable]\n" +
                                "- [Cultivo 4]: [Breve razón de por qué es viable]\n" +
                                "- [Cultivo 5]: [Breve razón de por qué es viable]\n\n" +
                                "Asegúrate de que la respuesta sea práctica, fácil de entender y directamente aplicable para agricultores del campo. " +
                                "Prioriza cultivos que sean realistas y rentables para la región de Jutiapa y sus condiciones típicas, especialmente aquellos que prosperen en temperaturas de {temperaturaActual}. " +
                                "Sé tan detallado como sea posible sin ser redundante. " +
                                (string.IsNullOrWhiteSpace(comentarioExtra) ? "" : $"\nInformación adicional proporcionada por el usuario: {comentarioExtra}");


                var request = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] { new { role = "user", content = prompt } },
                    max_tokens = 2000, // incrementar tokens para permitir respuestas más largas
                    temperature = 0.7
                };

                string body = JsonConvert.SerializeObject(request);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKeyOpenAI);
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic resultado = JsonConvert.DeserializeObject(responseJson);

                string recomendacion = resultado.choices[0].message.content;

                // Valido si la respuesta tiene los detalles esperados
                if (!ValidarRespuesta(recomendacion))
                {
                    MessageBox.Show("La respuesta generada no incluye todos los detalles esperados. Intenta consultar nuevamente o ajusta el prompt.", "Respuesta Incompleta");
                    txtResultado.Text = "La recomendación generada puede estar incompleta:\n\n" + recomendacion; // Muestro la respuesta aunque esté incompleta
                }
                else
                {
                    txtResultado.Text = $"Clima: {climaActual}, Temperatura: {temperaturaActual}\n\nTerreno: {largo}m x {ancho}m ({areaTerreno} m²)\n\nRecomendación:\n\n{recomendacion}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message + "\nVerifica tu conexión a internet o el estado de las APIs.", "Error de Consulta");
            }
            finally
            {
                btnConsultar.Enabled = true;
                btnConsultar.Text = "Consultar Recomendación";
            }
        }

        // Valido que la respuesta contenga las palabras clave esperadas.
        private bool ValidarRespuesta(string texto)
        {
            texto = texto.ToLower();
            // Busco los encabezados clave para asegurar la completitud de la respuesta,
            // adaptando para múltiples cultivos.
            return texto.Contains("cultivo recomendado:") &&
                   texto.Contains("cantidad aproximada de plantas:") &&
                   texto.Contains("distancia de siembra:") &&
                   texto.Contains("cuidados específicos") &&
                   (texto.Contains("plagas y enfermedades") || texto.Contains("control de plagas")) &&
                   texto.Contains("productos recomendados") &&
                   texto.Contains("tiempo de cosecha estimado:");
            // We might need to make this validation more robust if the AI doesn't strictly follow the "--- Cultivo X ---" format.
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string ubicacion = ubicacionParaMostrar; // Uso la ubicación que detecté/mostré
            DateTime fecha = dtpFecha.Value;
            string ambiente = cmbAmbiente.Text;
            string recomendacion = txtResultado.Text;
            string medidasTexto = txtMedidasTerreno.Text.Trim();

            // Valido las medidas del terreno
            string[] dimensiones = medidasTexto.Split('x');
            if (dimensiones.Length != 2 ||
                !int.TryParse(dimensiones[0], out int largo) ||
                !int.TryParse(dimensiones[1], out int ancho))
            {
                MessageBox.Show("Ingresa las medidas del terreno en el formato correcto (Ej: 10x15).", "Formato Incorrecto");
                return;
            }

            int area = largo * ancho;
            string descripcionMedidas = $"{largo} metros x {ancho} metros";
            string descripcionArea = $"{area} m²";  // Área con unidad para guardar en la BD

            // Valido que todos los datos necesarios estén antes de guardar.
            if (string.IsNullOrWhiteSpace(ubicacion) || string.IsNullOrWhiteSpace(ambiente) ||
                string.IsNullOrWhiteSpace(recomendacion) || string.IsNullOrWhiteSpace(medidasTexto) || area == 0 ||
                string.IsNullOrWhiteSpace(climaActual) || string.IsNullOrWhiteSpace(temperaturaActual))
            {
                MessageBox.Show("Faltan datos. Asegúrate de haber ingresado las medidas del terreno, detectado la ubicación y realizado una consulta antes de guardar.", "Datos Incompletos");
                return;
            }

            try
            {
                using SqlConnection conn = new(connectionString);
                conn.Open();

                string sql = "INSERT INTO Consultas (Ubicacion, FechaSiembra, Ambiente, Clima, Recomendacion, MedidasTerreno, AreaTerreno) " +
                             "VALUES (@ubicacion, @fecha, @ambiente, @clima, @recomendacion, @medidasTerreno, @areaTerreno)";
                using SqlCommand cmd = new(sql, conn);
                cmd.Parameters.AddWithValue("@ubicacion", ubicacion);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@ambiente", ambiente);
                // Guardo el clima y temperatura reales detectados.
                cmd.Parameters.AddWithValue("@clima", $"{climaActual}, {temperaturaActual}");
                cmd.Parameters.AddWithValue("@recomendacion", recomendacion);
                cmd.Parameters.AddWithValue("@medidasTerreno", descripcionMedidas);
                cmd.Parameters.AddWithValue("@areaTerreno", descripcionArea);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Consulta guardada exitosamente.", "Guardado Exitoso");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al guardar en base de datos: " + ex.Message, "Error de Base de Datos");
            }
        }

        // Limpio el formulario.
        private void buttLimpiar_Click(object sender, EventArgs e)
        {
            // Limpio los campos de texto
            txtMedidasTerreno.Text = "";
            txtComentarioExtra.Text = "";
            txtResultado.Text = "";

            // Restablezco el ComboBox
            cmbAmbiente.SelectedIndex = -1;
            cmbAmbiente.Text = "";

            // Restablezco la fecha a hoy
            dtpFecha.Value = DateTime.Today;

            // Limpio mis variables de clima y ubicación
            climaActual = "";
            temperaturaActual = "";
            ubicacionDetectadaCompleta = "";
            ubicacionParaMostrar = "";
            areaTerreno = 0;

            // Restablezco el campo de ubicación a su estado inicial (solo lectura y vacío)
            textUbicacion.Text = "";
            textUbicacion.ReadOnly = true;
        }
    }
}