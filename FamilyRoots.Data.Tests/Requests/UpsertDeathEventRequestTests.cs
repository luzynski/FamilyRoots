using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FamilyRoots.Data.Requests;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Requests
{
    [TestFixture]
    public class UpsertDeathEventRequestTests
    {
        private const string ValidId = "123e4567-e89b-12d3-a456-426614174000";
        private const string EmptyId = "00000000-0000-0000-0000-000000000000";

        [TestCase(null, ValidId, null, null, null)] //ValidCreateRequest
        [TestCase(null, EmptyId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_DecedentIdIsEmptyGuid
        [TestCase(null, ValidId, null, "   ", new [] { "Value cannot be null, empty or white spaces only." })] //InvalidCreateRequest_BurialPlaceBlank
        [TestCase(ValidId, ValidId, null, null, null)] //ValidUpdateRequest
        [TestCase(EmptyId, ValidId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_IdIsEmptyGuid
        [TestCase(ValidId, EmptyId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_DecedentIdIsEmptyGuid
        [TestCase(ValidId, ValidId, null, "   ", new [] { "Value cannot be null, empty or white spaces only." })] //InvalidCreateRequest_BurialPlaceBlank
        public void TestUpsertDeathEventRequest(string id, string decedentId, string deathDate, string burialPlace, string[] expectedErrorMessages)
        {
            var uut = new UpsertDeathEventRequest
            {
                Id = id == null ? (Guid?) null : Guid.Parse(id),
                DecedentId = Guid.Parse(decedentId),
                DeathDate = deathDate == null ? (DateTime?) null : DateTime.Parse(deathDate),
                BurialPlace = burialPlace
            };
            
            var context = new ValidationContext(uut);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(uut, context, results, true);
            
            Assert.AreEqual(!expectedErrorMessages?.Any() ?? true, isValid);
            if (expectedErrorMessages?.Any() ?? false)
            {
                Assert.AreEqual(expectedErrorMessages.Length, results.Count);
                CollectionAssert.AreEquivalent(expectedErrorMessages, results.Select(x => x.ErrorMessage));
            }
        }
    }
}