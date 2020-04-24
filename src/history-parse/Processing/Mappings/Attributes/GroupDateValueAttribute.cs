using System;
using CSGOStats.Infrastructure.Core.PageParse.Mapping;

namespace CSGOStats.Services.HistoryParse.Processing.Mappings.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class GroupDateValueAttribute : BaseMapValueAttribute
    {
        internal const string MapCode = "GroupDateValue";

        internal GroupDateValueAttribute()
            : base(MapCode)
        {
        }
    }
}