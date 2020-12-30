using System.Text;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using Neo4j.Driver;
using Newtonsoft.Json;

namespace FamilyRoots.WebAPI.Persistence
{
    public static class CypherQueryExtensions
    {
        public static string ToCypherCreateQuery(this CreatePersonRequest person)
        {
            var builder = new StringBuilder();
            builder.Append("MERGE (p:Person {id: apoc.create.uuid()}) ");
            builder.Append($"SET p.surname = '{person.Surname}', ");
            builder.Append($"p.names = ['{string.Join("','", person.Names)}'], ");
            builder.Append($"p.sex = '{person.Sex.ToString().ToLowerInvariant()}', ");
            builder.Append("p.birthdate = ");
            builder.Append(person.BirthDate.HasValue
                ? $"date('{person.BirthDate.Value:yyyy-MM-dd}'), "
                : "null, ");
            builder.Append("p.birthplace = ");
            builder.Append(person.BirthPlace == null
                ? "null, "
                : $"'{person.BirthPlace}', ");
            builder.Append("p.deathdate = ");
            builder.Append(person.DeathDate.HasValue
                ? $"date('{person.DeathDate.Value:yyyy-MM-dd}'), "
                : "null, ");
            builder.Append("p.burialplace = ");
            builder.Append(person.BurialPlace == null
                ? "null "
                : $"'{person.BurialPlace}' ");
            builder.Append("RETURN p");
            return builder.ToString();
        }
        
        public static string ToCypherUpdateQuery(this UpdatePersonRequest person)
        {
            var builder = new StringBuilder();
            builder.Append($"MATCH (p:Person {{id: '{person.Id}'}}) ");
            builder.Append($"SET p.surname = '{person.Surname}', ");
            builder.Append($"p.names = ['{string.Join("','", person.Names)}'], ");
            builder.Append($"p.sex = '{person.Sex.ToString().ToLowerInvariant()}', ");
            builder.Append("p.birthdate = ");
            builder.Append(person.BirthDate.HasValue
                ? $"date('{person.BirthDate.Value:yyyy-MM-dd}'), "
                : "null, ");
            builder.Append("p.birthplace = ");
            builder.Append(person.BirthPlace == null
                ? "null, "
                : $"'{person.BirthPlace}', ");
            builder.Append("p.deathdate = ");
            builder.Append(person.DeathDate.HasValue
                ? $"date('{person.DeathDate.Value:yyyy-MM-dd}'), "
                : "null, ");
            builder.Append("p.burialplace = ");
            builder.Append(person.BurialPlace == null
                ? "null "
                : $"'{person.BurialPlace}' ");
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