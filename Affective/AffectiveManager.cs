using System.Collections;
using UnityEngine;
using System;

namespace Enter.Assets.Scripts
{
    public class AffectiveManager
    {
        private static string ENTER_AFFECTIVE_MANAGER = "cn.entertech.affectivecloudsdk.EnterAffectiveCloudManager";
        private static string ENTER_BIODATA_PARAM = "cn.entertech.affectivecloudsdk.BiodataSubscribeParams$Builder";
        private static string ENTER_AFFECTIVE_CONFIG = "cn.entertech.affectivecloudsdk.EnterAffectiveCloudConfig$Builder";
        private static string ENTER_AFFECTIVE_PARAM = "cn.entertech.affectivecloudsdk.AffectiveSubscribeParams$Builder";
        // private static string ENTER_EEG_PARAM = "cn.entertech.affectivecloudsdk.AlgorithmParamsEEG$Builder";
        // private static string ENTER_EEG_FILTER_MODE = "cn.entertech.affectivecloudsdk.AlgorithmParamsEEG$FilterMode";
        // private static string ENTER_CLOUD_PARAM = "cn.entertech.affectivecloudsdk.AlgorithmParams$Builder";
        private static string ENTER_SERVICE_ENUM = "cn.entertech.affectivecloudsdk.entity.Service";
        public AndroidJavaObject manager = null;
        public event OnWebsocketDebug debugDelegate;
        private static AffectiveManager mInstance = null;
        public static AffectiveManager instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new AffectiveManager();
                }

