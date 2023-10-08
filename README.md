# 回车情感云Unity集成

## 2023.6.25 更新说明
- 添加蓝牙配对绑定连接
    - Android.zip内依赖库更新, 解压后需要将`biomoduleble-1.5.2.aar`放到对应文件夹内, 删除`biomoduleble-1.4.2.aar`
    - 需要先到系统蓝牙设置里进行蓝牙绑定
    - 蓝牙配对连接接口 `BLEManager.instance.bleConnectBonded(ref bleScanSuccessCallback, ref bleScanFailedCallback, ref connectSuccessCallback, ref connectFailedCallback);` 

## 简介
回车情感云Unity版本是基于安卓SDK的封装, 使用详情请查看, 附件`Android.zip`中有打包好的SDK, 其中有一些安卓的配置文件内容, 包含依赖库, 权限等
 - 情感云连接API[安卓情感云SDK](https://github.com/Entertech/Enter-AffectiveCloud-Android-SDK), 
 - 蓝牙使用API[安卓蓝牙SDK](https://github.com/Entertech/Enter-Biomodule-BLE-Android-SDK), 
 - [安卓SDK集成Demo](https://github.com/Entertech/Enter-AffectiveCloud-Demo-Android)

## Unity版本封装说明

### 蓝牙连接

蓝牙连接部分分为两个文件, 
- SDK调用文件: BLEManager.cs
- SDK回调文件: BLECallbackDelegate.cs

#### Unity获取安卓实例
``` csharp
    AndroidJavaClass ajc; //java类
    AndroidJavaObject ajo; //java对象，此处为android界面
    ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); // 此处为默认命名, 有些第三方Unity库会修改
    if (ajc == null)
    {
        return;
    }
    try
    {

        ajo = ajc.GetStatic<AndroidJavaObject>("currentActivity");

    }
    catch (NullReferenceException ex)
    {
        return;
    }
```

#### 蓝牙回调
``` csharp
    //蓝牙扫描回调, 使用前先初始化
    BleScanSuccessCallback bleScanSuccessCallback; 
    //蓝牙扫描失败回调, 使用前先初始化
    BleScanFailedCallback bleScanFailedCallback;
    // 蓝牙连接成功回调, 使用前先初始化
    ConnectSuccessCallback connectSuccessCallback;
    // 蓝牙连接失败回调, 使用前先初始化
    ConnectFailedCallback connectFailedCallback;
    // 蓝牙脑波回调, 使用前先初始化
    RawBrainDataCallback rawBrainDataCallback;
    // 蓝牙心率回调, 使用前先初始化
    HeartRateDataCallback heartRateDataCallback;
```

#### BLEManager
以下方法按蓝牙使用的生命周期先后顺序排列
- 请求定位权限 `BLEManager.instance.requestAuth();`
  - 安卓要使用蓝牙必须要先请求定位


- 初始化蓝牙类 `BLEManager.instance.initializeJavaObject(ref ajo)` 
  - ajo参考上方安卓实例


- 蓝牙连接 `BLEManager.instance.bleScanAndConnect(ref bleScanSuccessCallback, ref bleScanFailedCallback, ref connectSuccessCallback, ref connectFailedCallback);` 
  - 回调参数参考回调文件
  - 蓝牙连接成功后,设备灯会由闪烁变为常亮
  - 添加蓝牙配对绑定连接
    - 需要先到系统蓝牙设置里进行蓝牙绑定
    - 蓝牙配对连接接口 `BLEManager.instance.bleConnectBonded(ref bleScanSuccessCallback, ref bleScanFailedCallback, ref connectSuccessCallback, ref connectFailedCallback);` 


- 蓝牙开启服务 `BLEManager.instance.bleProcess();` 
  - 回调参数参考回调文件
  - 开启服务后, 两个回调方法分别会收到脑波数据和心率数据
  

- 蓝牙关闭服务 `BLEManager.instance.bleStop();`
  - 蓝牙不再采集数据, 但是依然连接

- 蓝牙服务监听
  - 包含脑波, 心率, 断开连接, 佩戴检测, 电量
  - 添加 `BLEManager.instance.addListener(ref rawBrainDataCallback, ref heartRateDataCallback, ref disconnectCall, ref contactCall, ref batteryCall)`
  - 移除 `BLEManager.instance.removeListener(ref rawBrainDataCallback, ref heartRateDataCallback, ref disconnectCall, ref contactCall, ref batteryCall)`

- 蓝牙断开 `BLEManager.instance.bleDisconnect()`

### 情感云

情感云连接部分分为四个文件, 
- SDK调用文件: AffectiveManager.cs
- websocket回调文件:  WebSocketCallback.cs, Websocket状态和内容的回调, 内容的回调会打印所有通信数据,主要用于调试
- 情感云回调文件: AffectiveCallback.cs, 情感云服务相关的回调内容
- 情感云配置文件: AffectiveConfig.cs, 配置内容包括情感云的key和secret, 用于认证用户

#### Config
``` csharp
    public static string APP_KEY = ""; //向管理员申请（目前此Key只能作为测试用）
    public static string APP_SECRET = ""; //向管理员申请（目前此Secret只能作为测试用）
    public static string USER_ID = "2022";  //合作方分配给用户的ID，比如注册产生的用户id
```

#### Websocket回调
``` csharp
    // 连接断开回调, 使用需要初始化
    WebsocketDisconnectCallback websocketDisconnectCallback;
    // 连接成功回调, 使用需要初始化
    WebsocketConnectSuccessCallback websocketConnectSuccessCallback;
    // 请求内容打印, 用于调试, 使用需要初始化
    RequestDataCallback websocketRequestCallback;
    // 回复内容打印, 用于调试, 使用需要初始化
    ResponseDataCallback websocketResponseCallback;
```

#### 情感云回调
``` csharp
    // 实时生物数据(eeg, hr等)回调
    RealtimeBiodataCallback realtimeBiodataCallback;
    // 实时情感数据回调(注意力,放松度等分析数据)回调
    RealtimeAffectiveDataCallback realtimeAffectiveDataCallback;
    // 情感云初始化回调
    CloudManagerInitCallback cloudInitCallback;
    // 情感云断开重连回调
    CloudManagerRestoreCallback cloudManagerRestoreCallback;
```

#### AffectiveManager
以下方法按蓝牙使用的生命周期先后顺序排列
``` csharp
//一，初始化情感云
AffectiveManager.instance.buildBioDataService();

//二，创建服务并认证，关于回调，请参考AffectiveCallback.cs内的注释
AffectiveManager.instance.createSession(ref cloudInitCallback);

//从这里开始,可以获取sessionid
if (AffectiveManager.instance.isSessionCreate())
    var sessionID = AffectiveManager.instance.getSessionId();

//三，创建监听，参考WebSocketCallback.cs，和AffectiveCallback.cs注释
AffectiveManager.instance.addListener(ref websocketConnectSuccessCallback, 
    ref websocketDisconnectCallback, ref realtimeBiodataCallback, 
    ref realtimeAffectiveDataCallback)
    
//四，从蓝牙的脑波回调和心率回调持续上传数据，相关代码查看BLECallbackDelegate.cs
AffectiveManager.instance.appendEEG(AndroidJavaObject list);
AffectiveManager.instance.appendHR(int value);

//五，情感云断开重连，当监听到WebSocketCallback.cs的情感云非正常断开时，重新连接并认证
AffectiveManager.instance.restore(ref cloudManagerRestoreCallback);

//六，结束服务
AffectiveManager.instance.releaseCloud()

//七，断开情感云, 如果需要在上面的结束服务命令后断开, 需要等待ReleaseCallback返回
AffectiveManager.instance.closeWebSocket()
```
