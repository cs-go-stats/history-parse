using System;

namespace CSGOStats.Services.HistoryParse.Aggregate.Entities
{
    // todo: move to core package
    public abstract class AggregateRoot
    {
        public Guid Id { get; private set; }

        public long Version { get; private set; }

        protected AggregateRoot() { }

        protected AggregateRoot(Guid id, long version)
        {
            Id = id;
            Version = version;
        }

        protected void Update() => Version++;
    }
}