using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigStarFK.Utilities;
using ServiceStack.Redis;

namespace BigStarFK.Cache.Redis
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public class Cache : ICache
    {
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            try
            {
                return RedisCache.Get<T>(SystemInfo.CachePrefix + cacheKey);
            }
            catch
            {
                return null;
            }            
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public void WriteCache<T>(T value, string cacheKey) where T : class
        {
            //RedisCache.Set(cacheKey, value);
            WriteCache(value, cacheKey, DateTime.Now.AddDays(2));
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            RedisCache.Set(SystemInfo.CachePrefix + cacheKey, value, expireTime);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void RemoveCache(string cacheKey)
        {
            RedisCache.Remove(SystemInfo.CachePrefix + cacheKey);
        }

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
        public void RemoveByPattern(string pattern)
        {
            RedisCache.RemoveByPattern(SystemInfo.CachePrefix + pattern);
        }        

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public void RemoveCache()
        {
            RedisCache.RemoveAll();
        }
    }
}
