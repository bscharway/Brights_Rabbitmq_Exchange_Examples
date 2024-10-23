namespace Topic_Exchange_Consumer_Microservice
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

                        // Declare the topic exchange
                        channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

                        // Declare a unique queue for this consumer
                        var queueName = channel.QueueDeclare().QueueName;

                        // Bind the queue to the exchange using a routing key pattern
                        string bindingKey = "log.*";
                        channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: bindingKey);

                        // Set up the consumer to consume messages from the queue
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var routingKey = ea.RoutingKey;
                            Console.WriteLine("[x] Received '{0}':'{1}'", routingKey, message);
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

//using RabbitMQ.Client.Events;
//using RabbitMQ.Client;
//using System.Text;
//using RabbitMQ.Client.Exceptions;

//namespace Topic_Exchange_Consumer_Microservice
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
//                        channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
//                        var queueName = channel.QueueDeclare().QueueName;

//                        string bindingKey = "log.*";
//                        channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: bindingKey);

//                        var consumer = new EventingBasicConsumer(channel);
//                        consumer.Received += (model, ea) =>
//                        {
//                            var body = ea.Body.ToArray();
//                            var message = Encoding.UTF8.GetString(body);
//                            var routingKey = ea.RoutingKey;
//                            Console.WriteLine("[x] Received '{0}':'{1}'", routingKey, message);
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
