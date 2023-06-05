using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Enter.Assets.Scripts
{
    public enum WebSocketState
    {
        connected,
        disconnected,
    }
    public delegate void OnWebsocketChanged(WebSocketState state);
    public delegate void OnWebsocketDebug(string data);
    public delegate void OnSessionCreated(bool flag);
    public delegate void OnSessionClosed();
    public delegate void OnReportCreated(int index);
    /// <summary>
    /// websocket request 回调
    /// </summary>
    public class RequestDataCallback : AndroidJavaProxy
    {
        public event OnWebsocketDebug debugDelegate;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ajo">为了调试传入android界面对象，可以自定义传入对象，进行输出</param>
        /// <returns></returns>
        public RequestDataCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(string jo)
        {
            debugDelegate("Send: " + jo);
            
        }
    }

    /// <summary>
    /// websocket response 回调
    /// </summary>
    public class ResponseDataCallback : AndroidJavaProxy
    {
        public event OnWebsocketDebug debugDelegate;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ajo">为了调试传入android界面对象，可以自定义传入对象，进行输出</param>
        /// <returns></returns>
        public ResponseDataCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(string jo)
        {
            debugDelegate("Receive: " + jo);
            
        }
    }


    /// <summary>
    /// websocket 连接成功 回调
    /// </summary>
    public class WebsocketConnectSuccessCallback : AndroidJavaProxy
    {
        public event OnWebsocketChanged connectState;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ajo">为了调试传入android界面对象，可以自定义传入对象，进行输出</param>
        /// <returns></returns>
        public WebsocketConnectSuccessCallback() : base("kotlin.jvm.functions.Function0")
        {
            
        }
        public void invoke()
        {
            connectState(WebSocketState.connected);
            
        }
    }

    /// <summary>
    /// websocket 断开 回调
    /// </summary>
    public class WebsocketDisconnectCallback : AndroidJavaProxy
    {

        public event OnWebsocketChanged connectState;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ajo">为了调试传入android界面对象，可以自定义传入对象，进行输出</param>
        /// <returns></returns>
        public WebsocketDisconnectCallback() : base("kotlin.jvm.functions.Function1")
        {
            
        }
        public void invoke(string jo)
        {
            connectState(WebSocketState.disconnected);
        }
    }

}