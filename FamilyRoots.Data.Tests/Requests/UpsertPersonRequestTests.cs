using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FamilyRoots.Data.Requests;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Requests
{
    [TestFixture]
    public class UpsertPersonRequestTests
    {
        private const string ValidId = "123e4567-e89b-12d3-a456-426614174000";
        private const string EmptyId = "00000000-0000-0000-0000-000000000000";

        [TestCase(null, "foo", "bar", Sex.Male, null, null, null)] //ValidCreateRequest
        [TestCase(null, "foo", "bar", null, null, null, new [] { "Person sex has to be set on creation." })] //InvalidCreateRequest_PersonSexMissing
        [TestCase(null, null, "bar", Sex.Male, null, null, new [] { "The FirstName field is required." })] //InvalidCreateRequest_FirstNameMissing
        [TestCase(null, "foo", null, Sex.Male, null, null, new [] { "The FamilyName field is required." })] //InvalidCreateRequest_FamilyNameMissing
        [TestCase(null, "foo", "bar", Sex.Male, null, "   ", new [] { "Value cannot be null, empty or white spaces only." })] //InvalidCreateRequest_MaidenNameBlank
        [TestCase(null, "foo", "bar", Sex.Male, new [] { "baz", "   " }, null, new [] { "Array cannot have values that are null, empty or white spaces only." })] //InvalidCreateRequest_OneOfMiddleNamesBlank
        [TestCase(ValidId, "foo", "bar", null, null, null, null)] //ValidUpdateRequest
        [TestCase(ValidId, "foo", "bar", Sex.Male, null, null, new [] { "Cannot change person sex." })] //InvalidUpdateRequest_PersonSexIsPresent
        [TestCase(ValidId, null, "bar", null, null, null, new [] { "The FirstName field is required." })] //InvalidUpdateRequest_FirstNameMissing
        [TestCase(ValidId, "foo", null, null, null, null, new [] { "The FamilyName field is required." })] //InvalidUpdateRequest_FamilyNameMissing
        [TestCase(ValidId, "foo", "bar", null, null, "   ", new [] { "Value cannot be null, empty or white spaces only." })] //InvalidUpdateRequest_MaidenNameBlank
        [TestCase(ValidId, "foo", "bar", null, new [] { "baz", "   " }, null, new [] { "Array cannot have values that are null, empty or white spaces only." })] //InvalidCreateRequest_OneOfMiddleNamesBlank
        [TestCase(EmptyId, "foo", "bar", null, null, null, new [] { "Id cannot be empty uuid." })] //InvalidUpdateRequest_IdIsEmptyGuid
        public void TestUpsertPersonRequest(string id, string firstName, string familyName, Sex? sex, string[] middleNames, string maidenName, string[] expectedErrorMessages)
        {
            var uut = new UpsertPersonRequest
            {
                Id = id == null ? (Guid?) null : Guid.Parse(id),
                FirstName = firstName,
                FamilyName = familyName,
                Sex = sex,
                MiddleNames = middleNames,
                MaidenName = maidenName
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