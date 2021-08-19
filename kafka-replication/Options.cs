using CommandLine;

namespace kafka_replication
{

    public class Options
    {
        [Value(0, Required = true, HelpText = "Source Bootstraper Server")]
        public string SourceServer { get; set; }

        [Value(1, Required = true, HelpText = "Target Bootstraper Server.")]
        public string TargetServer { get; set; }

        [Value(2, Required = true, HelpText = "TopicName.")]
        public string Topic { get; set; }
    }
}

