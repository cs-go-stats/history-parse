using CSGOStats.Infrastructure.Core.Initialization.RT.Actions;

namespace CSGOStats.Services.HistoryParse.Runtime
{
    internal class HistoryParseRuntimeAction : ActionsAggregator
    {
        public HistoryParseRuntimeAction()
            : base(new CreateRelationalDatabaseAction(),
                   new ExecuteJobsAction(),
                   new RegisterMessageHandlerForTypesAction(typeof(Objects.HistoricalMatchFound)),
                   new WaitForExternalInterruptionAction())
        {
        }
    }
}