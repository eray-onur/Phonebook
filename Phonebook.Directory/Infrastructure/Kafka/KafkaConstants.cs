namespace Phonebook.Directory.Infrastructure.Kafka
{
    public class KafkaConstants
    {
        public const string DirectoryConsumerGroup = nameof(DirectoryConsumerGroup);
        public const string PersonListRequestUpdates = nameof(PersonListRequestUpdates);

        public class Topics
        {
            public const string PersonListGenerate = "person-list-generate-topic";
            public const string PersonListGenerated = "person-list-generated-topic";
        }
    }
}