                return mInstance;
            }
        }

        /// <summary>
        /// 进行情感云初始化，初始化结果从 'CloudManagerInitCallback' 回调方法进行判断
        /// 此处会用到情感云的key 和 secret 从回车科技获取
        /// </summary>
        private void initService()
        {

            AndroidJavaClass serviceEnum = new AndroidJavaClass(ENTER_SERVICE_ENUM);
            var bioService = new AndroidJavaObject("java.util.ArrayList");
            var affectiveService = new AndroidJavaObject("java.util.ArrayList");
            AndroidJavaObject bioSubscribe = null;
            AndroidJavaObject affectiveSubscribe = null;
            AndroidJavaObject affectiveConfig = null;
            AndroidJavaObject eegParam = null;
            AndroidJavaObject cloudParam = null;
            try
            {
                // 订阅基础数据，此处EEG，HR为可选，不可为空
                using (var bioBuilderObject = new AndroidJavaObject(ENTER_BIODATA_PARAM))
                {
                    // 构造器写法 此处订阅了eeg和hr
                    bioSubscribe = bioBuilderObject.Call<AndroidJavaObject>("requestEEG")
                                                    .Call<AndroidJavaObject>("requestHR")
                                                        .Call<AndroidJavaObject>("build");
                    bioService.Call<bool>("add", serviceEnum.GetStatic<AndroidJavaObject>("EEG"));
                    bioService.Call<bool>("add", serviceEnum.GetStatic<AndroidJavaObject>("HR"));
                }

                using (var affectiveObject = new AndroidJavaObject(ENTER_AFFECTIVE_PARAM))
                {
                    // 构造器写法 此处订阅了注意力和放松压力值，更多请查看回车情感云文档
                    affectiveSubscribe = affectiveObject.Call<AndroidJavaObject>("requestAttention")
                                                        .Call<AndroidJavaObject>("requestRelaxation")
                                                        .Call<AndroidJavaObject>("requestPressure")
                                                        .Call<AndroidJavaObject>("build");
                    affectiveService.Call<bool>("add", serviceEnum.GetStatic<AndroidJavaObject>("ATTENTION"));
                    affectiveService.Call<bool>("add", serviceEnum.GetStatic<AndroidJavaObject>("RELAXATION"));
                    affectiveService.Call<bool>("add", serviceEnum.GetStatic<AndroidJavaObject>("PRESSURE"));
                }


                // 完成配置项， 情感云配置内容在 ‘AffectiveConfig’
                using (var configObject = new AndroidJavaObject(ENTER_AFFECTIVE_CONFIG,
                AffectiveConfig.APP_KEY, AffectiveConfig.APP_SECRET, AffectiveConfig.USER_ID))
                {
                    affectiveConfig = configObject.Call<AndroidJavaObject>("url", AffectiveConfig.AFFECTIVE_URL)
                    .Call<AndroidJavaObject>("timeout", 10000)
                    .Call<AndroidJavaObject>("uploadCycle", 1)
                    .Call<AndroidJavaObject>("availableBiodataServices", bioService)
                    .Call<AndroidJavaObject>("availableAffectiveServices", affectiveService)
                    .Call<AndroidJavaObject>("biodataSubscribeParams", bioSubscribe)
                    .Call<AndroidJavaObject>("affectiveSubscribeParams", affectiveSubscribe)
                    .Call<AndroidJavaObject>("build");
                }
                manager = new AndroidJavaObject(ENTER_AFFECTIVE_MANAGER, affectiveConfig);

            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }

        }

        public void createSession(ref CloudManagerInitCallback callback)
        {
            if (manager != null)
            {
                manager.Call("init", callback);
            }
                    }

        public bool isWebsocketConnect()
        {
            try 
            {
                if (manager != null)
                {
                    bool value = manager.Get<AndroidJavaObject>("mApi").Call<bool>("isWebSocketOpen");
                    
                    return value;
                }
                
            } catch (Exception ex) 
            {
                debugDelegate(ex.Message);
            }
            return false;
        }


        public bool isSessionCreate() 
        {
            try 
            {
                if (manager != null)
                {
                    bool value = manager.Get<AndroidJavaObject>("mApi").Call<bool>("isSessionCreated");
                    
                    return value;
                }
                
            } catch (Exception ex) 
            {
                debugDelegate(ex.Message);
            }
            return false;
        }

        public string getSessionId()
        {
            try 
            {
                if (manager != null)
                {
                    string value = manager.Get<AndroidJavaObject>("mApi").Call<string>("getSessionId");
                      return value;
                }
                
            } catch (Exception ex) 
            {
                debugDelegate(ex.Message);
            }
            return null;
        }

        public string getDeviceUniqueId() 
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
        /// <summary>
        /// 开启情感云
        /// </summary>
        public void buildBioDataService()
        {
            if (manager == null)
            {
                initService();
            }
        }

        public bool bIsInit()
        {
            return manager != null;
        }

        public void addDebugListener(ref RequestDataCallback reqCallback, ref ResponseDataCallback resCallback)
        {
                manager.Call("addRawJsonRequestListener", reqCallback);
                manager.Call("addRawJsonResponseListener", resCallback);
        }

        /// <summary>
        /// 添加情感云返回数据监听
        /// </summary>
        public void addListener(ref WebsocketConnectSuccessCallback wsSuccessCallback,
            ref WebsocketDisconnectCallback wsDisconnectCallback,
            ref RealtimeBiodataCallback rbCallback,
            ref RealtimeAffectiveDataCallback raCallback
        )
        {
            if (manager == null)
            {
                return;
            }
            try
            {
                // websocket连接监听
                manager.Call("addWebSocketConnectListener", wsSuccessCallback);
                // websocket断开监听
                manager.Call("addWebSocketDisconnectListener", wsDisconnectCallback);
                // 脑波心率等基础值监听
                manager.Call("addBiodataRealtimeListener", rbCallback);
                // 注意力，和谐度等分析值监听
                manager.Call("addAffectiveDataRealtimeListener", raCallback);
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }

        }

        /// <summary>
        /// 添加心率数据
        /// </summary>
        /// <param name="hr"></param>
        public void appendHR(int value)
        {
            if (manager == null)
            {
                return;
            }
            try
            {
                manager.Call("appendHeartRateData", value);
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }

        }

        /// <summary>
        /// 添加脑波数据
        /// </summary>
        /// <param name="eeg"></param>
        public void appendEEG(AndroidJavaObject list)
        {
            if (manager == null)
            {
                return;
            }
 
            try
            {
                
                manager.Call("appendEEGData", list.Get<AndroidJavaObject>("Buffer"));
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }
        }

        /// <summary>
        /// 释放情感云
        /// </summary>
        public void releaseCloud()
        {
            if (manager == null)
            {
                return;
            }
            try
            {
                manager.Call("release", new CloudManagerReleaseCallback());
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }
        }

        /// <summary>
        /// websocket 关闭
        /// </summary>
        public void closeWebSocket()
        {
            if (manager == null)
            {
                return;
            }
            try
            {
                manager.Call("closeWebSocket");
            }
            catch (Exception ex)
            {
                //异常处理
                debugDelegate(ex.Message);
            }

        }

        /// <summary>
        /// 当websocket断开导致情感云服务无法使用时，调用restore，
        /// 会在重新连接情感云，在同一个session服务下继续当前服务
        /// </summary>
        public void restore(ref CloudManagerRestoreCallback callback)
        {
            if (manager == null)
            {
                return;
            }
            try
            {
                manager.Call("restore", callback);
            }
            catch (Exception ex)
            {
                //异常处理
                debugDelegate(ex.Message);
            }
        }

        public void getReport(ref CloudManagerReportCallback biodata, ref CloudManagerReportCallback affective) 
        {
            if (manager == null)
            {
                return;
            }
            manager.Call("getBiodataReport", biodata);
            manager.Call("getAffectiveDataReport", affective);
        }


    }
}