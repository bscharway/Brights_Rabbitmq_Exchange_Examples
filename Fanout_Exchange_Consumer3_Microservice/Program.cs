namespace Fanout_Exchange_Consumer3_Microservice
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;
    using System.Text;

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

                        // Declare the fanout exchange
                        channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                        // Declare the queue for this consumer
                        var queueName = "queue_for_consumer_3";
                        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                        // Bind the queue to the fanout exchange
                        channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                        // Set up the consumer to consume messages from the queue
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("[x] Consumer 3 Received {0}", message);
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

//namespace Fanout_Exchange_Consumer3_Microservice
//{
//    using RabbitMQ.Client;
//    using RabbitMQ.Client.Events;
//    using RabbitMQ.Client.Exceptions;
//    using System.Text;
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
//                        channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

//                        var queueName = "queue_for_consumer_3";
//                        channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

//                        var consumer = new EventingBasicConsumer(channel);
//                        consumer.Received += (model, ea) =>
//                        {
//                            var body = ea.Body.ToArray();
//                            var message = Encoding.UTF8.GetString(body);
//                            Console.WriteLine("[x] Consumer 3 Received {0}", message);
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
