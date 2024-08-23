using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.IO;
using System.Threading;
using System.Timers;
using Java.Interop;
using Android.Views;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Content.Res;
using Android.Media;
using static Android.Provider.MediaStore.Images;

[assembly: Permission(Name = "android.permission.READ_EXTERNAL_STORAGE")]
[assembly: Permission(Name = "android.permission.WRITE_EXTERNAL_STORAGE")]
namespace AppDescansoBiblia
{

    [Activity(Label="Versiculos Coloridos", Icon = "@drawable/Ichts_icon",ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        ImageView screenshotImage;
        public int count = 1;
        string sValor = "";
        public Button BotaoPrint;

        //Temporizador do sistema (ativa a cada 1 segundo)
        System.Timers.Timer objTimer = new System.Timers.Timer();
        protected override void OnCreate(Bundle bundle)
        {

            string text_Valor = Intent.GetStringExtra("MyData");

            sValor = text_Valor;
             
            base.OnCreate(bundle);

            //detectar o tamanho da tela em uso
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            // Se for um Nexus S - Resolução 480 x 800
            if (widthInDp == 329 && heightInDp == 501)
            {
                SetContentView(Resource.Layout.Main_800);
            }
            else
            {
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Main);
            }

            BotaoPrint = (Button)FindViewById(Resource.Id.BtnPrint);

            if (text_Valor == "Geral")
            {
                //Trocar o versículo
                MudarVersiculo();
            }           
            else if(text_Valor == "Salmos")
            {
                Salmos();
            }
            else if (text_Valor == "Proverbios")
            {
                Proverbios();
            }
            else if (text_Valor == "NovoTestamento")
            {
                NovoTestamento();
            }
            else if (text_Valor == "AntigoTestamento")
            {
                AntigoTestamento();
            }
            else
            {
                MudarVersiculo(); 
            }
            
            //Define o temporizador inicial em 5 segundos
            objTimer.Interval = 5000;
            objTimer.Elapsed += new ElapsedEventHandler(TimerTick);
            objTimer.Enabled = true;

            //Inicia o temporizador
            objTimer.Start();

            BotaoPrint.Click += (sender, e) =>
            {
                // esconder o Botão
                BotaoPrint.Visibility = ViewStates.Invisible;

                // desativar o timer que muda o versiculo
                objTimer.Enabled = false;

                // Tira um screenshot da tela
                takeScreenShot();

                // compartilhar a imagem


                // reativar o timer que muda o versiculo
                objTimer.Enabled = true;

                // mostrar o botão
                BotaoPrint.Visibility = ViewStates.Visible;


            };
                        
        }

        public void takeScreenShot()
        {
            // Define o diretório
            string path = System.IO.Path.Combine(Android.OS.Environment.DirectoryPictures, "Imagens");

            var basePath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;

            // Marca a tela a tirar o screenshot
            View v1 = Window.DecorView.RootView;
            v1.DrawingCacheEnabled = true;
            int w = v1.Width;
            int h = v1.Height;

            // Cria a imagem
            Android.Graphics.Bitmap bitmap = Android.Graphics.Bitmap.CreateBitmap(v1.GetDrawingCache(true));

            // Coloca um nome  na imagem
            Java.IO.File imageFile = new Java.IO.File(basePath, System.Environment.TickCount + ".jpg");

            // trata a qualidade da imagem        
            System.IO.MemoryStream bytes = new System.IO.MemoryStream();
            int quality = 100;

            // Verifica o diretório da imagem
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(basePath);
            if (!dir.Exists)
                dir.CreateSubdirectory(basePath);

            // Salva a imagem
            Java.IO.FileOutputStream fo;
            imageFile.CreateNewFile();
            fo = new Java.IO.FileOutputStream(imageFile);

            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, quality, bytes);
            fo.Write(bytes.ToArray());
            fo.Close();

            // Toast.MakeText(this, "Screenshot realizado com sucesso!!", ToastLength.Short).Show();

            string nomearquivo = imageFile.ToString();

            //ResizeImage(nomearquivo, 200, 300);
            //Toast.MakeText(this, "Imagem reduzida com sucesso!!", ToastLength.Short).Show();
            //Compartilhar a imagem                      
            // 31/05/2017 13:42h
            try
            {

                //Compartilhar 
                // 24/01/2018 13:43h - Leo Metelys
                Intent shareIntent = new Intent(Intent.ActionSend);
                shareIntent.SetType("image/*");
                Android.Net.Uri uri = Android.Net.Uri.Parse(nomearquivo);
                shareIntent.PutExtra(Intent.ExtraStream, uri);
                StartActivity(Intent.CreateChooser(shareIntent, "Versiculos coloridos"));

            }

            catch
            {
                Android.Widget.Toast.MakeText(this, "Sem conexão com a Internet...", Android.Widget.ToastLength.Short).Show();
            }

        }


        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private void TimerTick(object source, ElapsedEventArgs e)
        {
            var TxtVersiculo = FindViewById<TextView>(Resource.Id.TxtVersiculo);
            
            RunOnUiThread(delegate
            {

                if (sValor == "Geral")
                {
                    //Trocar o versículo
                    MudarVersiculo();
                }
                else if (sValor == "Salmos")
                {
                    Salmos();
                }
                else if (sValor == "Proverbios")
                {
                    Proverbios();
                }
                else if (sValor == "NovoTestamento")
                {
                    NovoTestamento();
                }
                else if (sValor == "AntigoTestamento")
                {
                    AntigoTestamento();
                }
                else
                {
                    MudarVersiculo();
                }

            });
        }

        // 11/11/2018 16:11h
        // Usando a Bíblia Sagrada RA - Almeida Revista e Atualiazada no Brasil, 2ª edição de 1993
        // Por Manoel Leonardo Metelis Florindo
        public void AntigoTestamento()
        {
            var TxtVersiculo = FindViewById<TextView>(Resource.Id.TxtVersiculo);
            var txtCapitulo = FindViewById<TextView>(Resource.Id.txtCapitulo);
            var Tela = FindViewById<LinearLayout>(Resource.Id.linearLayoutMain);

            // 'Pegar próximo número aleatório sendo gerado entre 0 e 39

            Random randNum = new Random();
            int NumVer = randNum.Next(40);

            // 11/11/2018 16:11h
            // Usando a Bíblia Sagrada RA - Almeida Revista e Atualiazada no Brasil, 2ª edição de 1993
            // Por Manoel Leonardo Metelis Florindo

            // Define o texto e o versículo de cada número aleatório gerado
            if (NumVer == 0)
            {
                TxtVersiculo.Text = "Não oprimirás o teu próximo, nem o roubarás";
                txtCapitulo.Text = "Levítico 19:13a";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.BlueViolet);
                txtCapitulo.SetTextColor(Android.Graphics.Color.BlueViolet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);

            }

