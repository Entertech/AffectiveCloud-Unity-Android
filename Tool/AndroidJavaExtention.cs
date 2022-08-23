using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AndroidJavaExtention
{
    private static AndroidJavaExtention mInstance = null;

    public Text debugLabel;

    public static AndroidJavaExtention instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new AndroidJavaExtention();
            }

            return mInstance;
        }
    }

    
    /// <summary>
    /// android 提示框
    /// </summary>
    /// <param name="str"></param>
    /// <param name="ajo"></param>
    public void toastText(object str, AndroidJavaObject ajo)
    {
        if (debugLabel != null) {
            debugLabel.text = str.ToString();
        }
//         if (ajo == null) {
//             return;
//         }
// #if UNITY_ANDROID
//         AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
//         AndroidJavaObject context = ajo.Call<AndroidJavaObject>("getApplicationContext");
//         ajo.Call("runOnUiThread", new AndroidJavaRunnable(() => {
//             AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", str.ToString());
//             Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
//         }
//         ));
// #endif
    }
}
