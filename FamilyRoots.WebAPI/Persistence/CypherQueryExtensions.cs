using System.Text;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using Neo4j.Driver;
using Newtonsoft.Json;

namespace FamilyRoots.WebAPI.Persistence
{
    public static class CypherQueryExtensions
    {
        public static string ToCypherCreateQuery(this UpsertPersonRequest person)
        {
            var builder = new StringBuilder();
            builder.Append("MERGE (p:Person {id: apoc.create.uuid()}) ");
            builder.Append($"SET p.surname = '{person.Surname}', ");
            builder.Append($"p.names = ['{string.Join("','", person.Names)}'], ");
            builder.Append($"p.sex = '{person.Sex.ToString().ToLowerInvariant()}' ");
            builder.Append("RETURN p");
            return builder.ToString();
        }
        
        public static string ToCypherUpdateQuery(this UpsertPersonRequest person)
        {
            var builder = new StringBuilder();
            builder.Append($"MATCH (p:Person {{id: '{person.Id}'}}) ");
            builder.Append($"SET p.surname = '{person.Surname}', ");
            builder.Append($"p.names = ['{string.Join("','", person.Names)}'] ");
            builder.Append("RETURN p");
            return builder.ToString();
        }

        public static Person ToPerson(this INode node)
        {
            var nodeProperties = JsonConvert.SerializeObject(node.Properties);
            return JsonConvert.DeserializeObject<Person>(nodeProperties);
        }
    }
}