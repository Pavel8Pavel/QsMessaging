﻿using QsMessaging.Public;
using QsMessaging.RabbitMq.Interface;
using RabbitMQ.Client;

namespace QsMessaging.RabbitMq
{
    internal class RabbitMqConnectionStorage(QsMessagingConfiguration configuration) : IConnectionStorage
    {
        private IConnection? _connection;
        private IChannel? _channel;

        public async Task<(IConnection connection, IChannel chanel)> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            var attempt = 0;
            if (_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen)
            {
                return (_connection, _channel);
            }

            do
            {
                _connection = await CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                //TODO: Implement logariphmick waiting strategy
                await Task.Delay(attempt + 1000).WaitAsync(cancellationToken);
                attempt++;
            }
            while (!(_connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen));

            return (_connection, _channel);
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration.RabbitMQ.Host,
                UserName = configuration.RabbitMQ.UserName,
                Password = configuration.RabbitMQ.Password,
                Port = configuration.RabbitMQ.Port
            };

            return await factory.CreateConnectionAsync();
        }
    }
}
