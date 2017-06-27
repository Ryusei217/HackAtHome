using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.CustomAdapters;
using HackAtHome.SAL;
using HackAtHomeClient.Fragments;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidenceListActivity : Activity
    {
        private EvidenceFragment Data;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EvidenceList);            
                       
            // Utiliza el Fragment Manager para recuperar el fragmento
            Data = (EvidenceFragment)this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                // No ha sido almacenado, agregar el fragmento a la Activity
                Data = new EvidenceFragment();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
            }

            LoadData();            
        }

        protected override void OnPause()
        {
            // Guardamos el estado del ListView antes de pausar la aplicacion
            var EvidenceListView = FindViewById<ListView>(Resource.Id.listViewEvidence);
            Data.EvidenceListState = EvidenceListView.OnSaveInstanceState();
            base.OnPause();            
        }

        /// <summary>
        /// Cargamos la informacion en Data para no perder la informacion.
        /// </summary>
        private async void LoadData()
        {
            // Si no existe un token intentamos recargar la informacion
            if (string.IsNullOrWhiteSpace(Data.Token))
            {
                // Recuperamos la informaicon pasada desde la otra actividad
                Data.FullName = Intent.GetStringExtra("FullName");
                Data.Token = Intent.GetStringExtra("Token");

                // Recuperamos la lista de las evidencias
                var ServiceClient = new ServiceClient();

                try
                {
                    Data.EvidenceList = await ServiceClient.GetEvidencesAsync(Data.Token);
                }
                catch (Exception ex)
                {
                    // Creamos un dialogo para mostrar la excepcion
                    ErrorDialog(ex.Message);
                    Data.EvidenceList = new List<HackAtHome.Entities.Evidence>();
                }
            }            

            // Cargamos la informacion en el layout
            var FullNameTextView = FindViewById<TextView>(Resource.Id.textViewFullName);
            FullNameTextView.Text = Data.FullName;

            // Cargamos la informacion en el ListView
            var EvidenceListView = FindViewById<ListView>(Resource.Id.listViewEvidence);
            EvidenceListView.Adapter = new EvidenceAdapter(
                this, Data.EvidenceList, Resource.Layout.EvidenceListItem,
                Resource.Id.textViewEvidenceTitle, Resource.Id.textViewEvidenceStatus
            );

            // Restauramos el estado del ListView si existe
            if(Data.EvidenceListState != null)
            {
                EvidenceListView.OnRestoreInstanceState(Data.EvidenceListState);
            }
        }

        /// <summary>
        /// Genera un dialog de error para la aplicacion
        /// </summary>
        /// <param name="title">Titulo a proporicionar </param>
        /// <param name="iconResource">Icono del dialogo</param>
        /// <param name="message">Mensaje a mostrar</param>
        private void ErrorDialog(string title = "Error", int iconResource = Resource.Drawable.hath_icon, string message = "Ocurrio un error inesperado")
        {
            Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
            AlertDialog Alert = Builder.Create();
            Alert.SetTitle(title);
            Alert.SetIcon(iconResource);
            Alert.SetMessage($"Ocurrio un error inesperado:\n{message}");
            Alert.SetButton("Ok", (s, ev) => { });
            Alert.Show();
        }
    }
}