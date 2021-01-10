using System.Linq;
using System.Text;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using Neo4j.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FamilyRoots.WebAPI.Persistence
{
    public static class CypherQueryExtensions
    {
        private static readonly JsonSerializerSettings SnakeCaseSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        
        public static string ToCypherCreateQuery(this UpsertPersonRequest person)
        {
            var builder = new StringBuilder();
            builder.Append("MERGE (p:Person {id: apoc.create.uuid()}) ");
            builder.Append($"SET p.family_name = '{person.FamilyName}'");
            builder.Append($", p.first_name = '{person.FirstName}'");
            if (person.MiddleNames?.Any() ?? false)
            {
                builder.Append($", p.middle_names = ['{string.Join("','", person.MiddleNames)}']");
            }
            if (!string.IsNullOrWhiteSpace(person.MaidenName))
            {
                builder.Append($", p.maiden_name = '{person.MaidenName}'");
            }
            builder.Append($", p.sex = '{person.Sex.ToString().ToLowerInvariant()}'");
            builder.Append(" RETURN p");
            return builder.ToString();
        }
        
        public static string ToCypherUpdateQuery(this UpsertPersonRequest person)
        {
            var builder = new StringBuilder();
            builder.Append($"MATCH (p:Person {{id: '{person.Id}'}}) ");
            builder.Append($"SET p.family_name = '{person.FamilyName}'");
            builder.Append($", p.first_name = '{person.FirstName}'");
            if (person.MiddleNames?.Any() ?? false)
            {
                builder.Append($", p.middle_names = ['{string.Join("','", person.MiddleNames)}']");
            }
            if (!string.IsNullOrWhiteSpace(person.MaidenName))
            {
                builder.Append($", p.maiden_name = '{person.MaidenName}'");
            }
            builder.Append(" RETURN p");
            return builder.ToString();
        }

        public static Person ToPerson(this INode node)
        {
            var nodeProperties = JsonConvert.SerializeObject(node.Properties);
            return JsonConvert.DeserializeObject<Person>(nodeProperties, SnakeCaseSerializerSettings);
        }
    }
}