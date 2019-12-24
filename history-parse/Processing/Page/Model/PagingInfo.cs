using System.Diagnostics;
using CSGOStats.Infrastructure.Validation;

namespace CSGOStats.Services.HistoryParse.Processing.Page.Model
{
    [DebuggerDisplay("Start: {BatchStart}; End: {BatchEnd}; Total: {TotalRecords}.")]
    public class PagingInfo
    {
        public int BatchStart { get; }

        public int BatchEnd { get; }

        public int TotalRecords { get; }

        public PagingInfo(int batchStart, int batchEnd, int totalRecords)
        {
            BatchStart = batchStart.Positive(nameof(batchStart));
            BatchEnd = batchEnd.Positive(nameof(batchEnd)).GreaterThanOrEqual(batchStart, nameof(batchEnd));
            TotalRecords = totalRecords.Positive(nameof(totalRecords));
        }
    }
}