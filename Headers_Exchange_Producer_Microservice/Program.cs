using RabbitMQ.Client;
using System.Text;

namespace Headers_Exchange_Producer_Microservice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "headers_logs", type: ExchangeType.Headers);

                var headers = new Dictionary<string, object> { { "format", "pdf" }, { "type", "report" } };
                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;

                for (int i = 1; i < 11; i++)
                {

                    string message = $"Dette er en rapport nr {i} i PDF format som viser hvor mange gang de vilde kaniner med gnags er blevet spillet i de sidste 10 år.";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "headers_logs",
                                         routingKey: "",
                                         basicProperties: properties,
                                         body: body);
                    Console.WriteLine("[x] Sent message with headers");
                }
            }

        }
    }
}
