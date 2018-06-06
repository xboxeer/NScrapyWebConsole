using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NScrapyWebConsole.Redis
{
    public class RedisManager
    {
        private static object lockObj = new object();
        private static RedisManager instance;
        //public static ConnectionMultiplexer Connection { get; private set; }
        private RedisManager()
        {

        }

        public static RedisManager Current
        {
            get
            {
                lock(lockObj)
                {
                    if(instance==null)
                    {
                        instance = new RedisManager();
                    }
                    return instance;
                }
            }
        }

        public async Task<string> GetNodeStatus()
        {
            //Lets first return some mock data here
            return "{DownloaderCapbility:64,RunningDownloaders:5}";
        }
        /// <summary>
        /// Specify the Redis endpoint address
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// Specify the Redis endpoint port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Specify the Scheduler.RedisExt.ReceiverQueue in the appsetting.json of DownloaderShell
        /// </summary>
        public string DownloaderDomainName { get; set; }
        /// <summary>
        /// Specify the Scheduler.RedisExt.ResponseQueue in the appsetting.json of DownloaderShell
        /// </summary>
        public string SpiderDomainName { get; set; }
    }
}
