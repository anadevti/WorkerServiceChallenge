using RabbitMQ.Client;

namespace WorkerService.infra.RabbitMQ.Send;

public class Send
{
    public async Task<string> SendMessage()
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
        }
        return "Message sent successfully";
    }
}