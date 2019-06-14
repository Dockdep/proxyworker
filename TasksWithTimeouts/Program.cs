using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TasksWithTimeouts
{
    public class Program
    {
        

        public static async Task Main(string[] args)
        {
            var rotator = new ProxyRotatorService();
            rotator.ChangeProxy();
            while (true)
            {
   
                try
                {
                    await rotator.WebAccessViaProxyRotatorApi("https://www.expressvpn.com/ru/what-is-my-ip");
                    Thread.Sleep(1000);
                    rotator.ChangeProxy();
                }
                catch (Exception io)
                {
                    Console.WriteLine("Exception:"+io.Message);
                    
                    rotator.ChangeProxy();
     
                    Thread.Sleep(1000);
                } 
            }
     

        }
    }
}