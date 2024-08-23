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
    [Activity(Label = "Configurações")]

    
    public class Configurar : Activity
    {
        string sValor = "";

        [Export("btnSalvarClicked")]
        public void btnSalvarClicked_Click(View v)
        {

            var activity2 = new Intent(this, typeof(MainActivity));
            activity2.PutExtra("MyData", sValor);

            StartActivity(activity2);
            
            //Fechar a tela atual
            //this.Finish();
        }

        

        private void RdGeralClick(object sender, EventArgs e)
        {
            
            RadioButton Proverbios = FindViewById<RadioButton>(Resource.Id.rdProverbios);
            RadioButton Salmos = FindViewById<RadioButton>(Resource.Id.rdSalmos);
            RadioButton NovoTestamento = FindViewById<RadioButton>(Resource.Id.rdNovoTestamento);
            RadioButton AntigoTestamento = FindViewById<RadioButton>(Resource.Id.rdAntigo);

            sValor = "Geral";

            Salmos.Checked = false;
            Proverbios.Checked = false;
            NovoTestamento.Checked = false;
            AntigoTestamento.Checked = false;

            RadioButton rb = (RadioButton)sender;
            
            // Toast.MakeText(this, "Selecionar Geral", ToastLength.Short).Show();
        }

        private void RdSalmosClick(object sender, EventArgs e)
        {
            RadioButton Geral = FindViewById<RadioButton>(Resource.Id.rdGeral);
            RadioButton Proverbios = FindViewById<RadioButton>(Resource.Id.rdProverbios);
            RadioButton NovoTestamento = FindViewById<RadioButton>(Resource.Id.rdNovoTestamento);
            RadioButton AntigoTestamento = FindViewById<RadioButton>(Resource.Id.rdAntigo);

            sValor = "Salmos";

            Geral.Checked = false;
            Proverbios.Checked = false;
            NovoTestamento.Checked = false;
            AntigoTestamento.Checked = false;

            RadioButton rb = (RadioButton)sender;
            //Toast.MakeText(this, "Selecionar Salmos", ToastLength.Short).Show();
        }

        private void RdProverbiosClick(object sender, EventArgs e)
        {

            RadioButton Salmos = FindViewById<RadioButton>(Resource.Id.rdSalmos);
            RadioButton Geral = FindViewById<RadioButton>(Resource.Id.rdGeral);
            RadioButton NovoTestamento = FindViewById<RadioButton>(Resource.Id.rdNovoTestamento);
            RadioButton AntigoTestamento = FindViewById<RadioButton>(Resource.Id.rdAntigo);

            sValor = "Proverbios";

            Geral.Checked = false;
            Salmos.Checked = false;
            NovoTestamento.Checked = false;
            AntigoTestamento.Checked = false;


            RadioButton rb = (RadioButton)sender;
            //Toast.MakeText(this, "Selecionar Proverbios", ToastLength.Short).Show();
        }
        private void RdNovoTestamentoClick(object sender, EventArgs e)
        {

            RadioButton Salmos = FindViewById<RadioButton>(Resource.Id.rdSalmos);
            RadioButton Geral = FindViewById<RadioButton>(Resource.Id.rdGeral);
            RadioButton Proverbios = FindViewById<RadioButton>(Resource.Id.rdProverbios);
            RadioButton AntigoTestamento = FindViewById<RadioButton>(Resource.Id.rdAntigo);

            sValor = "NovoTestamento";

            Geral.Checked = false;
            Salmos.Checked = false;
            Proverbios.Checked = false;
            AntigoTestamento.Checked = false;

            RadioButton rb = (RadioButton)sender;
            //Toast.MakeText(this, "Selecionar Proverbios", ToastLength.Short).Show();
        }

        private void RdAntigoTestamentoClick(object sender, EventArgs e)
        {

            RadioButton Salmos = FindViewById<RadioButton>(Resource.Id.rdSalmos);
            RadioButton Geral = FindViewById<RadioButton>(Resource.Id.rdGeral);
            RadioButton Proverbios = FindViewById<RadioButton>(Resource.Id.rdProverbios);
            RadioButton NovoTestamento = FindViewById<RadioButton>(Resource.Id.rdNovoTestamento);

            sValor = "AntigoTestamento";

            Geral.Checked = false;
            Salmos.Checked = false;
            Proverbios.Checked = false;
            NovoTestamento.Checked = false;


            RadioButton rb = (RadioButton)sender;
            //Toast.MakeText(this, "Selecionar Proverbios", ToastLength.Short).Show();
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
                SetContentView(Resource.Layout.Configurar_800);

            }
            else
            {
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Configurar);
            }

            var TxtConfigurar = FindViewById<TextView>(Resource.Id.textView88);
            var TelaSobre = FindViewById<LinearLayout>(Resource.Id.linearLayoutConf);

            TxtConfigurar.SetTextColor(Android.Graphics.Color.Yellow);
            TelaSobre.SetBackgroundColor(Android.Graphics.Color.Blue);

            RadioButton Geral = FindViewById<RadioButton>(Resource.Id.rdGeral);
            RadioButton Salmos = FindViewById<RadioButton>(Resource.Id.rdSalmos);
            RadioButton Proverbios = FindViewById<RadioButton>(Resource.Id.rdProverbios);
            RadioButton NovoTestamento = FindViewById<RadioButton>(Resource.Id.rdNovoTestamento);
            RadioButton AntigoTestamento = FindViewById<RadioButton>(Resource.Id.rdAntigo);

            Geral.Click += RdGeralClick;
            Salmos.Click += RdSalmosClick;
            Proverbios.Click += RdProverbiosClick;
            NovoTestamento.Click += RdNovoTestamentoClick;
            AntigoTestamento.Click += RdAntigoTestamentoClick;

        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

    }    

}