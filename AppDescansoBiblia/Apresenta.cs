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
using System.Threading;
using Android.Content.PM;

namespace AppDescansoBiblia
{
    [Activity(Theme = "@style/Descansoo.Theme", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class Apresenta : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Apresentação em 4 segundos
            Thread.Sleep(1000);

            //Chamada da Aplicação
            StartActivity(typeof(MainActivity));

        }
    }
}