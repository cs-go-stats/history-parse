using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.DataAccess.Contexts.EF;
using Microsoft.Extensions.DependencyInjection;

namespace CSGOStats.Services.HistoryParse.Data
{
    public static class DataExtensions
    {
        public static Task EnsureDatabaseCreated(this IServiceProvider serviceProvider) =>
            serviceProvider.GetService<BaseDataContext>().Database.EnsureCreatedAsync();
    }
}