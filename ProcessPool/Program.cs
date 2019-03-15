using Client;
using Server;
using System;
using System.Threading;

namespace ProcessPool
{
    class Program
    {
        private const bool RemoveQueueItemAfterRetreive = true;
        private static readonly ServerProcess _serverProcess = new ServerProcess(true);
        private static readonly ClientProcess _clientProcess = new ClientProcess();

        private static volatile bool _stopServerThread;
        private static volatile bool _stopTextClientThread;
        private static volatile bool _stopObjectClientThread;

        public static void Main(string[] args)
        {
            RunNewThread(WriteDataToQueue, "Server");
            RunNewThread(ReadTextQueueItems, "TextClient");
            RunNewThread(ReadObjectQueueItems, "ObjectClient");

            Console.WriteLine("Threads are started. Press any key to stop threads!");
            Console.ReadLine();

            StopThreads();
            Environment.Exit(0);
        }


        private static void RunNewThread(ThreadStart threadStart, string nameSuffix)
        {
            var threadServer = new Thread(threadStart)
            {
                Name = $"Thread_{nameSuffix}"
            };
            threadServer.Start();
        }


        private static void WriteDataToQueue()
        {
            while (!_stopServerThread)
            {
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        _serverProcess.SaveTextToQueue();
                        _serverProcess.SaveObjectToQueue();
                        Console.WriteLine("**New Queue items added to MSMQ**");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Create new items with 1 second delay
                Thread.Sleep(1000);
            }
        }

        private static void ReadTextQueueItems()
        {
            //Let Queue Read action wait for some items to be created
            Thread.Sleep(2000);
            while (!_stopTextClientThread)
            {
                try
                {
                    var textReceived = _clientProcess.GetTextFromQueue(RemoveQueueItemAfterRetreive);
                    Console.WriteLine($"Time: {DateTime.Now}, Text: {textReceived}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void ReadObjectQueueItems()
        {
            //Let Queue Read action wait for some items to be created
            Thread.Sleep(2000);
            while (!_stopObjectClientThread)
            {
                try
                {
                    var objectReceived = _clientProcess.GetObjectFromQueue(RemoveQueueItemAfterRetreive);
                    foreach (var product in objectReceived)
                    {
                        Console.WriteLine($"Time: {DateTime.Now}, Product: {product}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void StopThreads()
        {
            Console.WriteLine("Stopping thread: Server");
            _stopServerThread = true;

            Thread.Sleep(500);
            Console.WriteLine("Stopping thread: TextClient");
            _stopTextClientThread = true;

            Thread.Sleep(500);
            Console.WriteLine("Stopping thread: ObjectClient");
            _stopObjectClientThread = true;
        }
    }
}
