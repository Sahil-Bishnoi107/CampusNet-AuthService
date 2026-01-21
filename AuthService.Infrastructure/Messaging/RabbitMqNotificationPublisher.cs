using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Interfaces;
using RabbitMQ.Client;
using AuthService.Domain.Dtos;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Messaging
{
    public class RabbitMqNotificationPublisher : INotificationPublisher
    { 
        private readonly RabbitMqConnection _connection;
        private readonly ILogger<RabbitMqNotificationPublisher> _logger;
        public RabbitMqNotificationPublisher( RabbitMqConnection connection,ILogger<RabbitMqNotificationPublisher> logger)
        {
            _connection = connection;
            _logger = logger;
        }
        public async Task SendAuthOtp(NotificationMessage message)
        {
            _logger.LogInformation("Publishing auth OTP notification to RabbitMQ for {Email}", message.To);
            await using var channel = await _connection.CreateChannelAsync();
            // Here we are declaring the exchange
            await channel.ExchangeDeclareAsync(
                exchange: "notification.auth",
                type: ExchangeType.Fanout,
                durable: true
            );
            //Here we are declaring the queue
            await channel.QueueDeclareAsync(
            queue: "notification.auth.queue",
               durable: true,
            exclusive: false,
            autoDelete: false
           );
            // binding the queue to the exchange
            await channel.QueueBindAsync(
                  queue: "notification.auth.queue",
                  exchange: "notification.auth",
                  routingKey: ""
               );
            var payload = System.Text.Json.JsonSerializer.Serialize(message );

            _logger.LogInformation("Payload for RabbitMQ: {Payload}", payload);
            await channel.BasicPublishAsync(
                exchange: "notification.auth",
                routingKey: "",
                body: Encoding.UTF8.GetBytes(payload)
            );
            _logger.LogInformation("Published auth OTP notification to RabbitMQ for {Email}", message.To);
        }
    }
}
