namespace Enter.Assets.Scripts
{
    public struct AffectiveConfig
    {
        public static string APP_KEY = ""; //向管理员申请（目前此Key只能作为测试用）
        public static string APP_SECRET = ""; //向管理员申请（目前此Secret只能作为测试用）
        public static string USER_ID = "10001";  //用户的ID，比如注册产生的用户id
        public static string AFFECTIVE_URL = "wss://server.affectivecloud.cn/ws/algorithm/v2/"; 
    }
}