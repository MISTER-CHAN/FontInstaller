using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.IO;
using System.Threading;
using Java.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FontInstaller
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button button;
        LinearLayout linearLayout;
        TextView textView;
        Timer timer = new Timer();
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            button = FindViewById<Button>(Resource.Id.button);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            textView = FindViewById<TextView>(Resource.Id.textView);

            button.Click += Button_Click;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = false;
            timer.Interval = 1;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            button.Enabled = false;
            new Thread(InstallFont).Start();
        }

        private void InstallFont()
        {
            RunOnUiThread(() =>
            {
                textView.Text = "刪除字體...\n";
            });
            File[] files = new File("/sdcard/MIUI/theme/.data/content/fonts/").ListFiles();
            string s = files[0].Name;
            files[0].Delete();
            RunOnUiThread(() =>
            {
                textView.Text = "複製字體並重命名...\n";
            });
            FileInputStream fileInputStream = new FileInputStream("/sdcard/MISTER_CHAN-ExtBCDEF.ttf");
            new File("/sdcard/MISTER_CHAN-ExtBCDEF.ttf").Exists();
            FileOutputStream fileOutputStream = new FileOutputStream("/sdcard/MIUI/theme/.data/content/fonts/" + s);
            byte[] buffer = new byte[1024];
            int byteRead;
            while ((byteRead = fileInputStream.Read(buffer)) != -1)
            {
                fileOutputStream.Write(buffer, 0, byteRead);
            }
            fileInputStream.Close();
            fileOutputStream.Flush();
            fileOutputStream.Close();
            timer.Enabled = true;
            RunOnUiThread(() =>
            {
                textView.Text = "完成!\n";
                button.Enabled = true;
            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            byte r = ((ColorDrawable)linearLayout.Background).Color.R;
            byte b = ((ColorDrawable)linearLayout.Background).Color.B;
            if (r == 0xf || b == 0xf)
            {
                timer.Enabled = false;
                return;
            }
            RunOnUiThread(() =>
            {
                linearLayout.SetBackgroundColor(new Color(r - 0x10, 0xff, b - 0x10));
            });
        }
    }
}