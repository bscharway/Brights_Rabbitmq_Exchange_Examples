using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace Headers_Exchange_Producer_Microservice
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

                        // Create headers for the message
                        var headers = new Dictionary<string, object> { { "format", "pdf" }, { "type", "report" } };
                        var properties = channel.CreateBasicProperties();
                        properties.Headers = headers;

                        // Publish 10 messages to the headers exchange
                        for (int i = 1; i < 11; i++)
                        {
                            string message = $"Dette er en rapport nr {i} i PDF format som viser hvor mange gang de vilde kaniner med gnags er blevet spillet i de sidste 10 år.";
                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: "headers_logs",
                                                 routingKey: "", // Routing key is ignored for headers exchange
                                                 basicProperties: properties,
                                                 body: body);
                            Console.WriteLine("[x] Sent message with headers: format=pdf, type=report");
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

//using RabbitMQ.Client;
//using RabbitMQ.Client.Exceptions;
//using System.Text;

//namespace Headers_Exchange_Producer_Microservice
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

//                        var headers = new Dictionary<string, object> { { "format", "pdf" }, { "type", "report" } };
//                        var properties = channel.CreateBasicProperties();
//                        properties.Headers = headers;

//                        for (int i = 1; i < 11; i++)
//                        {

//                            string message = $"Dette er en rapport nr {i} i PDF format som viser hvor mange gang de vilde kaniner med gnags er blevet spillet i de sidste 10 år.";
//                            var body = Encoding.UTF8.GetBytes(message);

//                            channel.BasicPublish(exchange: "headers_logs",
//                                                 routingKey: "",
//                                                 basicProperties: properties,
//                                                 body: body);
//                            Console.WriteLine("[x] Sent message with headers");
//                        }
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
