using System.Collections.Generic;
using System.Linq;
using System;

using LogUploader.Interfaces;

namespace LogUploader.Test.EliteInsights
{
    internal interface IValidator
    {
        bool Skip { get; }
        void Validate(LogFull actual);
    }

    internal abstract class AbstractValidator : IValidator
    {
        public bool Skip { get => false; }

        public abstract void Validate(LogFull actual);
    }

    internal class AggergatValidator : AbstractValidator
    {
        private readonly List<IValidator> Validators;

        public AggergatValidator(params IValidator[] validators) : this(validators.ToList())
        { }

        public AggergatValidator(IEnumerable<IValidator> validators) : this(validators.ToList())
        { }

        public AggergatValidator(List<IValidator> validators)
        {
            if (validators.Any(validator => validator is null)) throw new ArgumentNullException(nameof(validators), "At leas one provided validator was null");
            Validators = validators;
        }

        public override void Validate(LogFull actual)
        {
            foreach (IValidator validator in Validators)
            {
                validator.Validate(actual);
            }
        }
    }

    internal class PlayerExistsValidator : AbstractValidator
    {
        private readonly string accountName;
        private readonly string charName;

        public PlayerExistsValidator(string name, bool isAccountName)
        {
            accountName = isAccountName ? name : null;
            charName = isAccountName ? null : name;
        }

        public override void Validate(LogFull actual)
        {
            throw new NotImplementedException();
        }
    }

    internal class SkipValidator : IValidator
    {
        public bool Skip { get => true; }

        public void Validate(LogFull actual)
        {
            return;
        }
    }
}