            if (NumVer == 1)
            {
                TxtVersiculo.Text = "A paga do jornaleiro não ficará contigo até pela manhã";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Levítico 19:13b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 2)
            {
                TxtVersiculo.Text = "Não furtareis, nem mentireis, nem usareis de falsidade cada um com o seu próximo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Levítico 19:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 3)
            {
                TxtVersiculo.Text = "No princípio, criou Deus os céus e a terra";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Gênesis 1:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 4)
            {
                TxtVersiculo.Text = "Também disse Deus: Façamos o homem à nossa imagem, conforme a nossa semelhança";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGreen);
                txtCapitulo.Text = "Gênesis 1:26";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 5)
            {
                TxtVersiculo.Text = "Temos recebido o bem de Deus, e não receberíamos também o mal?";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Jó 2:10b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 6)
            {

                TxtVersiculo.Text = "Nu sai do ventre de minha mãe e nu voltarei; o SENHOR o deu e o SENHOR o tomou; bendito seja o nome do SENHOR";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtCapitulo.Text = "Jó 1:21";
                txtCapitulo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 7)
            {
                TxtVersiculo.Text = "De ti farei uma grande nação, e te abençoarei, e te engrandecerei o nome. Sê tu uma bênção!";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Gênesis 12:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 8)
            {

                TxtVersiculo.Text = "Eu sou o SENHOR, teu Deus, que te tirei da terra do Egito, da casa da servidão";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Êxodo 20:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 9)
            {
                TxtVersiculo.Text = "Lembra-te do dia de sábado, para o santificar";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Êxodo 20:8";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 10)
            {
                TxtVersiculo.Text = "Honra teu pai e tua mãe, para que se prolonguem os teus dias na terra que o SENHOR, teu Deus, te dá";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.RosyBrown);
                txtCapitulo.Text = "Êxodo 20:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.RosyBrown);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 11)
            {
                TxtVersiculo.Text = "Melhor é a boa fama do que o unguento precioso, e o dia da morte, melhor do que o dia do nascimento";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Aqua);
                txtCapitulo.Text = "Eclesiastes 7:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Aqua);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 12)
            {
                TxtVersiculo.Text = "Tudo  tem o seu tempo determinado, e há tempo para todo propósito debaixo do céu";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Eclesiastes 3:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 13)
            {
                TxtVersiculo.Text = "E o seu nome será: Maravilhoso Conselheiro, Deus Forte, Pai da Eternidade, Príncipe da Paz";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Isaías 9:6b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 14)
            {
                TxtVersiculo.Text = "Estas palavras que, hoje, te ordeno estarão no teu coração";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Deuteronômio 6:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 15)
            {
                TxtVersiculo.Text = "O SENHOR empobrece e enriquece; abaixa e também exalta";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Lime);
                txtCapitulo.Text = "1 Samuel 2:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Lime);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 16)
            {
                TxtVersiculo.Text = "O SENHOR é o que tira a vida e a dá; faz descer à sepultura e faz subir";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Orange);
                txtCapitulo.Text = "1 Samuel 2:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Orange);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 17)
            {
                TxtVersiculo.Text = "Portanto, não vos entristeçais, porque a alegria do SENHOR é a vossa força";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Neemias 8:10c";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 18)
            {
                TxtVersiculo.Text = "Buscai o SENHOR enquanto se pode achar, invocai-o enquanto está perto";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Isaías 55:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 19)
            {
                TxtVersiculo.Text = "Quem creu em nossa pregação? E a quem foi revelado o braço do SENHOR?";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Purple);
                txtCapitulo.Text = "Isaías 53:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Purple);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 20)
            {
                TxtVersiculo.Text = "Todos nós andávamos desgarrados como ovelhas; cada um se desviava pelo caminho, mas o SENHOR fez cair sobre ele a iniquidade de nós todos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Salmon);
                txtCapitulo.Text = "Isaías 53:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Salmon);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 21)
            {
                TxtVersiculo.Text = "As misericórdias do SENHOR são a causa de não sermos consumidos, porque as suas misericórdias não têm fim";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Silver);
                txtCapitulo.Text = "Lamentações 3:22";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Silver);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 22)
            {
                TxtVersiculo.Text = "O Senhor não rejeitará para sempre";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SkyBlue);
                txtCapitulo.Text = "Lamentações 3:31";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SkyBlue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 23)
            {
                TxtVersiculo.Text = "O SENHOR é a minha rocha, a minha cidaela, o meu libertador";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "2 Samuel 22:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 24)
            {
                TxtVersiculo.Text = "Ele foi oprimido e humilhado, mas não abriu a boca; como cordeiro foi levado ao matadouro; e, como ovelha muda perante os seus tosquiadores, ele não abriu a boca";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Isaías 53:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 25)
            {
                TxtVersiculo.Text = "Contudo, levou sobre si o pecado de muitos e pelos transgressores intercedeu";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Gold);
                txtCapitulo.Text = "Isaías 53:12c";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Gold);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 26)
            {
                TxtVersiculo.Text = "Porquanto derramou a sua alma na morte; foi contado com os transgressores";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.GreenYellow);
                txtCapitulo.Text = "Isaías 53:12b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.GreenYellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 27)
            {
                TxtVersiculo.Text = "Eu, eu mesmo, sou o que apago as tuas transgressões por amor de mim e dos teus pecados não me lembro";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.LawnGreen);
                txtCapitulo.Text = "Isaías 43:25";
                txtCapitulo.SetTextColor(Android.Graphics.Color.LawnGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 28)
            {
                TxtVersiculo.Text = "Invoco o SENHOR, digno de ser louvado, e serei salvo dos meus inimigos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.IndianRed);
                txtCapitulo.Text = "2 Samuel 22:4";
                txtCapitulo.SetTextColor(Android.Graphics.Color.IndianRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 29)
            {
                TxtVersiculo.Text = "Pois tenho guardado os caminhos do SENHOR e não me apartei perversamente do meu Deus";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Olive);
                txtCapitulo.Text = "2 Samuel 22:22";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Olive);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 30)
            {
                TxtVersiculo.Text = "Em vos converterdes e em sossegardes, está a vossa salvação; na tranquilidade e na confiança, a vossa força, mas não o quisestes";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SeaGreen);
                txtCapitulo.Text = "Isaías 30:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SeaGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 31)
            {
                TxtVersiculo.Text = "Porque os caminhos do SENHOR são retos, e os justos andarão neles";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Oséias 14:9b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 32)
            {
                TxtVersiculo.Text = "Eu é que sei que pensamentos tenho a vosso respeito, diz o SENHOR; pensamentos de paz e não de mal, para vos dar o fim que desejais";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Jeremias 29:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 33)
            {
                TxtVersiculo.Text = "Porque os meus pensamentos não são os vossos pensamentos, nem os vossos caminhos, os meus caminhos, diz o SENHOR";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Chocolate);
                txtCapitulo.Text = "Isaías 55:8";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Chocolate);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 34)
            {
                TxtVersiculo.Text = "Deus é a minha fortaleza e a minha força e ele perfeitamente desembaraça o meu caminho";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                txtCapitulo.Text = "2 Samuel 22:33";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 35)
            {
                TxtVersiculo.Text = "Vive o SENHOR, e bendita seja a minha Rocha! Exaltado seja o meu Deus, a Rocha da minha salvação!";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "2 Samuel 22:47";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 36)
            {
                TxtVersiculo.Text = "O SENHOR te abençoe e te guarde";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Números 6:24";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 37)
            {
                TxtVersiculo.Text = "O SENHOR faça resplandecer o rosto sobre ti e tenha misericórdia de ti";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Números 6:25";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 38)
            {
                TxtVersiculo.Text = "O SENHOR sobre ti levante o rosto e te dê a paz";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Números 6:26";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 39)
            {
                TxtVersiculo.Text = "Faz forte ao cansado e multiplica as forças ao que não tem nenhum vigor";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Isaías 40:29";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 40)
            {
                TxtVersiculo.Text = "Deposar-te-ei comigo para sempre; desposar-te-ei comigo em justiça, e em juízo, e em beniginidade, e em misericórdias";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Oséias 2:19";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            // Intervalo em segundo para o próximo versiculo
            objTimer.Interval = 6500;

        }

        // 11/11/2018 17:46h
        // Versiculos alterados para a versão da Almeida Revista e Atualizada 2ª Edição de 1993
        public void NovoTestamento()
        {
            var TxtVersiculo = FindViewById<TextView>(Resource.Id.TxtVersiculo);
            var txtCapitulo = FindViewById<TextView>(Resource.Id.txtCapitulo);
            var Tela = FindViewById<LinearLayout>(Resource.Id.linearLayoutMain);

            // 'Pegar próximo número aleatório sendo gerado entre 0 e 39

            Random randNum = new Random();
            int NumVer = randNum.Next(46);

            // Define o texto e o versículo de cada número aleatório gerado
            if (NumVer == 0)
            {
                TxtVersiculo.Text = "Eu sou o caminho, e a verdade, e a vida; ninguém vem ao Pai senão por mim"; 
                txtCapitulo.Text = "João 14:6";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.BlueViolet);
                txtCapitulo.SetTextColor(Android.Graphics.Color.BlueViolet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);

            }

            if (NumVer == 1)
            {
                TxtVersiculo.Text = "Ou fazei a árvore boa e o seu fruto bom ou a árvore má e o seu fruto mau; porque pelo fruto se conhece a árvore";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Mateus 12:33";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 2)
            {
                TxtVersiculo.Text = "Bem-aventurados os mansos, porque herdarão a terra";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Mateus 5:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 3)
            {
                TxtVersiculo.Text = "Bem-aventurados os limpos de coração, porque verão a Deus";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Mateus 5:8";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 4)
            {

                TxtVersiculo.Text = "Bem-aventurados os que choram, porque serão consolados";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGreen);
                txtCapitulo.Text = "Mateus 5:4";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 5)
            {
                TxtVersiculo.Text = "Bem-aventurados os pacificadores, porque serão chamados filhos de Deus";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Mateus 5:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 6)
            {

                TxtVersiculo.Text = "Perseverai na oração, vigiando com ações de graças";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtCapitulo.Text = "Colossenses 4:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 7)
            {
                TxtVersiculo.Text = "Evitai que alguém retribua a outrém mal por mal; pelo contrário, segui sempre o bem entre vós e para com todos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "1 Tessalonicenses 5:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 8)
            {

                TxtVersiculo.Text = "Orai sem cessar";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "1 Tessalonicenses 5:17 ";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 9)
            {
                TxtVersiculo.Text = "Combate o bom combate da fé. Toma posse da vida eterna, para a qual também foste chamado e de que fizeste a boa confissão perante muitas testemunhas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "1 Timóteo 6:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 10)
            {
                TxtVersiculo.Text = "Todavia, o Senhor é fiel; ele vos confirmará e guardará do Maligno";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.RosyBrown);
                txtCapitulo.Text = "2 Tessalonicenses 3:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.RosyBrown);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 11)
            {
                TxtVersiculo.Text = "Procura apresentar-te a Deus aprovado, como obreiro que não tem de que se envergonhar, que maneja bem a palavra da verdade";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Aqua);
                txtCapitulo.Text = "2 Timóteo 2:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Aqua);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 12)
            {
                TxtVersiculo.Text = "Porque Deus ama a quem dá com alegria";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "2 Coríntios 9:7b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 13)
            {
                TxtVersiculo.Text = "Nós amamos porque ele nos amou primeiro";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "1 João 4:19 ";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 14)
            {
                TxtVersiculo.Text = "Aquele que diz estar na luz e odeia a seu irmão, até agora, está nas trevas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "1 João 2:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 15)
            {
                TxtVersiculo.Text = "No mundo, passais por aflições; mas tende bom ânimo; eu venci o mundo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Lime);
                txtCapitulo.Text = "João 16:33b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Lime);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 16)
            {
                TxtVersiculo.Text = "O meu mandamento é este: que vos ameis uns aos outros, assim como eu vos amei";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Orange);
                txtCapitulo.Text = "João 15:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Orange);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 17)
            {
                TxtVersiculo.Text = "Vós sois meus amigos, se fazeis o que eu vos mando";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "João 15:14";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 18)
            {
                TxtVersiculo.Text = "O amor seja sem hipocrisia. Detestai o mal, apegando-vos ao bem";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Romanos 12:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 19)
            {
                TxtVersiculo.Text = "Eu e o Pai somos um";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Purple);
                txtCapitulo.Text = "João 10:30";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Purple);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 20)
            {
                TxtVersiculo.Text = "Eu sou o bom pastor. O bom pastor dá a vida pelas ovelhas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Salmon);
                txtCapitulo.Text = "João 10:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Salmon);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 21)
            {
                TxtVersiculo.Text = "Se, pois, o Filho vos libertar, verdadeiramente sereis livres";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Silver);
                txtCapitulo.Text = "João 8:36";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Silver);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 22)
            {
                TxtVersiculo.Text = "Se é pecador, não sei; uma coisa sei: eu era cego e agora vejo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SkyBlue);
                txtCapitulo.Text = "João 9:25";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SkyBlue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 23)
            {
                TxtVersiculo.Text = "Antes, bem-aventurados são os que ouvem a palavra de Deus e a guardam!";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Lucas 11:28b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 24)
            {
                TxtVersiculo.Text = "Suportai-vos uns aos outros, perdoai-vos mutuamente, caso alguém tenha motivo de queixa contra outrém. Assim como o Senhor vos perdoou, assim tambem perdoai vós";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Colossenses 3:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 25)
            {
                TxtVersiculo.Text = "Quem não é por mim é contra mim; e quem comigo não ajunta espalha";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Gold);
                txtCapitulo.Text = "Lucas 11:23";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Gold);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 26)
            {
                TxtVersiculo.Text = "Repara, pois, que a luz que há em ti não sejam trevas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.GreenYellow);
                txtCapitulo.Text = "Lucas 11:35";
                txtCapitulo.SetTextColor(Android.Graphics.Color.GreenYellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 27)
            {
                TxtVersiculo.Text = "E bem-aventurado é aquele que não achar em mim motivo de tropeço";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.LawnGreen);
                txtCapitulo.Text = "Lucas 7:23";
                txtCapitulo.SetTextColor(Android.Graphics.Color.LawnGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 28)
            {
                TxtVersiculo.Text = "E não pratiquemos imoralidade, como alguns deles o fizeram, e caíram, num só dia, vinte e três mil";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.IndianRed);
                txtCapitulo.Text = "1 Coríntios 10:8";
                txtCapitulo.SetTextColor(Android.Graphics.Color.IndianRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 29)
            {
                TxtVersiculo.Text = "Se amais os que vos amam, qual é a vossa recompensa? Porque até os pecadores amam aos que os amam";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Olive);
                txtCapitulo.Text = "Lucas 6:32";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Olive);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 30)
            {
                TxtVersiculo.Text = "Para a liberdade foi que Cristo nos libertou";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SeaGreen);
                txtCapitulo.Text = "Gálatas 5:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SeaGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 31)
            {
                TxtVersiculo.Text = "E tudo quanto pedirdes em meu nome, isso farei, a fim de que o Pai seja glorificado no Filho";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "João 14:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 32)
            {
                TxtVersiculo.Text = "Que Deus é luz, e não há nele treva nenhuma";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "1 João 1:5b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 33)
            {
                TxtVersiculo.Text = "Como quereis que os homens vos façam, assim fazei-o vós também a eles";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Chocolate);
                txtCapitulo.Text = "Lucas 6:31";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Chocolate);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 34)
            {
                TxtVersiculo.Text = "Não há árvore boa que dê mau fruto; nem tampouco árvore má que dê bom fruto";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                txtCapitulo.Text = "Lucas 6:43";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 35)
            {
                TxtVersiculo.Text = "Porque a mensagem que ouvistes desde o princípio é esta: que nos amemos uns aos outros";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "1 João 3:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 36)
            {
                TxtVersiculo.Text = "Eu, porém, vos digo: amai os vossos inimigos e orai pelos que vos perseguem";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.ForestGreen);
                txtCapitulo.Text = "Mateus 5:44";
                txtCapitulo.SetTextColor(Android.Graphics.Color.ForestGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 37)
            {
                TxtVersiculo.Text = "E tudo o que fizerdes, seja em palavra, seja em ação, fazei-no em nome do Senhor Jesus, dando por ele graças a Deus Pai";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Colossenses 3:17";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 38)
            {
                TxtVersiculo.Text = "Irai-vos e não pequeis; não se ponha o sol sobre a vossa ira, nem deis lugar ao diabo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Efésios 4:26-27";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }


            if (NumVer == 39)
            {
                TxtVersiculo.Text = "Filhos, obedecei a vossos pais no Senhor, pois isto é justo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Efésios 6:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 40)
            {
                TxtVersiculo.Text = "Mas o maior dentre vós será vosso servo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "Mateus 23:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 41)
            {
                TxtVersiculo.Text = "Quem a si mesmo se exaltar será humilhado; e quem a si mesmo se humilhar será exaltado";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Mateus 23:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 42)
            {
                TxtVersiculo.Text = "Não fará Deus justiça aos seus escolhidos, que a ele clamam dia e noite, embora pareça demorado em defendê-los? ";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "Lucas 18:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta); 
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 43)
            {
                TxtVersiculo.Text = "Se o mundo vos odeia, sabei que, primeiro do que a vós outros, me odiou a mim";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Black);
                txtCapitulo.Text = "João 15:18";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Black);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 44)
            {
                TxtVersiculo.Text = "Devem ser considerados merecedores de dobrados honorários os presbíteros que presidem bem, com especialidade os que se afadigam na palavra e no ensino";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "1 Timóteo 5:17";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 45)
            {
                TxtVersiculo.Text = "Maridos, amai vossa esposa e não a trateis com amargura ";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Colossenses 3:19";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 46)
            {
                TxtVersiculo.Text = "Esposas, sede submissas ao próprio marido, como convém ao Senhor";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Black);
                txtCapitulo.Text = "Colossenses 3:18";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Black);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            // Intervalo em segundo para o próximo versiculo
            objTimer.Interval = 6500;

        }

        // 11/11/2018 18:35h
        // Versiculos alterados para a versão da Almeida Revista e Atualizada 2ª Edição de 1993
        public void Proverbios()
        {

            var TxtVersiculo = FindViewById<TextView>(Resource.Id.TxtVersiculo);
            var txtCapitulo = FindViewById<TextView>(Resource.Id.txtCapitulo);
            var Tela = FindViewById<LinearLayout>(Resource.Id.linearLayoutMain);

            // 'Pegar próximo número aleatório sendo gerado entre 0 e 31

            Random randNum = new Random();
            int NumVer = randNum.Next(45);

            // Define o texto e o versículo de cada número aleatório gerado
            if (NumVer == 0)
            {
                TxtVersiculo.Text = "Melhor é o que se estima em pouco e faz o seu trabalho do que o vanglorioso que tem falta de pão";
                txtCapitulo.Text = "Provérbios 12:9";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.BlueViolet);
                txtCapitulo.SetTextColor(Android.Graphics.Color.BlueViolet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 1)
            {
                TxtVersiculo.Text = "O temor do SENHOR é a instrução da sabedoria, e a humildade precede a honra";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Provérbios 15:33";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 2)
            {
                TxtVersiculo.Text = "O olhar de amigo alegra ao coração; as boas-novas fortalecem até os ossos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Provérbios 15:30";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 3)
            {
                TxtVersiculo.Text = "Pela misericórdia e pela verdade, se expia a culpa; e pelo temor do SENHOR os homens evitam o mal";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Provérbios 16:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 4)
            {
                TxtVersiculo.Text = "O homem se alegra em dar a resposta adequada, e a palavra, a seu tempo, quão boa é!";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Provérbios 15:23";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 5)
            {
                TxtVersiculo.Text = "Em todo tempo ama o amigo, e na angústia se faz o irmão";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Provérbios 17:17";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 6)
            {

                TxtVersiculo.Text = "O que tapa o ouvido ao clamor do pobre também clamará e não será ouvido";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtCapitulo.Text = "Provérbios 21:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 7)
            {
                TxtVersiculo.Text = "A resposta branda desvia o furor, mas a palavra dura suscita a ira";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Provérbios 15:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 8)
            {
                TxtVersiculo.Text = "A mulher sábia edifica a sua casa, mas a insensata, com as próprias mãos, a derriba";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Provérbios 14:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 9)
            {
                TxtVersiculo.Text = "Se o justo é punido na terra, quanto mais o perverso e o pecador!"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Provérbios 11:31";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 10)
            {
                TxtVersiculo.Text = "A esperança que se adia faz adoecer o coração, mas o desejo cumprido é árvore de vida";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.RosyBrown);
                txtCapitulo.Text = "Provérbios 13:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.RosyBrown);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 11)
            {
                TxtVersiculo.Text = "Coroa dos velhos são os filhos dos filhos; e a glória dos filhos são os pais";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Aqua);
                txtCapitulo.Text = "Provérbios 17:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Aqua);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 12)
            {
                TxtVersiculo.Text = "Mais vale o bom nome do que as muitas riquezas, e o ser estimado é melhor do que a prata e o ouro";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Provérbios 22:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 13)
            {
                TxtVersiculo.Text = "Exercitar justiça e juízo é mais aceitável ao SENHOR do que sacrifício";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Provérbios 21:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 14)
            {
                TxtVersiculo.Text = "O ornato dos jovens é a sua força, e a beleza dos velhos, as suas cãs";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Provérbios 20:29";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 15)
            {
                TxtVersiculo.Text = "A posse antecipada de uma herança no fim não será abençoada";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Lime);
                txtCapitulo.Text = "Provérbios 20:21";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Lime);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 16)
            {
                TxtVersiculo.Text = "O justo anda na sua integridade; felizes lhe são os filhos depois dele";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Orange);
                txtCapitulo.Text = "Provérbios 20:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Orange);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 17)
            {
                TxtVersiculo.Text = "Há ouro e abundância de pérolas, mas os lábios instruídos são jóia preciosa";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Provérbios 20:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 18)
            {
                TxtVersiculo.Text = "O que torna agradável o homem é a sua misericórdia; o pobre é preferível ao mentiroso";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Provérbios 19:22";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 19)
            {
                TxtVersiculo.Text = "A casa e os bens vêm como herança dos pais; mas do SENHOR, a esposa prudente"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Purple);
                txtCapitulo.Text = "Provérbios 19:14";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Purple);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 20)
            {
                TxtVersiculo.Text = "Melhor é o pobre que anda na sua integridade do que o perverso de lábios e tolo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Salmon);
                txtCapitulo.Text = "Provérbios 19:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Salmon);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 21)
            {
                TxtVersiculo.Text = "O que acha uma esposa acha o bem e alcançou a benevolência do SENHOR";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Silver);
                txtCapitulo.Text = "Provérbios 18:22";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Silver);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 22)
            {
                TxtVersiculo.Text = "Mulher virtuosa, quem a achará? O seu valor muito excede o de finas jóias";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SkyBlue);
                txtCapitulo.Text = "Provérbios 31:10";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SkyBlue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 23)
            {
                TxtVersiculo.Text = "Quando sobem os perversos, os homens se escondem, mas, quando eles perecem, os justos se multiplicam";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Provérbios 28:28";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 24)
            {
                TxtVersiculo.Text = "O que desvia os ouvidos de ouvir a lei, até a sua oração será abominável";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Provérbios 28:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 25)
            {
                TxtVersiculo.Text = "O temor do SENHOR é o princípio do saber, mas os loucos desprezam a sabedoria e o ensino";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Gold);
                txtCapitulo.Text = "Provérbios 1:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Gold);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 26)
            {
                TxtVersiculo.Text = "Não sejas sábio aos teus próprios olhos; teme ao SENHOR e aparta-te do mal";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.GreenYellow);
                txtCapitulo.Text = "Provérbios 3:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.GreenYellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 27)
            {
                TxtVersiculo.Text = "Não te furtes a fazer o bem a quem de direito, estando na tua mão o poder de fazê-lo"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.LawnGreen);
                txtCapitulo.Text = "Provérbios 3:27";
                txtCapitulo.SetTextColor(Android.Graphics.Color.LawnGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 28)
            {
                TxtVersiculo.Text = "O caminho dos perversos é como a escuridão; nem sabem eles em que tropeçam";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.IndianRed);
                txtCapitulo.Text = "Provérbios 4:19";
                txtCapitulo.SetTextColor(Android.Graphics.Color.IndianRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 29)
            {
                TxtVersiculo.Text = "Sobre tudo o que se deve guardar, guarda o coração, porque dele procedem as fontes de vida";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Olive);
                txtCapitulo.Text = "Provérbios 4:23";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Olive);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 30)
            {
                TxtVersiculo.Text = "Vai ter com a formiga, ó preguiçoso. considera os seus caminhos e sê sábio";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SeaGreen);
                txtCapitulo.Text = "Provérbios 6:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SeaGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 31)
            {
                TxtVersiculo.Text = "O temor do SENHOR consiste em aborrecer o mal; a soberba, a arrogância, o mau caminho e a boca perversa, eu os aborreço";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Provérbios 8:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 32)
            {
                TxtVersiculo.Text = "O que diz a verdade manifesta a justiça, mas a testemunha falsa, a fraude";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Provérbios 12:17";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 33)
            {
                TxtVersiculo.Text = "Nenhum agravo sobrevirá ao justo, mas os perversos, o mal os apanhará em cheio";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Chocolate);
                txtCapitulo.Text = "Provérbios 12:21";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Chocolate);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 34)
            {
                TxtVersiculo.Text = "Alguém há cuja tagarelice é como pontas de espada, mas a língua dos sábios é medicina";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                txtCapitulo.Text = "Provérbios 12:18";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 35)
            {
                TxtVersiculo.Text = "Porque o SENHOR repreende a quem ama, assim como o pai, ao filho a quem quer bem";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Provérbios 3:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 36)
            {
                TxtVersiculo.Text = "A maldição do SENHOR habita na casa do perverso, porém a morada dos justos ele abençoa";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Provérbios 3:33";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 37)
            {
                TxtVersiculo.Text = "Ensina a criança no caminho em que deve andar, e, ainda quando for velho, não se desviará dele";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Provérbios 22:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 38)
            {
                TxtVersiculo.Text = "Não retires da criança a disciplina, pois, se a fustigares com a vara, não morrerá";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Provérbios 23:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 39)
            {
                TxtVersiculo.Text = "Mais poder tem o sábio do que o forte, e o homem de conhecimento, mais do que o robusto";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Black);
                txtCapitulo.Text = "Provérbios 24:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Black);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 40)
            {
                TxtVersiculo.Text = "A luz dos justos brilha intensamente, mas a lâmpada dos perversos se apagará";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Provérbios 13:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 41)
            {
                TxtVersiculo.Text = "A justiça guarda aos que andam em integridade, mas a malícia subverte ao pecador";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Provérbios 13:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.Green);
            }

            if (NumVer == 42)
            {
                TxtVersiculo.Text = "Uns se dizem ricos sem terem nada; outros se dizem pobres, sendo mui ricos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Provérbios 13:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 43)
            {
                TxtVersiculo.Text = "Da soberba só resulta a contenda, mas com os que se aconselham se acha a sabedoria";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.BlueViolet);
                txtCapitulo.Text = "Provérbios 13:10";
                txtCapitulo.SetTextColor(Android.Graphics.Color.BlueViolet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Green);
            }

            if (NumVer == 44)
            {
                TxtVersiculo.Text = "O justo aborrece a palavra da mentira, mas o perverso faz vergonha e se desonra";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Provérbios 13:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 45)
            {
                TxtVersiculo.Text = "A boa inteligência consegue favor; mas o caminho dos pérfidos é intransitável";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Provérbios 13:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            // Intervalo em segundo para o próximo versiculo
            objTimer.Interval = 6500;



        }

        // 11/11/2018 19:19h
        // Versiculos alterados para a versão da Almeida Revista e Atualizada 2ª Edição de 1993

        public void Salmos()
        {

            var TxtVersiculo = FindViewById<TextView>(Resource.Id.TxtVersiculo);
            var txtCapitulo = FindViewById<TextView>(Resource.Id.txtCapitulo);
            var Tela = FindViewById<LinearLayout>(Resource.Id.linearLayoutMain);

            // 'Pegar próximo número aleatório sendo gerado entre 0 e 29

            Random randNum = new Random();
            int NumVer = randNum.Next(40);

            // Define o texto e o versículo de cada número aleatório gerado
            if (NumVer == 0)
            {
                TxtVersiculo.Text = "De que maneira poderá o jovem guardar puro o seu caminho? Observando-o segundo a tua palavra"; 
                txtCapitulo.Text = "Salmo 119:9";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.BlueViolet);
                txtCapitulo.SetTextColor(Android.Graphics.Color.BlueViolet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 1)
            {
                TxtVersiculo.Text = "Guardo no coração as tuas palavras, para não pecar contra ti ";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Salmo 119:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 2)
            {
                TxtVersiculo.Text = "O SENHOR é o meu pastor; nada me faltará";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Salmo 23:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 3)
            {
                TxtVersiculo.Text = "Bem-aventurado o homem que não anda no conselho dos ímpios";  
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Salmo 1:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 4)
            {
                TxtVersiculo.Text = "Senhor, tu tens sido o nosso refúgio, de geração em geração"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Salmo 90:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 5)
            {
                TxtVersiculo.Text = "Lâmpada para os meus pés é a tua palavra; e, luz para os meus caminhos";  
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Salmo 119:105";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 6)
            {

                TxtVersiculo.Text = "Espera, ó Israel, no SENHOR, desde agora e para sempre"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtCapitulo.Text = "Salmo 131:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 7)
            {
                TxtVersiculo.Text = "Alegrei-me quando me disseram: Vamos à casa do SENHOR"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Salmo 122:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 8)
            {
                TxtVersiculo.Text = "Os que confiam no SENHOR são como o monte de Sião, que não se abala, firme para sempre"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Salmo 125:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 9)
            {
                TxtVersiculo.Text = "A tua benignidade, SENHOR, chega até os céus, até às nuvens, a tua fidelidade";  
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Salmo 36:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 10)
            {
                TxtVersiculo.Text = "Mas jamais retirarei dele a minha bondade, nem desmentirei a minha fidelidade"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.RosyBrown);
                txtCapitulo.Text = "Salmo 89:33";
                txtCapitulo.SetTextColor(Android.Graphics.Color.RosyBrown);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 11)
            {
                TxtVersiculo.Text = "Pois disse eu: a benignidade está fundada para sempre; a tua fidelidade, tu a confirmarás nos céus";  
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Aqua);
                txtCapitulo.Text = "Salmo 89:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Aqua);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 12)
            {
                TxtVersiculo.Text = "Ensina-nos a contar os nossos dias, para que alcancemos coração sábio";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Salmo 90:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 13)
            {
                TxtVersiculo.Text = "Entrega o teu caminho ao SENHOR, confia nele, e o mais ele fará";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Salmo 37:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 14)
            {
                TxtVersiculo.Text = "Bem-aventurado aquele cuja iniquidade é perdoada, cujo pecado é coberto";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Salmo 32:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 15)
            {
                TxtVersiculo.Text = "SENHOR, meu Deus, clamei a ti por socorro, e tu me saraste";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Lime);
                txtCapitulo.Text = "Salmo 30:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Lime);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 16)
            {
                TxtVersiculo.Text = "Engrandecei o SENHOR comigo, e todos, à uma, lhe exaltemos o nome";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Orange);
                txtCapitulo.Text = "Salmo 34:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Orange);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 17)
            {
                TxtVersiculo.Text = "Os olhos do SENHOR repousam sobre os justos, e os seus ouvidos estão abertos ao seu clamor";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Salmo 34:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 18)
            {
                TxtVersiculo.Text = "Bem-aventurado o que acode ao necessitado; o SENHOR o livra no dia do mal";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Salmo 41:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 19)
            {
                TxtVersiculo.Text = "Abençoe-nos Deus, e todos os confins da terra o temerão";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Purple);
                txtCapitulo.Text = "Salmo 67:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Purple);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 20)
            {
                TxtVersiculo.Text = "Mostra-nos, SENHOR, a tua misericórdia e concede-nos a tua salvação";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Salmon);
                txtCapitulo.Text = "Salmo 85:7";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Salmon);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 21)
            {
                TxtVersiculo.Text = "O SENHOR faz justiça e julga a todos os oprimidos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Silver);
                txtCapitulo.Text = "Salmo 103:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Silver);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 22)
            {
                TxtVersiculo.Text = "Bem-aventurados os que guardam a retidão e o que pratica a justiça em todo o tempo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SkyBlue);
                txtCapitulo.Text = "Salmo 106:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SkyBlue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 23)
            {
                TxtVersiculo.Text = "Socorre, SENHOR, Deus meu! Salva-me segundo a tua misericórdia";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Salmo 109:26";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 24)
            {
                TxtVersiculo.Text = "Rendei graças ao SENHOR, porque ele é bom, porque a sua misericórdia dura para sempre";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "Salmo 118:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 25)
            {
                TxtVersiculo.Text = "O SENHOR está comigo; não temerei. Que me poderá fazer o homem?";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Gold);
                txtCapitulo.Text = "Salmo 118:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Gold);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 26)
            {
                TxtVersiculo.Text = "A pedra que os construtores rejeitaram, essa veio a ser a principal pedra, angular";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.GreenYellow);
                txtCapitulo.Text = "Salmo 118:22";
                txtCapitulo.SetTextColor(Android.Graphics.Color.GreenYellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 27)
            {
                TxtVersiculo.Text = "Aborreço a duplicidade, porém amo a tua lei";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.LawnGreen);
                txtCapitulo.Text = "Salmo 119:113";
                txtCapitulo.SetTextColor(Android.Graphics.Color.LawnGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 28)
            {
                TxtVersiculo.Text = "SENHOR, livra-me dos lábios mentirosos, da língua enganadora";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.IndianRed);
                txtCapitulo.Text = "Salmo 120:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.IndianRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 29)
            {
                TxtVersiculo.Text = "Põe guarda, SENHOR, à minha boca; vigia a porta dos meus lábios";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Olive);
                txtCapitulo.Text = "Salmo 141:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Olive);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 30)
            {
                TxtVersiculo.Text = "Invoco o SENHOR, digno de ser louvado, e serei salvo dos meus inimigos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SeaGreen);
                txtCapitulo.Text = "Salmos 18:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SeaGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 31)
            {
                TxtVersiculo.Text = "Ainda que eu ande pelo vale da sombra da morte, não temerei mal nenhum, porque tu estás comigo; o teu bordão e o teu cajado me consolam";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Salmo 23:4";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 32)
            {
                TxtVersiculo.Text = "Bem-aventurado o povo a quem assim sucede! Sim, bem-aventurado é o povo cujo Deus é o SENHOR!";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Salmos 144:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 33)
            {
                TxtVersiculo.Text = "Vivifica-me, SENHOR, por amor do teu nome, por amor da tua justiça, tira a tribulação da minha alma";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Salmos 143:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 34)
            {
                TxtVersiculo.Text = "Por que estás abatida, ó minha alma? Por que te perturbas dentro de mim? Espera em Deus, pois ainda o louvarei, a ele, meu auxílio Deus meu";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                txtCapitulo.Text = "Salmos 42:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 35)
            {
                TxtVersiculo.Text = "Eu, porém, invocarei a Deus, e o SENHOR me salvará";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "Salmos 55:16";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 36)
            {
                TxtVersiculo.Text = "Seja sobre nós, SENHOR, a tua misericórdia, como de ti esperamos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Salmos 33:22";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 37)
            {
                TxtVersiculo.Text = "Grande é o SENHOR, e mui digno de ser louvado, na cidade do nosso Deus";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Salmos 48:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }


            if (NumVer == 39)
            {
                TxtVersiculo.Text = "Leva-me para a rocha que é alta demais para mim";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Salmos 61:2b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 40)
            {
                TxtVersiculo.Text = "Bendito seja o Senhor que, dia a dia, leva nosso fardo! Deus é nossa salvação";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Black);
                txtCapitulo.Text = "Salmos 68:19";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Black);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }
            
            // Intervalo em segundo para o próximo versiculo
            objTimer.Interval = 6500;

        }
        public void MudarVersiculo()
        {
            
            var TxtVersiculo = FindViewById<TextView>(Resource.Id.TxtVersiculo);
            var txtCapitulo = FindViewById<TextView>(Resource.Id.txtCapitulo);
            var Tela = FindViewById<LinearLayout>(Resource.Id.linearLayoutMain);

            // 'Pegar próximo número aleatório sendo gerado entre 0 e 40

            Random randNum = new Random();
            int NumVer = randNum.Next(46);

            // Define o texto e o versículo de cada número aleatório gerado
            if (NumVer == 0)
            {
                TxtVersiculo.Text = "O amor seja sem hipocrisia. Detestai o mal, apegando-vos ao bem";
                txtCapitulo.Text = "Romanos 12:9";                
                TxtVersiculo.SetTextColor(Android.Graphics.Color.BlueViolet);
                txtCapitulo.SetTextColor(Android.Graphics.Color.BlueViolet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);                

            }

            if (NumVer == 1)
            {
                TxtVersiculo.Text = "Bem-aventurados os que choram, porque serão consolados";                    
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Mateus 5:4";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 2)
            {
                TxtVersiculo.Text = "Bem-aventurados os pacificadores, porque serão chamados filhos de Deus";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Mateus 5:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 3)
            {
                TxtVersiculo.Text = "Eu sou o caminho, e a verdade, e a vida.; ninguém vem ao pai senão por mim";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "João 14:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 4) {

                TxtVersiculo.Text = "Irai-vos e não pequeis; não se ponha o sol sobre a vossa ira, nem deis lugar ao diabo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGreen);
                txtCapitulo.Text = "Efésios 4:26-27";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 5)
            {                
                TxtVersiculo.Text = "Perseverai na oração, vigiando com ações de graças";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Green);
                txtCapitulo.Text = "Colossenses 4:2";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Green);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 6)
            {

                TxtVersiculo.Text = "Para a liberdade foi que Cristo nos libertou";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                txtCapitulo.Text = "Gálatas 5:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.WhiteSmoke);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 7)
            {
                TxtVersiculo.Text = "Deus é luz; e não há nele treva nenhuma";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "1 João 1:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 8)
            {

                TxtVersiculo.Text = "Esperei confiantemente pelo SENHOR";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "Salmo 40:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 9)
            {
                TxtVersiculo.Text = "O SENHOR sobre ti levante o rosto e te dê a paz";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "Números 6:26";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 10)
            {
                TxtVersiculo.Text = "Não ameis o mundo nem as coisas que há no mundo. Se alguém amar o mundo, o amor do Pai não está nele";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.RosyBrown);
                txtCapitulo.Text = "1 João 2:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.RosyBrown);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 11)
            {
                TxtVersiculo.Text = "Bem-aventurados os mansos, porque herdarão a terra";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Aqua);
                txtCapitulo.Text = "Mateus 5:5";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Aqua);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 12)
            {
                TxtVersiculo.Text = "Bem-aventurados os limpos de coração, porque verão a Deus";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Mateus 5:8";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 13)
            {
                TxtVersiculo.Text = "Evitai que alguém retribua a outrém mal por mal; pelo contrário, segui sempre o bem entre vós e para com todos";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "1 Tessalonicenses 5:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 14)
            {
                TxtVersiculo.Text = "Orai sem cessar";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Cyan);
                txtCapitulo.Text = "1 Tessalonicenses 5:17";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Cyan);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }

            if (NumVer == 15)
            {
                TxtVersiculo.Text = "Combate o bom combate da fé. Toma posse da vida eterna, para a qual também foste chamado e de que fizeste a boa confissão perante muitas testemunhas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Lime);
                txtCapitulo.Text = "1 Timóteo 6:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Lime);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 16)
            {
                TxtVersiculo.Text = "Todavia, o Senhor é fiel; ele vos confirmará e guardará do Maligno";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Orange);
                txtCapitulo.Text = "2 Tessalonicenses 3:3";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Orange);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 17)
            {
                TxtVersiculo.Text = "Procura apresentar-te a Deus aprovado, como obreiro que não tem de que se envergonhar, que maneja bem a palavra da verdade";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.OrangeRed);
                txtCapitulo.Text = "2 Timóteo 2:15";
                txtCapitulo.SetTextColor(Android.Graphics.Color.OrangeRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 18)
            {
                TxtVersiculo.Text = "Porque Deus ama a quem dá com alegria";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);                
                txtCapitulo.Text = "2 Coríntios 9:7b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 19)
            {
                TxtVersiculo.Text = "Nós amamos porque ele nos amou primeiro";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Purple);
                txtCapitulo.Text = "1 João 4:19";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Purple);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 20)
            {
                TxtVersiculo.Text = "Aquele que diz estar na luz e odeia a seu irmão, até agora, está nas trevas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Salmon);
                txtCapitulo.Text = "1 João 2:9";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Salmon);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 21)
            {
                TxtVersiculo.Text = "No mundo passais por aflições; mas tende bom ânimo; eu venci o mundo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Silver);
                txtCapitulo.Text = "João 16:33b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Silver);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 22)
            {
                TxtVersiculo.Text = "O meu mandamento é este: que vos ameis uns aos outros, assim como eu vos amei";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SkyBlue);
                txtCapitulo.Text = "João 15:12";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SkyBlue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 23)
            {
                TxtVersiculo.Text = "Vós sois meus amigos, se fazeis o que eu vos mando";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "João 15:14";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 24)
            {
                TxtVersiculo.Text = "O que é nascido da carne é carne; e o que é nascido do Espírito é espírito";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Violet);
                txtCapitulo.Text = "João 15:6";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Violet);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 25)
            {
                TxtVersiculo.Text = "Porque qualquer que fizer a vontade de meu Pai celeste, esse é meu irmão, irmã e mãe";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Gold);
                txtCapitulo.Text = "Mateus 12:50";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Gold);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 26)
            {
                TxtVersiculo.Text = "Eu e o Pai somos um"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.GreenYellow);
                txtCapitulo.Text = "João 10:30";
                txtCapitulo.SetTextColor(Android.Graphics.Color.GreenYellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 27)
            {
                TxtVersiculo.Text = "Eu sou o bom pastor. O bom pastor dá a vida pelas ovelhas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.LawnGreen);
                txtCapitulo.Text = "João 10:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.LawnGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 28)
            {
                TxtVersiculo.Text = "Se, pois, o Filho vos libertar, verdadeiramente sereis livres";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.IndianRed);
                txtCapitulo.Text = "João 8:36";
                txtCapitulo.SetTextColor(Android.Graphics.Color.IndianRed);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 29)
            {
                TxtVersiculo.Text = "Se é pecador, não sei; uma coisa sei: eu era cego e agora vejo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Olive);
                txtCapitulo.Text = "João 9:25";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Olive);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 30)
            {
                TxtVersiculo.Text = "Antes, bem-aventurados são os que ouvem a palavra de Deus e a guardam!";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.SeaGreen);
                txtCapitulo.Text = "Lucas 11:28b";
                txtCapitulo.SetTextColor(Android.Graphics.Color.SeaGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 31)
            {
                TxtVersiculo.Text = "Quem não é por mim é contra mim; e quem comigo não ajunta espalha";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "Lucas 11:23";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 32)
            {
                TxtVersiculo.Text = "Suportai-vos uns aos outros, perdoai-vos mutuamente, caso alguém tenha motivo de queixa contra outrem. Assim como o Senhor vos perdoou, assim também perdoai vós";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Brown);
                txtCapitulo.Text = "Colossenses 3:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Brown);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 33)
            {
                TxtVersiculo.Text = "Repara, pois, que a luz que há em ti não sejam trevas";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Chocolate);
                txtCapitulo.Text = "Lucas 11:35";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Chocolate);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 34)
            {
                TxtVersiculo.Text = "E bem-aventurado é aquele que não achar em mim motivo de tropeço";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                txtCapitulo.Text = "Lucas 7:23";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkGoldenrod);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 35)
            {
                TxtVersiculo.Text = "Como quereis que os homens vos façam, assim fazei-o vós também a eles"; 
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "Lucas 6:31";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 36)
            {
                TxtVersiculo.Text = "Não há árvore boa que dê mau fruto; nem tampouco árvore má que dê bom fruto";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.ForestGreen);
                txtCapitulo.Text = "Lucas 6:43";
                txtCapitulo.SetTextColor(Android.Graphics.Color.ForestGreen);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 37)
            {
                TxtVersiculo.Text = "Porque a mensagem que ouvistes desde o princípio é esta: que nos amemos uns aos outros";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "1 João 3:11";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 38)
            {
                TxtVersiculo.Text = "Eu, porém, vos digo: amai os vossos inimigos e orai pelos que vos perseguem";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Mateus 5:44";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Red);
            }


            if (NumVer == 39)
            {
                TxtVersiculo.Text = "Filhos, obedecei a vossos pais no Senhor, pois isto é justo";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "Efésios 6:1";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 40)
            {
                TxtVersiculo.Text = "E tudo quanto pedirdes em meu nome, isso farei, a fim de que o Pai seja glorificado no Filho";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Red);
                txtCapitulo.Text = "João 14:13";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Red);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 41)
            {
                TxtVersiculo.Text = "Sede fortes, e revigore-se o vosso coração, vós todos que esperais no SENHOR";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.White);
                txtCapitulo.Text = "Salmos 31:24";
                txtCapitulo.SetTextColor(Android.Graphics.Color.White);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 42)
            {
                TxtVersiculo.Text = "Assim, os justos renderão graças ao teu nome, e os retos habitarão na tua presença";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "Salmos 140:14";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Blue);
            }

            if (NumVer == 43)
            {
                TxtVersiculo.Text = "Acima de tudo, porém, tende amor intenso uns para com os outros, porque o amor cobre multidão de pecados";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                txtCapitulo.Text = "1 Pedro 4:8";
                txtCapitulo.SetTextColor(Android.Graphics.Color.DarkMagenta);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            if (NumVer == 44)
            {
                TxtVersiculo.Text = "Aquele que tem os meus mandamentos e os guarda, esse é o que me ama";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Blue);
                txtCapitulo.Text = "João 14:21a";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Blue);
                Tela.SetBackgroundColor(Android.Graphics.Color.White);
            }

            if (NumVer == 45)
            {
                TxtVersiculo.Text = "Se o mundo vos odeia, sabei que, primeiro do que a vós outros, me odiou a mim";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Black);
                txtCapitulo.Text = "João 15:18";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Black);
                Tela.SetBackgroundColor(Android.Graphics.Color.Yellow);
            }

            if (NumVer == 46)
            {
                TxtVersiculo.Text = "Devem ser considerados merecedores de dobrados honorários os presbíteros que presidem bem, com especialidade os que se afadigam na palavra e no ensino";
                TxtVersiculo.SetTextColor(Android.Graphics.Color.Yellow);
                txtCapitulo.Text = "I Timóteo 5:17";
                txtCapitulo.SetTextColor(Android.Graphics.Color.Yellow);
                Tela.SetBackgroundColor(Android.Graphics.Color.Black);
            }

            // Intervalo em segundo para o próximo versiculo
            objTimer.Interval = 6000;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            MenuInflater inflater = this.MenuInflater;
            inflater.Inflate(Resource.Menu.Menu1, menu);  
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Resource.Id.Configurar:
                    // Configurar o APP                 
                    var atividadeConfig = new Intent(this, typeof(Configurar));
                    StartActivity(atividadeConfig);
                    break;


                case Resource.Id.Sobre:
                    // SOBRE O APP                 
                    var atividadeSobre = new Intent(this, typeof(Sobre));
                    StartActivity(atividadeSobre);
                    break;

                

            }
            return base.OnOptionsItemSelected(item);
        }

        public void onBackPressed()
        {
            SetContentView(Resource.Layout.Main);
        }

        
    
    }
    

}

