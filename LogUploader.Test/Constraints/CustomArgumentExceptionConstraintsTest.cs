using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace LogUploader.Test.Constraints
{
    class CustomArgumentExceptionConstraintsTest
    {
    }
    public abstract class CustomArgumentExceptionConstraintsTestBase<TException>
    {
        [Test]
        public void AssertValidArgumentExceptionTest()
        {
            var validArgEx = CreateException("Valid Arguement test message", "testParam");
            Assert.That(validArgEx, CreateConstraint());
        }

        [Test]
        public virtual void AssertInvalidMessageTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string invalidMessage)
        {
            var invalidArgEx = CreateException(invalidMessage, "testParam");
            Assert.That(invalidArgEx, Is.Not.Append(CreateConstraint()));
        }

        [Test]
        public void AssertInvalidParamNameTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidOneWordStrings))] string invalidParamName)
        {
            var invalidArgEx = CreateException("A valid exception message", invalidParamName);
            Assert.That(invalidArgEx, Is.Not.Append(CreateConstraint()));
        }

        [Test]
        public void AssertValidMessageTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidMulitWordStrings))] string validMessage)
        {
            if (validMessage.Split(null).Count() == 1) Assert.Pass();
            var invalidArgEx = CreateException(validMessage, "testParam");
            Assert.That(invalidArgEx, CreateConstraint());
        }

        [Test]
        public void AssertValidParamNameTest([ValueSource(typeof(TestHelper), nameof(TestHelper.ValidOneWordStrings))] string validParamName)
        {
            var invalidArgEx = CreateException("A valid exception message", validParamName);
            Assert.That(invalidArgEx, CreateConstraint());
        }

        [Test]
        public void AssertInvalidTypeTest()
        {
            Assert.That(new object(), Is.Not.Append(CreateConstraint()));
            Assert.That(null, Is.Not.Append(CreateConstraint()));
        }

        internal abstract NUnit.Framework.Constraints.Constraint CreateConstraint();

        internal abstract TException CreateException(string message, string paramName);
    }

    public class CustomArgumentExceptionConstraintTest : CustomArgumentExceptionConstraintsTestBase<ArgumentException>
    {
        internal override NUnit.Framework.Constraints.Constraint CreateConstraint()
        {
            return new CustomArgumentExceptionConstraint();
        }

        internal override ArgumentException CreateException(string message, string paramName)
        {
            return new ArgumentException(message, paramName);
        }
    }

    public class CustomArgumentNullExceptionConstraintTest : CustomArgumentExceptionConstraintsTestBase<ArgumentNullException>
    {
        [Test]
        public override void AssertInvalidMessageTest([ValueSource(typeof(TestHelper), nameof(TestHelper.InvalidMulitWordStrings))] string invalidMessage)
        {
            if (string.IsNullOrEmpty(invalidMessage)) Assert.Pass();
            base.AssertInvalidMessageTest(invalidMessage);
        }

        internal override NUnit.Framework.Constraints.Constraint CreateConstraint()
        {
            return new CustomArgumentNullExceptionConstraint();
        }

        internal override ArgumentNullException CreateException(string message, string paramName)
        {
            return new ArgumentNullException(paramName, message);
        }
    }

    public class CustomArgumentOutOfRangeExceptionConstraintsTest : CustomArgumentExceptionConstraintsTestBase<ArgumentOutOfRangeException>
    {
        internal override NUnit.Framework.Constraints.Constraint CreateConstraint()
        {
            return new CustomArgumentOutOfRangeExceptionConstraint();
        }

        internal override ArgumentOutOfRangeException CreateException(string message, string paramName)
            => CreateOutOfRangeException(message, paramName, new object());

        private ArgumentOutOfRangeException CreateOutOfRangeException(string message, string paramName, object actualValue)
        {
            return new ArgumentOutOfRangeException(paramName, actualValue, message);
        }

        [Test]
        public void AssertInvalidActualValueTest()
        {
            Assert.That(CreateOutOfRangeException("Valid Message", "testParam", null), Is.Not.Append(CreateConstraint()));
        }

        [Test]
        public void CreateCustomAssertArgumentOutOfRangeExceptionNoValueCompareTest()
        {
            var expected = new object();
            var obj = new CustomArgumentOutOfRangeExceptionConstraint();

            Assert.That(obj.ValidateActualValue, Is.False);
        }
    }

    public class CustomAssertArgumentOutOfRangeExceptionValueAssertTests : CustomArgumentExceptionConstraintsTestBase<ArgumentOutOfRangeException>
    {
        private const int EXPECTED_ACTUAL_VALUE = 41;

        internal override NUnit.Framework.Constraints.Constraint CreateConstraint()
        {
            return CreateValueCheckingConstraint(EXPECTED_ACTUAL_VALUE);
        }

        private static NUnit.Framework.Constraints.Constraint CreateValueCheckingConstraint(int expectedActualValue)
        {
            return new CustomArgumentOutOfRangeExceptionConstraint(expectedActualValue);
        }

        internal override ArgumentOutOfRangeException CreateException(string message, string paramName)
            => CreateOutOfRangeException(message, paramName, EXPECTED_ACTUAL_VALUE);

        private ArgumentOutOfRangeException CreateOutOfRangeException(string message, string paramName, object actualValue)
        {
            return new ArgumentOutOfRangeException(paramName, actualValue, message);
        }

        [Test]
        public void AssertInvalidActualValueTest()
        {
            Assert.That(CreateOutOfRangeException("Valid Message", "testParam", null), Is.Not.Append(CreateConstraint()));
            Assert.That(CreateOutOfRangeException("Valid Message", "testParam", EXPECTED_ACTUAL_VALUE + 1), Is.Not.Append(CreateConstraint()));
        }

        [Test]
        public void CreateCustomAssertArgumentOutOfRangeExceptionValueCompareTest()
        {
            var expected = new object();
            var obj = new CustomArgumentOutOfRangeExceptionConstraint(expected);

            Assert.That(obj.ValidateActualValue);
            Assert.That(obj.ExpectedActualValue, Is.EqualTo(expected));
        }
    }
}
