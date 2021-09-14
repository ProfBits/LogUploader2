using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework.Constraints;

namespace LogUploader.Test.Constraints
{
    internal abstract class CustomArgumentExceptionConstraintBase<ArgExType> : Constraint where ArgExType : ArgumentException
    {
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if (actual is ArgExType ex)
            {
                return ValidateArgumentException(ex);
            }
            Description = $"The type of given exeption does not match the expected {typeof(ArgExType).FullName}";
            return new ConstraintResult(this, typeof(TActual).FullName, false);
        }

        protected virtual ConstraintResult ValidateArgumentException(ArgExType argEx)
        {
            var paramResult = ValidateParamName(argEx);
            if (paramResult.Status != ConstraintStatus.Success) return paramResult;

            var messageResult = ValidateMessage(argEx);
            if (messageResult.Status != ConstraintStatus.Success) return messageResult;

            return new ConstraintResult(this, argEx, true);
        }

        protected virtual ConstraintResult ValidateParamName(ArgExType argEx)
        {
            var paramName = argEx.ParamName;
            if (string.IsNullOrWhiteSpace(argEx.ParamName))
            {
                Description = $"The {nameof(argEx.ParamName)} should not be null or whiespace";
                return new ConstraintResult(this, argEx.ParamName, false);
            }
            if (paramName.Trim() != paramName)
            {
                Description = $"The {nameof(argEx.ParamName)} should not beginn or end with whiespace";
                return new ConstraintResult(this, argEx.ParamName, false);
            }
            if (paramName.Split(' ', '\t', '\n', '\r', '\0').Count() != 1)
            {
                Description = $"The {nameof(argEx.ParamName)} should not have any whitespace seperating words";
                return new ConstraintResult(this, argEx.ParamName, false);
            }
            return new ConstraintResult(this, argEx, true);
        }

        protected virtual ConstraintResult ValidateMessage(ArgExType argEx)
        {
            string message = GetPrivateMessageString(argEx);
            if (string.IsNullOrWhiteSpace(message))
            {
                Description = $"The {nameof(argEx.Message)} should not be null or whiespace";
                return new ConstraintResult(this, argEx.Message, false);
            }
            if (message.Trim() != message)
            {
                Description = $"The {nameof(argEx.Message)} should not beginn or end with whiespace";
                return new ConstraintResult(this, argEx.Message, false);
            }
            if (message.Split(null).Count() < 2)
            {
                Description = $"The {nameof(argEx.Message)} should have at least 2 words";
                return new ConstraintResult(this, argEx.Message, false);
            }
            return new ConstraintResult(this, argEx, true);
        }

        protected static string GetPrivateMessageString(ArgExType argEx)
        {
            return (string)argEx.GetType().GetField("_message", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(argEx);
        }
    }

    internal class CustomArgumentExceptionConstraint : CustomArgumentExceptionConstraintBase<ArgumentException>
    { }

    internal class CustomArgumentNullExceptionConstraint : CustomArgumentExceptionConstraintBase<ArgumentNullException>
    {
        protected override ConstraintResult ValidateMessage(ArgumentNullException argEx)
        {
            if (string.IsNullOrEmpty(GetPrivateMessageString(argEx)))
                return new ConstraintResult(this, argEx, ConstraintStatus.Success);

            return base.ValidateMessage(argEx);
        }
    }

    internal class CustomArgumentOutOfRangeExceptionConstraint : CustomArgumentExceptionConstraintBase<ArgumentOutOfRangeException>
    {
        internal readonly bool ValidateActualValue;
        internal readonly object ExpectedActualValue;

        public CustomArgumentOutOfRangeExceptionConstraint() : this(null, false)
        { }

        public CustomArgumentOutOfRangeExceptionConstraint(object expectedActualValue) : this(expectedActualValue, true)
        { }

        private CustomArgumentOutOfRangeExceptionConstraint(object expectedActualValue, bool validateActualValue) : base()
        {
            ValidateActualValue = validateActualValue;
            ExpectedActualValue = expectedActualValue;
        }

        protected override ConstraintResult ValidateArgumentException(ArgumentOutOfRangeException argEx)
        {
            var baseRes = base.ValidateArgumentException(argEx);
            if (baseRes.Status != ConstraintStatus.Success) return baseRes;

            if (argEx.ActualValue is null)
            {
                Description = $"The {nameof(argEx.ActualValue)} is null, a {nameof(ArgumentNullException)} should be used instead";
                return new ConstraintResult(this, argEx.ActualValue, false);
            }
            if (ValidateActualValue && !argEx.ActualValue.Equals(ExpectedActualValue))
            {
                Description = $"The {nameof(argEx.ActualValue)} does not equal the expected {ExpectedActualValue}";
                return new ConstraintResult(this, argEx.ActualValue, false);
            }

            return new ConstraintResult(this, argEx, true);
        }
    }
}
