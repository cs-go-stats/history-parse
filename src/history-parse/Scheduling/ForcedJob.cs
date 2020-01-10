namespace CSGOStats.Services.HistoryParse.Scheduling
{
    public class ForcedJob : BaseHistoryParseJob
    {
        protected override string Code => nameof(ForcedJob);

        protected override bool IsForcedRun => true;
    }
}