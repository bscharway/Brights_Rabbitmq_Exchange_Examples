using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace Direct_Exchange_Producer_Microservice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool connected = false;
            int retries = 5;

            while (!connected && retries > 0)
            {
                try
                {
                    var factory = new ConnectionFactory() { HostName = "rabbitmq" };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        connected = true;

                        var queueName = "queue_for_direct_logs";
                        string severity = "error";

                        // Declare the direct exchange
                        channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                        // Declare the queue
                        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                        // Bind the queue to the exchange with a routing key
                        channel.QueueBind(queue: queueName, exchange: "direct_logs", routingKey: severity);

                        // Send 10 messages to the exchange
                        for (int i = 1; i < 11; i++)
                        {
                            string message = $"Dette er en fejlmeddelelse nr {i}!! Bright kan ikke finde ud af at kode.";
                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: "direct_logs",
                                                 routingKey: severity,
                                                 basicProperties: null,
                                                 body: body);
                            Console.WriteLine("[x] Sent '{0}':'{1}'", severity, message);
                        }
                    }
                }
                catch (BrokerUnreachableException)
                {
                    retries--;
                    Console.WriteLine("RabbitMQ ikke tilgængelig. Forsøger igen... (resterende forsøg: {0})", retries);
                    System.Threading.Thread.Sleep(5000); // Vent 5 sekunder før næste forsøg
                }
            }

            if (!connected)
            {
                Console.WriteLine("Kunne ikke oprette forbindelse til RabbitMQ efter flere forsøg.");
            }
        }
    }
}

