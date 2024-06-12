using Memcached.ClientLibrary;
using System;

namespace BZY.OA.MemcacheDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //memcache服务端与客户端版本一致问题，先按照视频中学习
            string[] serverlist = { "127.0.0.1:11211", "10.0.0.132:11211" };
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

            // 获得客户端实例
            MemcachedClient mc = new MemcachedClient();
            mc.EnableCompression = false;

            Console.WriteLine("------------测  试-----------");
            mc.Set("test", "my value");  //存储数据到缓存服务器，这里将字符串"my value"缓存，key 是"test"

            if (mc.KeyExists("test"))   //测试缓存存在key为test的项目
            {
                Console.WriteLine("test is Exists");
                Console.WriteLine(mc.Get("test").ToString());  //在缓存中获取key为test的项目
            }
            else
            {
                Console.WriteLine("test not Exists");
            }

            Console.ReadLine();

            mc.Delete("test");  //移除缓存中key为test的项目

            if (mc.KeyExists("test"))
            {
                Console.WriteLine("test is Exists");
                Console.WriteLine(mc.Get("test").ToString());
            }
            else
            {
                Console.WriteLine("test not Exists");
            }
            Console.ReadLine();

            SockIOPool.GetInstance().Shutdown();  //关闭池， 关闭sockets

            //新版使用
            //var configuration = new MemcachedClientConfiguration();
            //configuration.Servers.Add(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211)); // 替换为你的Memcached服务器地址和端口
            //configuration.Protocol = MemcachedProtocol.Text;
            //MemcachedClient mc = new MemcachedClient(configuration);

            //mc.Store(StoreMode.Add, "test", "hello world");
            //object cacheObj = mc.Get("test").ToString();

            //Console.WriteLine("获取的缓存：" + cacheObj);

            //Console.ReadLine();

            //MemcachedClient mc = new MemcachedClient();

            //mc.Store(StoreMode.Set, "test", "hello world");
            //object cacheObj = mc.Get("test").ToString();

            //Console.WriteLine("获取的缓存：" + cacheObj);

            //Console.ReadLine();

        }
    }
}
