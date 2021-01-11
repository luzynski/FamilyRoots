using System;
using FamilyRoots.Data.Validation;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Validation
{
    [TestFixture]
    public class GuidNotEmptyAttributeTests
    {
        private GuidNotEmptyAttribute _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new GuidNotEmptyAttribute();
        }

        [Test]
        public void NullValueShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(null));
        }

        [Test]
        public void EmptyGuidShouldBeInvalid()
        {
            Assert.IsFalse(_uut.IsValid(Guid.Empty));
        }

        [Test]
        public void NonGuidObjectShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(new object()));
        }
    }
}