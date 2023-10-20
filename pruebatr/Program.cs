using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace LevantaServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configura el puerto en el que escuchará el servidor
            int serverPort = 8080;

            // Configura el servidor HTTP para escuchar en el puerto
            string serverUrl = $"http://localhost:{serverPort}/";
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(serverUrl);
            listener.Start();

            Console.WriteLine($"Iniciando servidor en {serverUrl}");

            while (true)
            {
                // Esperar a que llegue una solicitud HTTP
                HttpListenerContext context = await listener.GetContextAsync();

                // Procesar la solicitud
                await ProcessRequest(context);
            }
        }

        static async Task ProcessRequest(HttpListenerContext context)
        {
            // Obtener la solicitud HTTP y el cuerpo del mensaje
            HttpListenerRequest request = context.Request;
            string requestBody = new System.IO.StreamReader(request.InputStream).ReadToEnd();

            // Parsear los datos del formulario
            //var formData = HttpUtility.ParseQueryString(requestBody);

            // Verificar si los campos requeridos están presentes
            string account_id = "tci5c009981eef0f7e351d83290b362ca70";
            string custom_type = "juliancarvajalegc@gmail.com";

            // Verificar si se proporcionaron account_id y custom_type
            if (!string.IsNullOrEmpty(account_id) && !string.IsNullOrEmpty(custom_type))
            {
                // Realizar la solicitud POST a Truora
                string truoraApiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiIiwiYWRkaXRpb25hbF9kYXRhIjoie30iLCJjbGllbnRfaWQiOiJUQ0kzY2EzNDFjNGQ5Njc2MDQ2ZjI2ZDFmOGJkMDQyMDBjNyIsImV4cCI6MzI3MzU4ODMwNCwiZ3JhbnQiOiIiLCJpYXQiOjE2OTY3ODgzMDQsImlzcyI6Imh0dHBzOi8vY29nbml0by1pZHAudXMtZWFzdC0xLmFtYXpvbmF3cy5jb20vdXMtZWFzdC0xXzZRcXBPblF2NyIsImp0aSI6IjM0ZTRhYjc3LTQxYjMtNDIxMy04N2Q4LWZhMDI5M2FmOGQyYSIsImtleV9uYW1lIjoicHJ1ZWJhX3RlY25pY2FfaGFtYWwiLCJrZXlfdHlwZSI6ImJhY2tlbmQiLCJ1c2VybmFtZSI6InRydW9yYW5hb3MtcHJ1ZWJhX3RlY25pY2FfaGFtYWwifQ.gpyfPT-DprI3s3Fah7--46sr-ZkGWduJx3L_b9zmWA4";
                string truoraApiUrl = "https://api.validations.truora.com/v1/validations";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Truora-API-Key", truoraApiKey);

                    var posData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("type", "document-validation"),
                        new KeyValuePair<string, string>("country", "CO"),
                        new KeyValuePair<string, string>("document_type", "national-id"),
                        new KeyValuePair<string, string>("user_authorized", "true"),
                        new KeyValuePair<string, string>("account_id", account_id),
                        new KeyValuePair<string, string>("custom_type", custom_type)
                    };
                    HttpContent contentdos = new FormUrlEncodedContent(posData);

                    // Realizar la solicitud POST
                    var response = await client.PostAsync(truoraApiUrl, contentdos);

                    if (response.IsSuccessStatusCode)
                    {
                        // Leer la respuesta como cadena
                        string responseContent = await response.Content.ReadAsStringAsync();

                        // Deserializar la respuesta JSON
                        var responseObject = JsonSerializer.Deserialize<TruoraResponse>(responseContent);

                        // Acceder a los campos que necesitas
                        string validationId = responseObject.validation_id;
                        string frontUrl = responseObject.instructions.front_url;
                        string reverseUrl = responseObject.instructions.reverse_url;

                        using (var putClient = new HttpClient())
                        {
                            // Configura la URL de la solicitud PUT con la variable frontUrl
                            string putUrl = frontUrl;

                            // Carga el archivo .jpeg como datos binarios
                            byte[] fileBytes = File.ReadAllBytes("C:\\Users\\Julian Carvajal\\Documents\\AppsJuli\\frontok.jpeg");
                            HttpContent putContent = new ByteArrayContent(fileBytes);

                            // Realiza la solicitud PUT
                            var putResponse = await putClient.PutAsync(putUrl, putContent);

                            if (putResponse.IsSuccessStatusCode)
                            {
                                // La solicitud PUT se realizó con éxito
                                // Puedes procesar la respuesta si es necesario
                            }
                            else
                            {
                                // La solicitud PUT falló, puedes manejar el error adecuadamente
                                context.Response.StatusCode = 500; // Código de error "Internal Server Error"
                                context.Response.Close();
                            }
                        }

                        using (var putClient2 = new HttpClient())
                        {
                            string putUrl2 = reverseUrl; // Reemplaza "SEGUNDA_URL_PUT" con la URL adecuada
                            byte[] fileBytes2 = File.ReadAllBytes("C:\\Users\\Julian Carvajal\\Documents\\AppsJuli\\reverseok.jpeg");
                            HttpContent putContent2 = new ByteArrayContent(fileBytes2);
                            var putResponse2 = await putClient2.PutAsync(putUrl2, putContent2);

                            if (putResponse2.IsSuccessStatusCode)
                            {
                                // La segunda solicitud PUT se realizó con éxito
                            }
                            else
                            {
                                context.Response.StatusCode = 500;
                                context.Response.Close();
                            }
                        }

                        using (var getClient = new HttpClient())
                        {
                            getClient.DefaultRequestHeaders.Add("Truora-API-Key", truoraApiKey);
                            string getUrl = $"https://api.validations.truora.com/v1/validations/{validationId}";

                            // Realizar la solicitud GET
                            var getResponse = await getClient.GetAsync(getUrl);

                            if (getResponse.IsSuccessStatusCode)
                            {
                                // Leer la respuesta como cadena
                                string getResponseContent = await getResponse.Content.ReadAsStringAsync();

                                // Puedes deserializar la respuesta JSON si es necesario
                                var validationData = JsonSerializer.Deserialize<ValidationData>(getResponseContent);

                                // Enviar la respuesta al cliente
                                string jsonResponse = getResponseContent;
                                context.Response.ContentType = "application/json";
                                context.Response.StatusCode = 200;
                                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse);
                                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                context.Response.StatusCode = 500; // O maneja el error adecuadamente                              
                            }
                        }
                        context.Response.Close();
                    }
                    else
                    {
                        // Enviar una respuesta de error al cliente
                        context.Response.StatusCode = (int)response.StatusCode;
                        context.Response.Close();
                    }
                }
            }
            else
            {
                // Enviar una respuesta al cliente indicando que faltan los campos requeridos
                context.Response.StatusCode = 400; // Código de error "Bad Request"
                context.Response.Close();
            }
        }
    }
}

public class RequestModel
{
    public string account_id { get; set; }
    public string custom_type { get; set; }
    // Otros campos según tus necesidades
}

public class TruoraResponse
{
    public string validation_id { get; set; }

    public TruoraInstructions instructions { get; set; }
}

public class TruoraInstructions
{
    public string front_url { get; set; }
    public string reverse_url { get; set; }
}

public class ValidationData
{
    // Define las propiedades necesarias para la respuesta de validación completa
}