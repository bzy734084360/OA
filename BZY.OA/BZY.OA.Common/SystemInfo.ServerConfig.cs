
namespace BZY.OA.Common
{
    public partial class SystemInfo
    {

        /// <summary>
        /// 服务器缓存类型
        /// </summary>
        public static string CacheType = CacheTypeEnum.Redis.ToDescription();

        /// <summary>
        /// 服务器缓存Key前缀
        /// </summary>
        public static string CachePrefix = "BZY_";


    }
}