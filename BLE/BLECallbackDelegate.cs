using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace Enter.Assets.Scripts
{
    public enum BluetoothState
    {
        scan,
        connecting,
        connected,
        disconnecting,
        disconnected,
    }

    public delegate void OnBLEChanged(BluetoothState state);
    public delegate void OnEEGChanged(AndroidJavaObject eeg);
    public delegate void OnHRChanged(int hr);

    public delegate void OnContactChanged(string value);
    public delegate void OnBatteryChanged(double value);
    /// <summary>
    /// 蓝牙扫描到设备回调
    /// </summary>
    public class BleScanSuccessCallback : AndroidJavaProxy
    {
        public event OnBLEChanged bleChangedDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示
        /// kotlin.jvm.functions.Function0 表示无参数回调
        public BleScanSuccessCallback() : base("kotlin.jvm.functions.Function0")
        {
            
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke()
        {
            bleChangedDelegate(BluetoothState.connecting);
            
        }
    }

    /// <summary>
    /// 蓝牙未扫描到设备回调
    /// </summary>
    public class BleScanFailedCallback : AndroidJavaProxy
    {

        public event OnBLEChanged bleChangedDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// kotlin.jvm.functions.Function1 表示回调有1个参数
        /// <returns></returns>
        public BleScanFailedCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(AndroidJavaObject error)
        {
            

            bleChangedDelegate(BluetoothState.disconnected);

        }
    }
    /// <summary>
    /// 连接成功回调
    /// </summary>
    public class ConnectSuccessCallback : AndroidJavaProxy
    {
        public event OnBLEChanged bleChangedDelegate;
        
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// kotlin.jvm.functions.Function1 表示回调有1个参数

        public ConnectSuccessCallback() : base("kotlin.jvm.functions.Function1")
        {

        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(string result)
        {
            bleChangedDelegate(BluetoothState.connected);
        }
    }
    /// <summary>
    /// 发起连接失败回调
    /// </summary>
    public class ConnectFailedCallback : AndroidJavaProxy
    {
        public event OnBLEChanged bleChangedDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// kotlin.jvm.functions.Function1 表示回调有1个参数

        public ConnectFailedCallback() : base("kotlin.jvm.functions.Function1")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(string result)
        {
            bleChangedDelegate(BluetoothState.disconnected);
        }
    }
    
    /// <summary>
    /// 连接断开回调
    /// </summary>
    public class DisconnectCallback : AndroidJavaProxy
    {
        public event OnBLEChanged bleChangedDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// kotlin.jvm.functions.Function1 表示回调有1个参数

        public DisconnectCallback() : base("kotlin.jvm.functions.Function1")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(string result)
        {
            bleChangedDelegate(BluetoothState.disconnected);
        }
    }

    /// <summary>
    /// 佩戴检测回调
    /// </summary>
    public class ContactCallback : AndroidJavaProxy
    {
        public event OnContactChanged contactDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// kotlin.jvm.functions.Function1 表示回调有1个参数

        public ContactCallback() : base("kotlin.jvm.functions.Function1")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(string result)
        {
            contactDelegate(result);
        }
    }
    /// <summary>
    /// 电量回调
    /// </summary>
    public class BatteryCallback : AndroidJavaProxy
    {
        public event OnBatteryChanged batteryDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示
        /// kotlin.jvm.functions.Function1 表示回调有1个参数

        public BatteryCallback() : base("kotlin.jvm.functions.Function1")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(double result)
        {
            batteryDelegate(result);
        }
    }

    /// <summary>
    /// 脑波数据的蓝牙回调
    /// </summary>
    public class RawBrainDataCallback : AndroidJavaProxy
    {
        public event OnEEGChanged eegChangedDelegate;
        public RawBrainDataCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(AndroidJavaObject eeg)
        {
            eegChangedDelegate(eeg);
        }
    }

    /// <summary>
    /// 心率数据的蓝牙回调
    /// </summary>
    public class HeartRateDataCallback : AndroidJavaProxy
    {
        public event OnHRChanged hrChangedDelegate;
        public HeartRateDataCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(int hr)
        {
            hrChangedDelegate(hr);

        }
    }


}