namespace BZY.OA.Common.Cache
{
    /// <summary>
    /// 缓存工厂类
    /// </summary>
    public class CacheFactory
    {
        public static ICache Cache()
        {
            string cacheType = SystemInfo.CacheType;
            switch (cacheType)
            {
                case "Redis":
                    return new RedisCache();
                case "WebCache":
                    return new WebCache();
                default:
                    return new WebCache();
            }
        }
    }
}
