using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/hath_icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var validateButton = FindViewById<Button>(Resource.Id.buttonValidate);
            validateButton.Click += (s, ev) =>
            {
                Validate();
            };
        }

        private async void Validate()
        {
            ServiceClient ServiceClient = new ServiceClient();
            var emailEditText = FindViewById<EditText>(Resource.Id.editTextEmail);
            var passwordEditText = FindViewById<EditText>(Resource.Id.editTextPassword);

            ResultInfo Result = await ServiceClient.AutenticateAsync(emailEditText.Text, passwordEditText.Text);

            Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
            AlertDialog Alert = Builder.Create();
            Alert.SetTitle("Resultado de la Verificación");
            Alert.SetIcon(Resource.Drawable.Icon);
            Alert.SetMessage($"{Result.Status}\n{Result.FullName}\n{Result.Token}");
            Alert.SetButton("Ok", (s, ev) => { });
            Alert.Show();
        }
    }
}

