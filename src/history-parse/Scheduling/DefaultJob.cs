namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public class DefaultJob : BaseHistoryParseJob
    {
        protected override string Code => nameof(DefaultJob);

        protected override bool IsForcedRun => false;
    }
}