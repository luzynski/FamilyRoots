using FamilyRoots.Data.Validation;
using NUnit.Framework;

namespace FamilyRoots.Data.Tests.Validation
{
    [TestFixture]
    public class BlankNotAllowedAttributeTests
    {
        private BlankNotAllowedAttribute _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new BlankNotAllowedAttribute();
        }

        [Test]
        public void NullValueShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(null));
        }

        [Test]
        public void EmptyStringShouldBeInvalid()
        {
            Assert.IsFalse(_uut.IsValid(""));
        }

        [Test]
        public void WhiteSpacesStringShouldBeInvalid()
        {
            Assert.IsFalse(_uut.IsValid(@"    "));
        }

        [Test]
        public void NonStringObjectShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(new object()));
        }

        [Test]
        public void EmptyStringArrayShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(new string[0]));
        }

        [Test]
        public void StringArrayWithValidStringsShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(new [] { "foo", "bar" }));
        }

        [Test]
        public void StringArrayWithInvalidStringsShouldBeInvalid()
        {
            Assert.IsFalse(_uut.IsValid(new [] { "foo", "bar", "    " }));
        }

        [Test]
        public void StringArrayWithNullValueShouldBeInvalid()
        {
            Assert.IsFalse(_uut.IsValid(new [] { "foo", "bar", null }));
        }

        [Test]
        public void NonStringArrayShouldBeValid()
        {
            Assert.IsTrue(_uut.IsValid(new [] { new object(), new object() }));
        }
    }
}