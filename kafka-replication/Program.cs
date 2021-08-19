using System;
using System.Threading;
using CommandLine;
using Confluent.Kafka;

namespace kafka_replication
{
    class Program
    {
        static void Main(string[] args)
        {
            Options parsedOptions = null;
            var parserResult = Parser.Default
                  .ParseArguments<Options>(args)
                  .WithParsed(parsedOpt => parsedOptions = parsedOpt);

            var config = new ConsumerConfig
            {
                BootstrapServers = parsedOptions.SourceServer,
                GroupId = "replicator-test",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = parsedOptions.TargetServer
            };
            var cancellationToken = new CancellationToken();

            using (var producer = new ProducerBuilder<string, byte[]>(config).Build())
            { 
                using (var consumer = new ConsumerBuilder<string, byte[]>(config).Build())
                {
                    consumer.Subscribe(new[] { parsedOptions.Topic });

                    while (true)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        Console.WriteLine(consumeResult.Headers.Count);

                        producer.Produce(consumeResult.Topic, consumeResult.Message);
                        // process message here.
                    }
                }
            }
        }
    }
}
