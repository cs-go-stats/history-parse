using System;
using CSGOStats.Infrastructure.PageParse.Mapping;

namespace CSGOStats.Services.HistoryParse.Processing.Mappings.Attributes
{
    // todo: to shared lib; over abstraction like 'attribute value'
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageTitleValueAttribute : BaseMapValueAttribute
    {
        public ImageTitleValueAttribute()
            : base("ImageTitle")
        {
        }
    }
}