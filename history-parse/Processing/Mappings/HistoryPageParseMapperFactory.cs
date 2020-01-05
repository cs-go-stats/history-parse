using System;
using CSGOStats.Infrastructure.PageParse.Mapping;
using CSGOStats.Services.HistoryParse.Processing.Mappings.Attributes;
using CSGOStats.Services.HistoryParse.Processing.Mappings.ValueMappers;

namespace CSGOStats.Services.HistoryParse.Processing.Mappings
{
    internal class HistoryPageParseMapperFactory : IValueMapperFactory
    {
        public IValueMapper Create(string mapperCode)
        {
            switch (mapperCode)
            {
                case GroupDateValueAttribute.MapCode:
                    return new GroupDateMapper();
                default:
                    throw new ArgumentOutOfRangeException(nameof(mapperCode), "Unknown mapper");
            }
        }
    }
}