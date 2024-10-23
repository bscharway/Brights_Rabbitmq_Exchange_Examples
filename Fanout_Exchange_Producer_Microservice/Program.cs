using RabbitMQ.Client;
using System.Text;

namespace Fanout_Exchange_Producer_Microservice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string queue1 = "queue_for_consumer_1";
                string queue2 = "queue_for_consumer_2";
                string queue3 = "queue_for_consumer_3";

                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
                channel.QueueDeclare(queue1, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue2, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue3, durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueBind(queue1, exchange: "logs", routingKey: "");
                channel.QueueBind(queue2, exchange: "logs", routingKey: "");
                channel.QueueBind(queue3, exchange: "logs", routingKey: "");

                string message = "Broadcast besked";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("[x] Sent {0}", message);
            }
        }
    }
}
