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
        Text log;
        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数，此处只是传入两个参数作为例子
        /// </summary>
        /// <param name="list">需要获取的信息，此处作为例子</param>
        /// <param name="obj">安卓的activity界面,用于安卓调试</param>
        /// <returns></returns>
        public CloudManagerInitCallback(ref Text text) : base("cn.entertech.affectivecloudsdk.interfaces.Callback")
        {
            log = text;
        }

        /// <summary>
        /// 回调触发
        /// </summary>
        public void onError(AndroidJavaObject error)
        {
            try
            {
                var message = error.Call<string>("toString");
                log.text = message;

            }
            catch (Exception ex)
            {

            }
        }

        public void onSuccess()
        {

            log.text = "Get Session";
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
        /// </summary>
        /// <param name="obj">安卓的activity界面</param>
        /// <returns></returns>
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
                // var realtimeHrData = jo.Get<AndroidJavaObject>("realtimeHrData");
                // if (realtimeHrData != null)
                // {
                //     var hrObject = realtimeHrData.Get<AndroidJavaObject>("hr");
                //     var hr = hrObject.Call<int>("intValue"); //数据为心率数据

                //     var hrvObject = realtimeHrData.Get<AndroidJavaObject>("hrv");
                //     var hrv = hrvObject.Call<int>("intValue");//心率变异性
                // }
                var realtimeEEGData = jo.Get<AndroidJavaObject>("realtimeEEGData");
                if (realtimeEEGData != null)
                {
                    using (var leftObject = realtimeEEGData.Get<AndroidJavaObject>("leftwave"))
                    {
                        int leftCount = leftObject.Call<int>("size");

                        for (int i = 0; i < leftCount; i += 2)
                        {
                            if (AffectiveCloudGloble.SharedInstance.leftEEGList.Count < 120)
                            {
                                AffectiveCloudGloble.SharedInstance.leftEEGList.Enqueue(leftObject.Call<AndroidJavaObject>("get", i).Call<int>("intValue"));
                            }

                        }
                    }
                    using (var rightObject = realtimeEEGData.Get<AndroidJavaObject>("rightwave"))
                    {
                        int rightCount = rightObject.Call<int>("size");
                        for (int i = 0; i < rightCount; i += 2)
                        {
                            if (AffectiveCloudGloble.SharedInstance.leftEEGList.Count < 120)
                            {
                                AffectiveCloudGloble.SharedInstance.rightEEGList.Enqueue(rightObject.Call<AndroidJavaObject>("get", i).Call<int>("intValue"));
                            }

                        }
                    }
                    using (var quality = realtimeEEGData.Get<AndroidJavaObject>("quality"))
                    {
                        AffectiveCloudGloble.SharedInstance.quality = quality.Call<int>("intValue");
                        if (AffectiveCloudGloble.SharedInstance.quality < 2) {
                            AffectiveCloudGloble.SharedInstance.ssvepNoneValue += 1;
                        }
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
                using (var realtimeAttentionData = jo.Get<AndroidJavaObject>("realtimeAttentionData"))
                {
                    if (realtimeAttentionData != null)
                    {
                        var attentionObject = realtimeAttentionData.Get<AndroidJavaObject>("attention");
                        var attention = attentionObject.Call<int>("intValue"); //输出的注意力
                        AffectiveCloudGloble.SharedInstance.attention = attention;
                        if (AffectiveCloudGloble.SharedInstance.attentionList.Count < 4)
                            AffectiveCloudGloble.SharedInstance.attentionList.Enqueue(attention);
                    }

                }

                using (var realtimeCoherenceData = jo.Get<AndroidJavaObject>("realtimeRelaxationData"))
                {
                    if (realtimeCoherenceData != null)
                    {
                        var coherenceObject = realtimeCoherenceData.Get<AndroidJavaObject>("relaxation");
                        var coherence = coherenceObject.Call<int>("intValue"); //输出的和谐度
                        AffectiveCloudGloble.SharedInstance.relaxation = coherence;
                        if (AffectiveCloudGloble.SharedInstance.relaxationList.Count < 4)
                            AffectiveCloudGloble.SharedInstance.relaxationList.Enqueue(coherence);

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

        /// <summary>
        /// 回调初始化，初始化时可传入参数获取回调内容，或者传入界面进行内容显示，
        /// 此处可根据业务需求修改参数个数，此处只是传入两个参数作为例子
        /// </summary>
        /// <param name="list">需要获取的信息，此处作为例子</param>
        /// <param name="obj">安卓的activity界面,用于安卓调试</param>
        /// <returns></returns>
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
        }

        public void onSuccess()
        {
        }
    }


    /// <summary>
    /// restore监听
    /// </summary>
    public class CloudManagerRestoreCallback : AndroidJavaProxy
    {
        public event OnWebsocketDebug restoreDelegate;
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
            try
            {
                var message = error.Call<string>("toString");
                restoreDelegate(message);
            }
            catch (Exception ex)
            {
                restoreDelegate(ex.Message);
            }
        }

        public void onSuccess()
        {
            restoreDelegate("Restore success");
        }
    }


}