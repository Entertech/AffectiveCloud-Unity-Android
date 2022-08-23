// using UnityEngine;
// using System.Collections;
// using System.Threading;
// using Enter.Assets.Scripts;
// public class ErrorCallbackDelegate : BuglyCallback
// {
//     private static readonly ErrorCallbackDelegate _instance = new ErrorCallbackDelegate();

//     static ErrorCallbackDelegate()
//     {
//     }

//     private ErrorCallbackDelegate()
//     {
//     }

//     public static ErrorCallbackDelegate Instance
//     {
//         get
//         {
//             return _instance;
//         }
//     }

//     public AndroidJavaObject active = null;



//     /// <summary>
//     /// Raises the application log callback handler event.
//     /// </summary>
//     /// <param name="condition">Condition.</param>
//     /// <param name="stackTrace">Stack trace.</param>
//     /// <param name="type">Type.</param>
//     public override void OnApplicationLogCallbackHandler(string condition, string stackTrace, LogType type)
//     {
//         // AndroidJavaExtention.instance.toastText("crash", active);
//         var file = new string[2];
//         file[0] = string.Format("Current thread: {0} ", Thread.CurrentThread.ManagedThreadId);
//         file[1] = string.Format("[{0}] - {1}\n{2}", type.ToString(), condition, stackTrace);

//         FileOperation.instance.saveFile(file);
//         //// only for test and check the callback handler called

//         //		System.Console.Write ("--------- OnApplicationLogCallbackHandler ---------\n");
//         //
//         //		System.Console.Write ("Current thread: {0}", Thread.CurrentThread.ManagedThreadId);
//         //		System.Console.WriteLine ();
//         //
//         //		System.Console.Write ("[{0}] - {1}\n{2}",type.ToString(), condition, stackTrace);
//         //
//         //		System.Console.Write ("--------- OnApplicationLogCallbackHandler ---------");
//         //        System.Console.WriteLine ();
//     }
// }