using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Collections.Generic;
using RabbitMQ.Client.Exceptions;

namespace Headers_Exchange_Consumer_Microservice
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

                        // Declare the headers exchange
                        channel.ExchangeDeclare(exchange: "headers_logs", type: ExchangeType.Headers);

                        // Declare the queue dynamically
                        var queueName = channel.QueueDeclare().QueueName;

                        // Bind the queue to the headers exchange with specific headers
                        var headers = new Dictionary<string, object>
                        {
                            { "format", "pdf" }
                            // Optionally add { "x-match", "all" } or { "x-match", "any" }
                        };
                        channel.QueueBind(queue: queueName, exchange: "headers_logs", routingKey: "", arguments: headers);

                        // Set up the consumer to consume messages from the queue
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("[x] Received '{0}'", message);
                        };
                        channel.BasicConsume(queue: queueName,
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
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

//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System.Text;
//using System.Collections.Generic;
//using RabbitMQ.Client.Exceptions;
//namespace Headers_Exchange_Consumer_Microservice
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            bool connected = false;
//            int retries = 5;
//            while (!connected && retries > 0)
//            {
//                try
//                {
//                    var factory = new ConnectionFactory() { HostName = "rabbitmq" };
//                    using (var connection = factory.CreateConnection())
//                    using (var channel = connection.CreateModel())
//                    {
//                        connected = true;
//                        channel.ExchangeDeclare(exchange: "headers_logs", type: ExchangeType.Headers);
//                        var queueName = channel.QueueDeclare().QueueName;

//                        var headers = new Dictionary<string, object> { { "format", "pdf" } };
//                        channel.QueueBind(queue: queueName, exchange: "headers_logs", routingKey: "", arguments: headers);

//                        var consumer = new EventingBasicConsumer(channel);
//                        consumer.Received += (model, ea) =>
//                        {
//                            var body = ea.Body.ToArray();
//                            var message = Encoding.UTF8.GetString(body);
//                            Console.WriteLine("[x] Received '{0}'", message);
//                        };
//                        channel.BasicConsume(queue: queueName,
//                                             autoAck: true,
//                                             consumer: consumer);

//                        Console.WriteLine(" Press [enter] to exit.");
//                        Console.ReadLine();
//                    }
//                }
//                catch (BrokerUnreachableException)
//                {
//                    retries--;
//                    Console.WriteLine("RabbitMQ ikke tilgængelig. Forsøger igen...");
//                    System.Threading.Thread.Sleep(5000); // Vent 5 sekunder før næste forsøg
//                }
//            }


//        }
//    }
//}
