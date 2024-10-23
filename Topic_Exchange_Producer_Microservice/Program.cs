namespace Topic_Exchange_Producer_Microservice
{
    using RabbitMQ.Client;
    using System.Text;
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

                string routingKey = "log.error";
                string message = "Dette er en fejl-log.";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "topic_logs",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine("[x] Sent '{0}':'{1}'", routingKey, message);
            }
        }
    }
}
