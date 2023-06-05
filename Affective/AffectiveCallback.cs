using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

namespace Enter.Assets.Scripts
{
    /// <summary>
    /// 情感云初始化监听
    /// </summary>
    public class CloudManagerInitCallback : AndroidJavaProxy
    {
        public event OnSessionCreated sessionDelegate;
        public event OnWebsocketDebug debugDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示
        public CloudManagerInitCallback() : base("cn.entertech.affectivecloudsdk.interfaces.Callback")
        {
            
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void onError(AndroidJavaObject error)
        {
            sessionDelegate(false);
            try
            {
                var message = error.Call<string>("toString");
                debugDelegate(message);

            }
            catch (Exception ex)
            {
                debugDelegate(ex.ToString());
            }
        }

        public void onSuccess()
        {
            sessionDelegate(true);
        }
    }

    /// <summary>
    /// 实时生物数据回调函数，包括心率，心率变异性，脑波
    /// </summary>
    public class RealtimeBiodataCallback : AndroidJavaProxy
    {
        public event OnWebsocketDebug debugDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示
        public RealtimeBiodataCallback() : base("kotlin.jvm.functions.Function1")
        {

        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(AndroidJavaObject jo)
        {
            // 解析实时数据
            try
            {
                //此处是心率解析
                var realtimeHrData = jo.Get<AndroidJavaObject>("realtimeHrData");
                if (realtimeHrData != null)
                {
                    var hrObject = realtimeHrData.Get<AndroidJavaObject>("hr");
                    var hr = hrObject.Call<int>("intValue"); //数据为心率数据
                    // 代理传出去，或者给某个全局变量赋值

                    var hrvObject = realtimeHrData.Get<AndroidJavaObject>("hrv");
                    var hrv = hrvObject.Call<int>("intValue");//心率变异性
                    // 代理传出去，或者给某个全局变量赋值
                }
                //脑波检测
                var realtimeEEGData = jo.Get<AndroidJavaObject>("realtimeEEGData");
                if (realtimeEEGData != null)
                {
                    using (var leftObject = realtimeEEGData.Get<AndroidJavaObject>("leftwave")) //左脑波
                    {
                        // 代理传出去，或者给某个全局变量赋值, 解析方法如下
                        int leftCount = leftObject.Call<int>("size");
                        for (int i = 0; i < leftCount; i += 2)
                        {
                            //数据处理
                            var leftValue = leftObject.Call<AndroidJavaObject>("get", i).Call<int>("intValue");
                        }
                    }
                    using (var rightObject = realtimeEEGData.Get<AndroidJavaObject>("rightwave")) //右脑波
                    {   
                        // 代理传出去，或者给某个全局变量赋值, 解析方法如下
                        int rightCount = rightObject.Call<int>("size");
                        for (int i = 0; i < rightCount; i += 2)
                        {

                            //数据处理
                            var rightValue = rightObject.Call<AndroidJavaObject>("get", i).Call<int>("intValue");
                        }
                    }
                    using (var quality = realtimeEEGData.Get<AndroidJavaObject>("quality"))  //信号质量
                    {
                        // 代理传出去，或者给某个全局变量赋值, 解析方法如下
                        int quality = quality.Call<int>("intValue");
                    }

                }
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }

        }

    }

    /// <summary>
    /// 实时情感数据回调函数，包括和谐度，注意力，放松度等
    /// </summary>
    public class RealtimeAffectiveDataCallback : AndroidJavaProxy
    {
        public event OnWebsocketDebug debugDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示
        /// </summary>
        /// <param name="obj">安卓的activity界面</param>
        /// <returns></returns>
        public RealtimeAffectiveDataCallback() : base("kotlin.jvm.functions.Function1")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void invoke(AndroidJavaObject jo)
        {
            // 解析实时数据
            try
            {
                //  取值范围[0-100]，0一般代表消极状态，100代表积极状态
                //  realtimeAttentionData  注意力数据  attention
                //  realtimeRelaxationData 放松度数据  relaxation
                //  realtimePressureData   压力数据    pressure
                //  realtimePleasureData   愉悦度数据  pleasure
                //  realtimeCoherenceData  和谐度     coherence
                // 这里用注意力举例,解析方法如下: 
                using (var realtimeAttentionData = jo.Get<AndroidJavaObject>("realtimeAttentionData"))
                {
                    if (realtimeAttentionData != null)
                    {
                        var attentionObject = realtimeAttentionData.Get<AndroidJavaObject>("attention");
                        var attention = attentionObject.Call<int>("intValue"); //输出的注意力
                        // 代理传出去，或者给某个全局变量赋值
                    }

                }

            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }

        }

    }

    /// <summary>
    /// 情感云释放监听
    /// </summary>
    public class CloudManagerReleaseCallback : AndroidJavaProxy
    {
        public event OnSessionClosed sessionDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示
        public CloudManagerReleaseCallback() : base("cn.entertech.affectivecloudsdk.interfaces.Callback")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void onError(AndroidJavaObject error)
        {
            try
            {
                var message = error.Call<string>("toString");
            }
            catch (Exception ex)
            {
            }
            sessionDelegate();
        }

        public void onSuccess()
        {
            sessionDelegate();
        }
    }


    /// <summary>
    /// restore监听
    /// </summary>
    public class CloudManagerRestoreCallback : AndroidJavaProxy
    {
        public event OnSessionCreated sessionDelegate;
        public event OnWebsocketDebug debugDelegate;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数
        /// </summary>
        /// <param name="obj">安卓的activity界面,用于安卓调试</param>
        /// <returns></returns>
        public CloudManagerRestoreCallback() : base("cn.entertech.affectivecloudsdk.interfaces.Callback")
        {
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void onError(AndroidJavaObject error)
        {
            sessionDelegate(false);
            try
            {
                var message = error.Call<string>("toString");
                debugDelegate(message);
            }
            catch (Exception ex)
            {
                debugDelegate(ex.Message);
            }
        }

        public void onSuccess()
        {
            sessionDelegate(true);
        }
    }

    /// <summary>
    /// 情感云报表回调
    /// </summary>
    public class CloudManagerReportCallback : AndroidJavaProxy
    {
        
        public CloudManagerReportCallback() : base("cn.entertech.affectivecloudsdk.interfaces.Callback2")
        {
            
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void onError(AndroidJavaObject error)
        {
            try
            {
                var message = error.Call<string>("toString");
            }
            catch (Exception ex)
            {
            }
        }

        public void onSuccess(AndroidJavaObject javaObj)
        {
            // 这里报表解析字段参考情感云说明,解析方法参考上面实时数据
        }
    }
}