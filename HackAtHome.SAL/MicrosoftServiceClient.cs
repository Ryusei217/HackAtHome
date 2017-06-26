using Microsoft.WindowsAzure.MobileServices;
using HackAtHome.Entities;
using System.Threading.Tasks;

namespace HackAtHome.SAL
{
    public class MicrosoftServiceClient
    {
        // Cliente para acceder al servicio Mobile
        MobileServiceClient Client;

        // Objeto para realizar operaciones con tablas de Mobile Service
        private IMobileServiceTable<LabItem> LabItemTable;

        public async Task SendEvidence(LabItem userEvidence)
        {
            Client = new MobileServiceClient(@"http://xamarin-diplomado.azurewebsites.net/");
            LabItemTable = Client.GetTable<LabItem>();
            await LabItemTable.InsertAsync(userEvidence);
        }
    }
}