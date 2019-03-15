using System;
using System.Collections.Generic;
using System.Messaging;
using Common;
using DTO;

namespace Server
{
    public class ServerProcess
    {
        private int _productId = 1;

        public ServerProcess(bool createQueueIfMissing)
        {
            if (createQueueIfMissing)
            {
                if (MessageQueueDoesNotExists(QueuePaths.TextMessageQueuePath))
                {
                    MessageQueue.Create(QueuePaths.TextMessageQueuePath);
                }

                if (MessageQueueDoesNotExists(QueuePaths.ObjectMessageQueuePath))
                {
                    MessageQueue.Create(QueuePaths.ObjectMessageQueuePath);
                }
            }
        }

        public void SaveTextToQueue()
        {
            var message = new Message
            {
                Label = $"{DateTime.Now} - {nameof(ServerProcess)}.{StackTraceUtils.GetCalledMethodName()}",
                Body = "Body with text"
            };

            using (var messageQueue = new MessageQueue(QueuePaths.TextMessageQueuePath))
            {
                messageQueue.Send(message);
            }
        }

        public void SaveObjectToQueue()
        {
            var message = new Message
            {
                Label = $"{DateTime.Now} -{nameof(ServerProcess)}.{StackTraceUtils.GetCalledMethodName()}",
                Body = GenerateProductList()
            };

            using (var messageQueue = new MessageQueue(QueuePaths.ObjectMessageQueuePath))
            {
                messageQueue.Send(message);
            }
        }

        private List<Product> GenerateProductList()
        {
            var firstProductId = _productId++;
            var secondProductId = _productId++;

            return new List<Product>()
            {
                new Product{ Id = firstProductId, Name = $"Product {firstProductId}" },
                new Product{ Id = secondProductId, Name = $"Product {secondProductId}" }
            };
        }

        private bool MessageQueueDoesNotExists(string path)
        {
            return !MessageQueue.Exists(path);
        }
    }
}
