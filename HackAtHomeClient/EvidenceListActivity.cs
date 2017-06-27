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

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidenceListActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EvidenceList);
            string FullName = Intent.GetStringExtra("FullName");
            string Token = Intent.GetStringExtra("Token");

            var FullNameTextView = FindViewById<TextView>(Resource.Id.textViewFullName);
            FullNameTextView.Text = FullName;

            LoadEvidenceList(Token);
        }

        private async void LoadEvidenceList(string Token)
        {
            var ServiceClient = new ServiceClient();
            try
            {
                var EvidenceList = await ServiceClient.GetEvidencesAsync(Token);
                var EvidenceListView = FindViewById<ListView>(Resource.Id.listViewEvidence);
                EvidenceListView.Adapter = new EvidenceAdapter(
                    this, EvidenceList.ToList(), Resource.Layout.EvidenceListItem,
                    Resource.Id.textViewEvidenceTitle, Resource.Id.textViewEvidenceStatus
                );
            }
            catch(Exception ex)
            {
                Android.Util.Log.Debug("H@H", ex.Message);
            }            
        }
    }
}