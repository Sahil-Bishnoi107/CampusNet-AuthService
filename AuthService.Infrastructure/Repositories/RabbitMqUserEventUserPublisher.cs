using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Messaging;
using RabbitMQ.Client;

namespace AuthService.Infrastructure.Repositories
{
    public class RabbitMqUserEventPublisher : IUserEventPublisher
    {
        private readonly RabbitMqConnection _connection;

        public RabbitMqUserEventPublisher(RabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishUserCreatedEventAsync(string userId, string email, string name,string phoneNo)
        {
            await using var channel = await _connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "user.events",
                type: ExchangeType.Fanout,
                durable: true
            );

            var payload = JsonSerializer.Serialize(new
            {
                UserId = userId,
                Email = email,
                Name = name,
                PhoneNo = phoneNo,
            });

            await channel.BasicPublishAsync(
                exchange: "user.events",
                routingKey: "",
                body: Encoding.UTF8.GetBytes(payload)
            );
        }
    }
}
