using Infrastructure.DataAccess.ElasticSearch.ElasticSearchModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using SharedModel.Models;

namespace Infrastructure.DataAccess.ElasticSearch
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:baseUrl"];
            var index = configuration["ElasticSettings:defaultIndex"];
            var settings = new ConnectionSettings(new Uri(baseUrl ?? "")).PrettyJson().CertificateFingerprint("255ee456dfdb56cd3851515fd47c41b1b88b2e16f4e29e6c1343dfcd55abb390").BasicAuthentication("elastic", "HxI2WNW_a3=umYTezObA").DefaultIndex(index);
            settings.EnableApiVersioningHeader();
            AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
        }
        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<ElasticAdPreviewModel>(m => m.IndexName("adpreview"));
        }
        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map<ElasticAdPreviewModel>(x => x.AutoMap()));
        }
    }
}
