using RabbitMQ.Client;
using System;
using System.Text;

namespace MQSender
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            //并发量大会报错
            ThreadStart obj = new ThreadStart(WriteContent);
            for (int i = 0; i < 10000; i++)
            {
                Thread t = new Thread(obj)
                {
                    Name = i.ToString(),
                    IsBackground = true
                };
                t.Start();
            }*/

            WriteContent();
        }
        /*
        static void WriteContent()
        {
            System.IO.File.AppendAllText("d:/testfile.txt", "this is a test file " + System.DateTime.Now.ToShortTimeString());
        }*/

        static void WriteContent()
        {
            var factory = new ConnectionFactory() { HostName = "127.0.0.1", UserName = "admin", Password = "123456" };
            using (var connection = factory.CreateConnection()) //建立连接
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "queue1", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind("queue1", "wsExchange", "writefile");

                for (int i = 0; i < 10000; i++)
                {
                    string message = "this is a test file " + i;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "wsExchange", routingKey: "writefile", basicProperties: null, body: body);
                    Console.WriteLine($" [x] Sent {message}");
                }
            }
        }

    }
}
