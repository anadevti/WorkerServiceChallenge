using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WorkerService.infra.RabbitMQ.Receive;

public class Receive
{
    public async Task<string> ReceiveMessage()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();
        await using (connection)
        {
            var channel = await connection.CreateChannelAsync();
            await using (channel)
            {
                await channel.QueueDeclareAsync(queue: "Teste", 
                    durable: false, 
                    exclusive: false, 
                    autoDelete: false,
                    arguments: null);
            }
            
            Console.WriteLine(" [*] Waiting for messages.");
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("hello", autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            
        }
        return "Message Received successfully";
    }
}