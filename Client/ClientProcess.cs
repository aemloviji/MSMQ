using System;
using System.Collections.Generic;
using System.Messaging;
using Common;
using DTO;

namespace Client
{
    public class ClientProcess
    {
        public string GetTextFromQueue(bool shouldRemove)
        {
            using (var messageQueue = new MessageQueue(QueuePaths.TextMessageQueuePath))
            {
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                Message message = shouldRemove ? messageQueue.Receive() : messageQueue.Peek();
                return message.Body.ToString();
            }
        }

        public IEnumerable<Product> GetObjectFromQueue(bool shouldRemove)
        {
            using (var messageQueue = new MessageQueue(QueuePaths.ObjectMessageQueuePath))
            {
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Product[]) });

                Message message = shouldRemove ? messageQueue.Receive() : messageQueue.Peek();
                return (IEnumerable<Product>)message.Body;
            }
        }
    }
}
