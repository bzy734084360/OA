using System;

namespace BZY.OA.Common.Cache
{
    /// <summary>
    /// 定义缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        T GetCache<T>(string cacheKey) where T : class;

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        void WriteCache<T>(T value, string cacheKey) where T : class;

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class;

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        void RemoveCache(string cacheKey);

        /// <summary>
        /// 通过模式匹配移除指定数据缓存
        /// </summary>
        /// <remarks>
        /// 语法：KEYS pattern 
        /// 说明：返回与指定模式相匹配的所用的keys。 
        /// 该命令所支持的匹配模式如下： 
        /// （1）?：用于匹配单个字符。例如，h? llo可以匹配hello、hallo和hxllo等； 
        /// （2）*：用于匹配零个或者多个字符。例如，h* llo可以匹配hllo和heeeello等； 
        /// （3）[]：可以用来指定模式的选择区间。例如h[ae] llo可以匹配hello和hallo，但是不能匹配hillo。 
        /// 同时，可以使用“/”符号来转义特殊的字符
        /// </remarks>
        /// <param name="pattern">指定模式</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        void RemoveCache();
    }
}
