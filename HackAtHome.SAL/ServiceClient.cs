using System;
using System.Text;

using HackAtHome.Entities;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HackAtHome.SAL
{
    public class ServiceClient
    {
        /// <summary>
        /// Realiza la autenticacion al servicio Web API
        /// </summary>
        /// <param name="studentEmail">Correo del usuario</param>
        /// <param name="studentPassword">Password del usuario</param>
        /// <returns>Objeto Resultinfo con los datos del usuario y un token de autenticacion</returns>
        public async Task<ResultInfo> AutenticateAsync(string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;

            // Direccion Base de Datos de la Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";

            // ID del diplomado
            string EventID = "xamarin30";

            string RequestUri = "api/evidence/Authenticate";

            // El servicio requiere un objeto UserInfo con los datos del usuario y el evento
            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };

            // Utilizamos el objeto System.Net.HttpClient para consumir el servicio REST
            using(var Client = new HttpClient())
            {
                // Establecemos la direccion base del servicio REST
                Client.BaseAddress = new Uri(WebAPIBaseAddress);

                // Limpiamos Encabezados de la peticion
                Client.DefaultRequestHeaders.Accept.Clear();

                // Indicamos al Servicio que envie los datos en formato JSON
                Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    // Serializamos en formato JSON
                    var JSONUserInfo = JsonConvert.SerializeObject(User);

                    // Hacemos una peticion POST al servicio enviando el objeto JSON
                    HttpResponseMessage Response = await Client.PostAsync(RequestUri, new StringContent(JSONUserInfo.ToString(), Encoding.UTF8, "application/json"));

                    // Leemos el resultado devuelto
                    var ResultWebApi = await Response.Content.ReadAsStringAsync();

                    // Deserializaos el resultado
                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebApi);
                }
                catch(Exception)
                {
                    // Manejo de la Excepcion
                    Result = new ResultInfo
                    {
                        FullName = "Ocurrio un error inesperado",
                        Status = Status.Error,
                        Token = ""
                    };
                }

                return Result;
            }
        }

        /// <summary>
        /// Obtiene el detalle de una evidencia
        /// </summary>
        /// <param name="token">Token de autenticacion del usuario</param>
        /// <returns>Una lista con las evidencias</returns>
        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;

            // Direccion base de la API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome";

            // URL de la evidencia
            string URI = $"{WebAPIBaseAddress}/api/evidence/getevidences?token={token}";

            // Utilizamos el objeto System.Net.HttpClient para consumir el servicio REST
            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    // Realizamos la peticion GET
                    HttpResponseMessage Response = await Client.GetAsync(URI);                    

                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Deserializaos el resultado
                        var ResultWebApi = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebApi);
                    }
                }
                catch (Exception)
                {
                    // Manejo de la Excepcion
                    Evidences = new List<Evidence>();
                }

                return Evidences;
            }
        }

        /// <summary>
        /// Obtiene el detalle de una evidencia
        /// </summary>
        /// <param name="token">Tokend de autenticacion del usuario</param>
        /// <param name="evidenceID">Identificador de la evidencia</param>
        /// <returns>Informacion de la evidencia</returns>
        public async Task<EvidenceDetail> GetEvidenceByIdAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;

            // Direccion base de la API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome";

            // URL de la evidencia
            string URI = $"{WebAPIBaseAddress}/api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            // Utilizamos el objeto System.Net.HttpClient para consumir el servicio REST
            using (var Client = new HttpClient())
            { 
                Client.DefaultRequestHeaders.Accept.Clear(); 
                Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    // Realizamos la peticion GET
                    HttpResponseMessage Response = await Client.GetAsync(URI);
                    var ResultWebApi = await Response.Content.ReadAsStringAsync();

                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Deserializaos el resultado
                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebApi);
                    }
                }
                catch (Exception)
                {
                    // Manejo de la Excepcion
                    Result = new EvidenceDetail
                    {
                        Description = "Ocurrio un error inesperado",
                        Url = ""
                    };
                }

                return Result;
            }
        }
    }
}