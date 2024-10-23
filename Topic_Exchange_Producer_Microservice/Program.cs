namespace Topic_Exchange_Producer_Microservice
{
    using RabbitMQ.Client;
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

                        // Define the routing key and message to be sent
                        string routingKey = "log.error";
                        string message = "Dette er en fejl-log.";
                        var body = Encoding.UTF8.GetBytes(message);

                        // Publish the message to the topic exchange with a routing key
                        channel.BasicPublish(exchange: "topic_logs",
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine("[x] Sent '{0}':'{1}'", routingKey, message);
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

//namespace Topic_Exchange_Producer_Microservice
//{
//    using RabbitMQ.Client;
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

//                        channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

//                        string routingKey = "log.error";
//                        string message = "Dette er en fejl-log.";
//                        var body = Encoding.UTF8.GetBytes(message);

//                        channel.BasicPublish(exchange: "topic_logs",
//                                             routingKey: routingKey,
//                                             basicProperties: null,
//                                             body: body);
//                        Console.WriteLine("[x] Sent '{0}':'{1}'", routingKey, message);
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
