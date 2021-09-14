using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework.Constraints;

namespace LogUploader.Test.Constraints
{
    internal static class CustomConstraintExtensiones
    {
        public static CustomArgumentExceptionConstraint ValidateArgumentException(this ConstraintExpression expression)
        {
            var constraint = new CustomArgumentExceptionConstraint();
            expression.Append(constraint);
            return constraint;
        }

        public static CustomArgumentNullExceptionConstraint ValidateArgumentNullException(this ConstraintExpression expression)
        {
            var constraint = new CustomArgumentNullExceptionConstraint();
            expression.Append(constraint);
            return constraint;
        }

        public static CustomArgumentOutOfRangeExceptionConstraint ValidateArgumentOutOfRangeException(this ConstraintExpression expression)
        {
            var constraint = new CustomArgumentOutOfRangeExceptionConstraint();
            expression.Append(constraint);
            return constraint;
        }

        public static CustomArgumentOutOfRangeExceptionConstraint ValidateArgumentOutOfRangeException(this ConstraintExpression expression, object expectedActualValue)
        {
            var constraint = new CustomArgumentOutOfRangeExceptionConstraint(expectedActualValue);
            expression.Append(constraint);
            return constraint;
        }
    }

    internal class Throws : NUnit.Framework.Throws
    {
        public static CustomArgumentExceptionConstraint ValidateArgumentException { get => new CustomArgumentExceptionConstraint(); }
        public static CustomArgumentNullExceptionConstraint ValidateArgumentNullException { get => new CustomArgumentNullExceptionConstraint(); }
        public static CustomArgumentOutOfRangeExceptionConstraint ValidateArgumentOutOfRangeException() => new CustomArgumentOutOfRangeExceptionConstraint();
        public static CustomArgumentOutOfRangeExceptionConstraint ValidateArgumentOutOfRangeException(object expectedActualValue) => new CustomArgumentOutOfRangeExceptionConstraint(expectedActualValue);
    }
}
