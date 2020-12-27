using UnityEngine;

namespace UnityCore
{
    public static class AndroidUtil
    {
        /// <summary>
        /// 安装apk
        /// </summary>
        public static void InstallApk(string apkPath)
        {
            using (AndroidJavaClass cl = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject ob = cl.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //然后调用android来安装apk
                    ob.Call("InstallApk", apkPath);
                }
            }
        }
        /// <summary>
        /// 安装apk
        /// </summary>
        public static void InstallAPK(string apkPath)
        {
            try
            {
                var FileProvider = new AndroidJavaClass("android.support.v4.content.FileProvider");
                var Intent = new AndroidJavaClass("android.content.Intent");
                var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
                var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                var FLAG_GRANT_READ_URI_PERMISSION = Intent.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
                var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);
                var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                 
                var file = new AndroidJavaObject("java.io.File", apkPath);
                var uri = FileProvider.CallStatic<AndroidJavaObject>("getUriForFile", currentActivity, string.Format("{0}.fileprovider", Application.identifier), file);
                intent.Call<AndroidJavaObject>("setFlags", FLAG_ACTIVITY_NEW_TASK);
                intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");

                currentActivity.Call("startActivity", intent);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static void InstallAPK_7(string apk)
        {
            /*
             Java_Code
             Intent install = new Intent(Intent.ACTION_VIEW);
            install.setDataAndType(Uri.fromFile(apkFile), "application/vnd.android.package-archive");
            install.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(install);
             */

        }
        public static void InstallAPK_8(string apk)
        {
            /*
             Java_Code
             Uri apkUri = FileProvider.getUriForFile(this, "完整包名.fileprovider", apkFile);
             //在AndroidManifest中的android:authorities值
            Intent install = new Intent(Intent.ACTION_VIEW);
            install.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            install.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
            install.setDataAndType(apkUri, "application/vnd.android.package-archive");
            startActivity(install);
             */
        }
    }
}
