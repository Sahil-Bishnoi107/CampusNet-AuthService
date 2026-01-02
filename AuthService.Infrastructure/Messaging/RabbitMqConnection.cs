using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;


namespace AuthService.Infrastructure.Messaging
{
    using RabbitMQ.Client;

    public class RabbitMqConnection
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;

        public RabbitMqConnection(IConfiguration config)
        {
            var host =
                Environment.GetEnvironmentVariable("RABBITMQ_HOST")
                ?? config["RabbitMq:Host"]
                ?? "localhost";

            var user =
                Environment.GetEnvironmentVariable("RABBITMQ_USER")
                ?? config["RabbitMq:Username"]
                ?? "guest";

            var pass =
                Environment.GetEnvironmentVariable("RABBITMQ_PASS")
                ?? config["RabbitMq:Password"]
                ?? "guest";
            _factory = new ConnectionFactory
            {
                HostName = host,
                UserName = user,
                Password = pass
            };
        }

        private async Task<IConnection> GetConnectionAsync()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await _factory.CreateConnectionAsync();
            }
            return _connection;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            var connection = await GetConnectionAsync();
            return await connection.CreateChannelAsync();
        }
    }

}
