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

namespace HackAtHome.Entities
{
    public class ResultInfo
    {
        public Status Status { get; set; }

        public string Token { get; set; } // El Token expiera depsues de 10min del ultimo acceso al servicio REST

        public string FullName { get; set; }
    }
}