using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace Enter.Assets.Scripts
{

    public delegate void OnBLEChanged(string state);
    
    /// <summary>
    /// 蓝牙扫描到设备回调
    /// </summary>
    public class BleScanSuccessCallback : AndroidJavaProxy
    {
        Text objects;


        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数，此处只是传入两个参数作为例子
        /// kotlin.jvm.functions.Function0 表示无参数回调
        /// </summary>
        /// <param name="list">需要获取的信息</param>
        /// <param name="obj">安卓的activity界面</param>
        /// <returns></returns>
        public BleScanSuccessCallback(ref Text obj) : base("kotlin.jvm.functions.Function0")
        {
            objects = obj;
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke()
        {
            
            
        }
    }

    /// <summary>
    /// 蓝牙未扫描到设备回调
    /// </summary>
    public class BleScanFailedCallback : AndroidJavaProxy
    {

        Text log;

        public event OnBLEChanged BLEChangedDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数，此处只是传入两个参数作为例子
        /// kotlin.jvm.functions.Function1 表示回调有1个参数
        /// </summary>
        /// <param name="list">需要获取的信息</param>
        /// <param name="obj">安卓的activity界面</param>
        /// <returns></returns>
        public BleScanFailedCallback(ref Text text) : base("kotlin.jvm.functions.Function1")
        {

            log = text;
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(AndroidJavaObject error)
        {
            
            // var sprite = Resources.Load<Sprite>("Disconnect");
            // objects.image.sprite = sprite;
            BLEChangedDelegate("Disconnect");
            log.text = "Scan Failed";
        }
    }

    public class ConnectSuccessCallback : AndroidJavaProxy
    {

        Text log;
        public event OnBLEChanged BLEChangedDelegate;
        
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数，此处只是传入两个参数作为例子
        /// kotlin.jvm.functions.Function1 表示回调有1个参数
        /// </summary>
        /// <param name="list">需要获取的信息</param>
        /// <param name="obj">安卓的activity界面</param>
        /// <returns></returns>
        public ConnectSuccessCallback(ref Text text) : base("kotlin.jvm.functions.Function1")
        {

            log = text;
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(string result)
        {
            log.text = "Connected";
            BLEChangedDelegate("Connected");
            // var sprite = Resources.Load<Sprite>("Connected");
            // objects.image.sprite = sprite;
            
        }
    }

    public class ConnectFailedCallback : AndroidJavaProxy
    {

        Text log;
        public event OnBLEChanged BLEChangedDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数，此处只是传入两个参数作为例子
        /// kotlin.jvm.functions.Function1 表示回调有1个参数
        /// </summary>
        /// <param name="list">需要获取的信息</param>
        /// <param name="obj">安卓的activity界面</param>
        /// <returns></returns>
        public ConnectFailedCallback(ref Text text) : base("kotlin.jvm.functions.Function1")
        {

            log = text;
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(string result)
        {
            BLEChangedDelegate("Disconnect");
            // var sprite = Resources.Load<Sprite>("Disconnect");
            // objects.image.sprite = sprite;
            log.text = "Connect Failed";
        }
    }

    /// <summary>
    /// 脑波数据的蓝牙回调
    /// </summary>
    public class RawBrainDataCallback : AndroidJavaProxy
    {
        public event OnBLEChanged BLEChangedDelegate;
        public RawBrainDataCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(AndroidJavaObject eeg)
        {
            try
            {
                
                if (AffectiveManager.instance.bIsInit())
                {
                    
                    AffectiveManager.instance.appendEeg(eeg);
                }
            }
            catch (System.Exception ex)
            {
                
            }
        }
    }

    /// <summary>
    /// 心率数据的蓝牙回调
    /// </summary>
    public class HeartRateDataCallback : AndroidJavaProxy
    {
        
        public HeartRateDataCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(int hr)
        {
            // 心率数据传到情感云上
            try
            {
                // if (AffectiveManager.instance.bIsInit())
                // {   
                //     AffectiveManager.instance.appendHeartRate(hr);
                // }
            }
            catch (System.Exception ex)
            {
                
            }

        }
    }


}