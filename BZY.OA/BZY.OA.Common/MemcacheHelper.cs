using Memcached.ClientLibrary;
using System;

namespace BZY.OA.Common
{
    /// <summary>
    /// Memcache帮助类
    /// </summary>
    public class MemcacheHelper
    {
        private static MemcachedClient mc;
        static MemcacheHelper()
        {
            //memcache服务端与客户端版本一致问题，先按照视频中学习
            //可能这个地方需要通过配置文件获取 新版本无法兼容已有的memcached服务端
            string[] serverlist = { "127.0.0.1:11211" };
            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            mc = new MemcachedClient();
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            mc.Set(key, value);
        }
        /// <summary>
        /// 设置缓存包含过期时间 最长30天
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value, DateTime time)
        {
            mc.Set(key, value, time);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        public static object Get(string key)
        {
            return mc.Get(key);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
                // 删除键
                return mc.Delete(key);
            }
            return false;
        }
    }
}
