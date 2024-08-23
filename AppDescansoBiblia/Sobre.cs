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
using Java.Interop;

namespace AppDescansoBiblia
{
    [Activity(Label = "Sobre o Descanso Bíblia")]
    public class Sobre : Activity
    {
        

        [Export("btnFecharClicked")]
        public void btnFecharClicked_Click(View v)
        {

            //Fechar a tela atual
            this.Finish();
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            //detectar o tamanho da tela em uso
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            // Se for um Nexus S - Resolução 480 x 800
            if (widthInDp == 329 && heightInDp == 501)
            {
                SetContentView(Resource.Layout.Sobre_800);

            }
            else
            {
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Sobre);
            }

            var TxtDesenvolvedor = FindViewById<TextView>(Resource.Id.textView1);
            var TxtNomeDesenvolvedor = FindViewById<TextView>(Resource.Id.textView2);
            var TxtLocal = FindViewById<TextView>(Resource.Id.textView3);
            var TxtVersiculo = FindViewById<TextView>(Resource.Id.textView4);
            var TxtVersiculo2 = FindViewById<TextView>(Resource.Id.textView5);
            var TxtVersiculo3 = FindViewById<TextView>(Resource.Id.textView7);
            var TelaSobre = FindViewById<LinearLayout>(Resource.Id.linearLayoutSobre);


            TelaSobre.SetBackgroundColor(Android.Graphics.Color.Blue);
            TxtNomeDesenvolvedor.SetTextColor(Android.Graphics.Color.Orange);
            TxtLocal.SetTextColor(Android.Graphics.Color.Yellow);

            TxtDesenvolvedor.SetTextColor(Android.Graphics.Color.Yellow);
            TxtVersiculo.SetTextColor(Android.Graphics.Color.Orange);
            TxtVersiculo2.SetTextColor(Android.Graphics.Color.Orange);
            TxtVersiculo3.SetTextColor(Android.Graphics.Color.Yellow);
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}