
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

namespace CorresApp.Droid
{
    [Activity(NoHistory = true, Theme = "@style/SplashTheme", MainLauncher = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                if (Intent.Extras != null)
                {
                    var id = Intent.Extras.Get("Id");
                    if (id != null)
                    {
                        Intent i = new Intent(this, typeof(MainActivity));
                        i.PutExtra("ID", id.ToString());
                    
                        StartActivity(i);
                    }
                    else
                    {
                        StartActivity(typeof(MainActivity));
                    }
             
                }
                else
                {
                    StartActivity(typeof(MainActivity));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        
        }
    }
}
