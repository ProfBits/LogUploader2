using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace LogUploader.Test.Constraints
{
    public class CustomConstraintExtensionesTest
    {
        [Test]
        public void ValidateArguemntExceptionTest()
        {
            Assert.That(Throws.ValidateArgumentException, Is.TypeOf(typeof(CustomArgumentExceptionConstraint)).And.Not.Null);
            Assert.That(Is.Not.ValidateArgumentException, Is.TypeOf(typeof(CustomArgumentExceptionConstraint)).And.Not.Null);
        }

        [Test]
        public void ValidateArguemntNullExceptionTest()
        {
            Assert.That(Throws.ValidateArgumentNullException, Is.TypeOf(typeof(CustomArgumentNullExceptionConstraint)).And.Not.Null);
            Assert.That(Is.Not.ValidateArgumentNullException, Is.TypeOf(typeof(CustomArgumentNullExceptionConstraint)).And.Not.Null);
        }

        [Test]
        public void ValidateArguemntOutOfRangeExceptionTest()
        {
            Assert.That(Throws.ValidateArgumentOutOfRangeException(), Is.TypeOf(typeof(CustomArgumentOutOfRangeExceptionConstraint)).And.Not.Null);
            Assert.That(Throws.ValidateArgumentOutOfRangeException().ValidateActualValue, Is.False);
            Assert.That(Is.Not.ValidateArgumentOutOfRangeException(), Is.TypeOf(typeof(CustomArgumentOutOfRangeExceptionConstraint)).And.Not.Null);
            Assert.That(Is.Not.ValidateArgumentOutOfRangeException().ValidateActualValue, Is.False);
        }

        [Test]
        public void ValidateArguemntOutOfRangeExceptionValueTest()
        {
            Assert.That(Throws.ValidateArgumentOutOfRangeException(404), Is.TypeOf(typeof(CustomArgumentOutOfRangeExceptionConstraint)).And.Not.Null);
            Assert.That(Throws.ValidateArgumentOutOfRangeException(404).ValidateActualValue);
            Assert.That(Throws.ValidateArgumentOutOfRangeException(404).ExpectedActualValue, Is.EqualTo(404));
            Assert.That(Is.Not.ValidateArgumentOutOfRangeException(13), Is.TypeOf(typeof(CustomArgumentOutOfRangeExceptionConstraint)).And.Not.Null);
            Assert.That(Throws.ValidateArgumentOutOfRangeException(13).ValidateActualValue);
            Assert.That(Throws.ValidateArgumentOutOfRangeException(13).ExpectedActualValue, Is.EqualTo(13));
        }
    }
}
