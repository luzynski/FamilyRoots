using System;
using FamilyRoots.Data.Requests;
using FamilyRoots.Data.Validation;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Validation
{
    [TestFixture]
    public class ImmutableSexAttributeTests
    {
        private ImmutableSexAttribute _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new ImmutableSexAttribute();
        }

        [Test]
        public void NullValueShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(null));
        }

        [Test]
        public void CreateRequestWithoutSexShouldBeInvalid()
        {
            var request = new UpsertPersonRequest
            {
                Id = null,
                Sex = null
            };

            Assert.IsFalse(_uut.IsValid(request));
        }

        [Test]
        public void CreateRequestWithSexShouldBeValid()
        {
            var request = new UpsertPersonRequest
            {
                Id = null,
                Sex = Sex.Female
            };

            Assert.IsTrue(_uut.IsValid(request));
        }

        [Test]
        public void UpdateRequestWithSexShouldBeInvalid()
        {
            var request = new UpsertPersonRequest
            {
                Id = Guid.NewGuid(),
                Sex = Sex.Male
            };

            Assert.IsFalse(_uut.IsValid(request));
        }

        [Test]
        public void UpdateRequestWithoutSexShouldBeValid()
        {
            var request = new UpsertPersonRequest
            {
                Id = Guid.NewGuid(),
                Sex = null
            };

            Assert.IsTrue(_uut.IsValid(request));
        }

        [Test]
        public void NonRequestObjectShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(new object()));
        }
    }
}