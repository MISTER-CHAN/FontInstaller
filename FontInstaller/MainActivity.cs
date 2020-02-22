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
        Button bLaunch, bRename;
        LinearLayout linearLayout;
        string s = "";
        TextView textView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            bLaunch = FindViewById<Button>(Resource.Id.b_launch);
            bRename = FindViewById<Button>(Resource.Id.b_rename);
            linearLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            textView = FindViewById<TextView>(Resource.Id.textView);

            bLaunch.Click += BLaunch_Click;
            bRename.Click += BRename_Click;
        }

        private void BLaunch_Click(object sender, System.EventArgs e)
        {
            bLaunch.Visibility = ViewStates.Gone;
            textView.Text = "刪除字體...\n";
            File file = new File("/sdcard/MIUI/theme/.data/content/fonts/");
            if (!file.Exists())
            {
                file.Mkdir();
            }
            File[] files = file.ListFiles();
            if (files.Length > 0)
            {
                files[0].Delete();
            }
            textView.Text = "已刪除字體\n";
            StartActivity(PackageManager.GetLaunchIntentForPackage("com.android.thememanager"));
        }

        private void BRename_Click(object sender, System.EventArgs e)
        {
            bLaunch.Visibility = ViewStates.Gone;
            bRename.Enabled = false;
            textView.Text = "刪除字體...\n";
            File[] files = new File("/sdcard/MIUI/theme/.data/content/fonts/").ListFiles();
            s = files[0].Name;
            files[0].Delete();
            textView.Text = "複製字體並重命名...\n";
            new Thread(InstallFont).Start();
        }

        private void InstallFont()
        {
            FileInputStream fileInputStream = new FileInputStream("/sdcard/MISTER_CHAN-ExtBCDEF.ttf");
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
                StartActivity(PackageManager.GetLaunchIntentForPackage("com.android.thememanager"));
                textView.Text = "完成!\n";
                bRename.Visibility = ViewStates.Gone;
            });
        }
    }
}