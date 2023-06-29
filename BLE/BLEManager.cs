using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System;

namespace Enter.Assets.Scripts
{
    public class BLEManager
    {
        private static string ENTER_BLE_MANAGER = "cn.entertech.ble.single.BiomoduleBleManager";
        public AndroidJavaObject ble = null;
        public event OnWebsocketDebug debugDelegate;
        private AndroidJavaObject active = null;
        private static BLEManager mInstance = null;


        public static BLEManager instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new BLEManager();
                }

                return mInstance;
            }
        }
        /// 安卓如果没有定位权限无法开启蓝牙，所以首次只会开启定位权限
        public void requestAuth()
        {

            try
            {
                var bluetooth = new AndroidJavaClass("android.bluetooth.BluetoothAdapter");//获得Android BluetoothAdapter类

                var bluetoothAdapter = bluetooth.CallStatic<AndroidJavaObject>("getDefaultAdapter");

                //判断系统蓝牙是否打开  
                if (!bluetoothAdapter.Call<bool>("isEnabled"))
                {
                    var isOpen = bluetoothAdapter.Call<bool>("enable");  //打开蓝牙，需要BLUETOOTH_ADMIN权限  
                }
                //定位权限
                if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
                {
                    Permission.RequestUserPermission(Permission.FineLocation);
                    return;
                }
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }

        }

        /// <summary>
        /// 初始化ble，
        /// </summary>
        /// <param name="currentActivity"></param>
        /// <returns></returns>
        public bool initializeJavaObject(ref AndroidJavaObject currentActivity)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                return false;
            }
            if (currentActivity == null)
            {
                debugDelegate("currentActivity is null");
                return false;
            }
            else
            {
                active = currentActivity;
            }
            if (ble == null)
            {

                try
                {   //初始化
                    AndroidJavaClass ajc = new AndroidJavaClass(ENTER_BLE_MANAGER);
                    var companion = ajc.GetStatic<AndroidJavaObject>("Companion");
                    ble = companion.Call<AndroidJavaObject>("getInstance", active);


                }
                catch (Exception ex)
                {
                    // 进行异常处理
                    debugDelegate(ex.Message);
                    return false;
                }
                //初始化判断是否成功
                if (ble == null)
                {

                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 蓝牙扫描周边6秒，找到符合的uuid，然后连接信号最好的那个
        /// </summary>
        /// <param name="bleScanSuccessCallback">蓝牙扫描回调</param>
        /// <param name="bleScanFailedCallback">蓝牙未扫描到回调</param>
        /// <param name="connectSuccessCallback">连接成功回调</param>
        /// <param name="connectFailedCallback">连接失败回调</param>
        /// <param name="currentActivity"></param>
        public void bleScanAndConnect(ref BleScanSuccessCallback bleScanSuccessCallback,
                                    ref BleScanFailedCallback bleScanFailedCallback,
                                    ref ConnectSuccessCallback connectSuccessCallback,
                                    ref ConnectFailedCallback connectFailedCallback
                                    )
        {
            debugDelegate("start connect");
            if (ble != null)
            {
                try
                {
                    //开始扫描并连接设备
                    ble.Call("scanNearDeviceAndConnect",
                    bleScanSuccessCallback,
                    bleScanFailedCallback,
                    connectSuccessCallback,
                    connectFailedCallback);
                }
                catch (Exception ex)
                {
                    // 进行异常处理
                    debugDelegate(ex.Message);
                }

            }
            else
            {
                debugDelegate("ble is null");
                // 进行异常处理

            }
        }

        /// <summary>
        /// 蓝牙连接绑定的设备, 需要先进行系统蓝牙绑定
        /// </summary>
        /// <param name="bleScanSuccessCallback">蓝牙扫描回调</param>
        /// <param name="bleScanFailedCallback">蓝牙未扫描到回调</param>
        /// <param name="connectSuccessCallback">连接成功回调</param>
        /// <param name="connectFailedCallback">连接失败回调</param>
        /// <param name="currentActivity"></param>
        public void bleConnectBonded(ref BleScanSuccessCallback bleScanSuccessCallback,
                                    ref BleScanFailedCallback bleScanFailedCallback,
                                    ref ConnectSuccessCallback connectSuccessCallback,
                                    ref ConnectFailedCallback connectFailedCallback
                                    )
        {
            debugDelegate("start connect");
            if (ble != null)
            {
                try
                {
                    //开始扫描并连接设备
                    ble.Call("scanNearDeviceAndConnect",
                    bleScanSuccessCallback,
                    bleScanFailedCallback,
                    connectSuccessCallback,
                    connectFailedCallback, 1);
                }
                catch (Exception ex)
                {
                    // 进行异常处理
                    debugDelegate(ex.Message);
                }

            }
            else
            {
                debugDelegate("ble is null");
                // 进行异常处理

            }
        }

        /// <summary>
        /// 断开蓝牙
        /// </summary>
        public void bleDisconnect()
        {
            ble.Call("disConnect");
        }

        ///添加监听
        public void addListener(ref RawBrainDataCallback rawBrainDataCallback, ref HeartRateDataCallback heartRateDataCallback, ref DisconnectCallback disCall, ref ContactCallback contactCall, ref BatteryCallback batteryCall)
        {
            if (ble == null)
            {
                // 未初始化，进行异常处理
                return;
            }
            try
            {
                ble.Call("addDisConnectListener", disCall);
                ble.Call("addConnectListener", contactCall);
                ble.Call("addBatteryVoltageListener", batteryCall);
                ble.Call("addRawDataListener4CSharp", rawBrainDataCallback);
                ble.Call("addHeartRateListener", heartRateDataCallback);
            }
            catch (Exception ex)
            {
                // 进行异常处理
                debugDelegate(ex.Message);
            }

        }
        /// 移除监听
        public void removeListener(ref RawBrainDataCallback rawBrainDataCallback, ref HeartRateDataCallback heartRateDataCallback, ref DisconnectCallback disCall, ref ContactCallback contactCall, ref BatteryCallback batteryCall)
        {
            if (ble == null)
            {
                // 未初始化，进行异常处理
                return;
            }
            ble.Call("removeDisConnectListener", disCall);
            ble.Call("removeContactListener", contactCall);
            ble.Call("removeBatteryVoltageListener", batteryCall);
            ble.Call("removeRawDataListener4CSharp", rawBrainDataCallback);
            ble.Call("removeHeartRateListener", heartRateDataCallback);
        }

        /// <summary>
        /// 开启蓝牙服务
        /// </summary>
        public void bleProcess()
        {
            if (ble == null)
            {
                // 未初始化，进行异常处理
                return;
            }

            ble.Call("startHeartAndBrainCollection");

        }

        /// <summary>
        /// 停止蓝牙服务
        /// </summary>
        public void bleStop()
        {
            if (ble == null)
            {
                // 未初始化，进行异常处理
                return;
            }


            ble.Call("stopHeartAndBrainCollection");
        }

        

    }






}
