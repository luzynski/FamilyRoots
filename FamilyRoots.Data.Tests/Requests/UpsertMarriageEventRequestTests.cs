using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FamilyRoots.Data.Requests;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Requests
{
    [TestFixture]
    public class UpsertMarriageEventRequestTests
    {
        private const string ValidId = "123e4567-e89b-12d3-a456-426614174000";
        private const string EmptyId = "00000000-0000-0000-0000-000000000000";

        [TestCase(null, ValidId, ValidId, null, null, null)] //ValidCreateRequest
        [TestCase(null, EmptyId, ValidId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_FirstSpouseIdIsEmptyGuid
        [TestCase(null, ValidId, EmptyId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidCreateRequest_SecondSpouseIdIsEmptyGuid
        [TestCase(ValidId, ValidId, ValidId, null, null, null)] //ValidUpdateRequest
        [TestCase(EmptyId, ValidId, ValidId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_IdIsEmptyGuid
        [TestCase(ValidId, EmptyId, ValidId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_FirstSpouseIdIsEmptyGuid
        [TestCase(ValidId, ValidId, EmptyId, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_SecondSpouseIdIsEmptyGuid
        public void TestUpsertMarriageEventRequest(string id, string firstSpouseId, string secondSpouseId, string marriageDate, string divorceDate, string[] expectedErrorMessages)
        {
            var uut = new UpsertMarriageEventRequest
            {
                Id = id == null ? (Guid?) null : Guid.Parse(id),
                FirstSpouseId = Guid.Parse(firstSpouseId),
                SecondSpouseId = Guid.Parse(secondSpouseId),
                MarriageDate = marriageDate == null ? (DateTime?) null : DateTime.Parse(marriageDate),
                DivorceDate = divorceDate == null ? (DateTime?) null : DateTime.Parse(divorceDate)
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