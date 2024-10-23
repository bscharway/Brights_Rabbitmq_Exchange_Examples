using RabbitMQ.Client;
using System.Text;
namespace Direct_Exchange_Producer_Microservice

{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                string severity = "error";
                string message = "Dette er en fejlmeddelelse.";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: severity,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("[x] Sent '{0}':'{1}'", severity, message);
            }
        }
    }
}
