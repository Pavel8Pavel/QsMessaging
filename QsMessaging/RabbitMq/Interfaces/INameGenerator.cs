﻿namespace QsMessaging.RabbitMq.Interface
{
    internal interface INameGenerator
    {
        string GetExchangeNameFromType<TModel>();

        string GetExchangeNameFromType(Type TModel);

        string GetQueueNameFromType(Type TModel, QueueType queueType);
    }
}
