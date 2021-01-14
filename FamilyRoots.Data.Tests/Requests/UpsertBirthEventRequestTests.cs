using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FamilyRoots.Data.Requests;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Requests
{
    [TestFixture]
    public class UpsertBirthEventRequestTests
    {
        private const string ValidId = "123e4567-e89b-12d3-a456-426614174000";
        private const string EmptyId = "00000000-0000-0000-0000-000000000000";

        [TestCase(null, ValidId, null, null, null, null, null)] //ValidCreateRequest
        [TestCase(null, EmptyId, null, null, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_ChildIdIsEmptyGuid
        [TestCase(null, ValidId, EmptyId, null, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_FatherIdIsEmptyGuid
        [TestCase(null, ValidId, null, EmptyId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_MotherIdIsEmptyGuid
        [TestCase(ValidId, ValidId, null, null, null, null, null)] //ValidUpdateRequest
        [TestCase(EmptyId, ValidId, null, null, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_IdIsEmptyGuid
        [TestCase(ValidId, EmptyId, null, null, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_ChildIdIsEmptyGuid
        [TestCase(ValidId, ValidId, EmptyId, null, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_FatherIdIsEmptyGuid
        [TestCase(ValidId, ValidId, null, EmptyId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_MotherIdIsEmptyGuid
        public void TestUpsertBirthEventRequest(string id, string childId, string fatherId, string motherId, string birthDate, string birthPlace, string[] expectedErrorMessages)
        {
            var uut = new UpsertBirthEventRequest
            {
                Id = id == null ? (Guid?) null : Guid.Parse(id),
                ChildId = Guid.Parse(childId),
                FatherId = fatherId == null ? (Guid?) null : Guid.Parse(fatherId),
                MotherId = motherId == null ? (Guid?) null : Guid.Parse(motherId),
                BirthDate = birthDate == null ? (DateTime?) null : DateTime.Parse(birthDate),
                BirthPlace = birthPlace
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