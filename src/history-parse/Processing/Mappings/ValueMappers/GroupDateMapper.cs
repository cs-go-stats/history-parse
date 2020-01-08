using System;
using System.Globalization;
using System.Text.RegularExpressions;
using CSGOStats.Extensions.Extensions;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.PageParse.Mapping;
using HtmlAgilityPack;
using NodaTime;
using NodaTime.Calendars;

namespace CSGOStats.Services.HistoryParse.Processing.Mappings.ValueMappers
{
    public class GroupDateMapper : IValueMapper
    {
        public object Map(HtmlNode root)
        {
            var matchGroups = new Regex(@"Results\sfor\s(?<month>\w+)\s(?<day>\d{1,2})[stndrh]{2}\s(?<year>\d{4})").Match(root.InnerText).ForSucceeded().Groups;
            return new OffsetDate(
                new LocalDate(
                    era: Era.Common,
                    yearOfEra: matchGroups["year"].Value.Int(),
                    month: DateTime.ParseExact(matchGroups["month"].Value, "MMMM", CultureInfo.DefaultThreadCurrentCulture).Month,
                    day: matchGroups["day"].Value.Int()),
                Offset.Zero);
        }
    }
}
