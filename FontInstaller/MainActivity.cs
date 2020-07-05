using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.IO;
using System.Collections.Generic;
using Thread = System.Threading.Thread;

namespace FontInstaller
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button bCustom, bDelete, bOfficial;
        CheckBox cbStart;
        string s = "";
        TextView textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            bCustom = FindViewById<Button>(Resource.Id.b_custom);
            bDelete = FindViewById<Button>(Resource.Id.b_delete);
            bOfficial = FindViewById<Button>(Resource.Id.b_official);
            cbStart = FindViewById<CheckBox>(Resource.Id.cb_start);
            textView = FindViewById<TextView>(Resource.Id.textView);

            bDelete.Click += BDelete_Click;
            bCustom.Click += BCustom_Click;
            bOfficial.Click += BOfficial_Click;
            FindViewById<Button>(Resource.Id.b_start).Click += BStart_Click;
        }

        private void BCustom_Click(object sender, System.EventArgs e)
        {
            bCustom.Enabled = false;
            Replace("/sdcard/FontInstaller/MISTER_CHAN-ExtBCDEF.ttf");
        }

        private void BDelete_Click(object sender, System.EventArgs e)
        {
            bDelete.Enabled = false;
            bOfficial.Enabled = false;
            textView.Text = "刪除 MRC...\n";
            File file = new File("/sdcard/MIUI/theme/.data/content/fonts/");
            if (!file.Exists())
                file.Mkdir();
            File[] files = file.ListFiles();
            if (files.Length > 0)
                files[0].Delete();
            textView.Text = "已刪除 MRC\n";
            if (cbStart.Checked)
                StartActivity(PackageManager.GetLaunchIntentForPackage("com.android.thememanager"));
        }

        private void BOfficial_Click(object sender, System.EventArgs e)
        {
            bOfficial.Enabled = false;
            bDelete.Enabled = false;
            Replace("/sdcard/FontInstaller/MIUI.mrc");
        }

        private void BStart_Click(object sender, System.EventArgs e)
        {
            StartActivity(PackageManager.GetLaunchIntentForPackage("com.android.thememanager"));
        }

        private void InstallFont(object input)
        {
            FileInputStream fileInputStream = new FileInputStream(input + "");
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
            RunOnUiThread(() =>
            {
                if (cbStart.Checked)
                    StartActivity(PackageManager.GetLaunchIntentForPackage("com.android.thememanager"));
                textView.Text = "完成!\n";
            });
        }

        void Replace(string input)
        {
            textView.Text = "刪除 MRC...\n";
            File file = new File("/sdcard/MIUI/theme/.data/content/fonts/");
            if (!file.Exists())
                file.Mkdir();
            File[] files = file.ListFiles();
            if (files.Length > 0)
            {
                s = files[0].Name;
                files[0].Delete();
                textView.Text = "複製 MRC 並重命名...\n";
                new Thread(new System.Threading.ParameterizedThreadStart(InstallFont)).Start(input);
            }
            else
                textView.Text = "失敗: MIUI 中無 MRC!\n";
        }
    }
}