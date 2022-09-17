using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Common.GlobalConstants
{
    public class ValidationConstants
    {
        public const string ActorFirstNameMinimumLengthValidationMessage = "The first name of the " +
            "actor cannot be shorter than 2 symbols";

        public const string ActorFirstNameMaximumLengthValidationMessage = "The first name " +
            "of the actor cannot be longer than 25 symbols";

        public const string ActorLastNameMinimumLengthValidationMessage = "The last name of the " +
             "actor cannot be shorter than 2 symbols";

        public const string ActorLastNameMaximumLengthValidationMessage = "The last name " +
            "of the actor cannot be longer than 25 symbols";
    }
}
