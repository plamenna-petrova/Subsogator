﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Common.GlobalConstants
{
    public class NotificationMessages
    {
        public const string CrewMemberEntityCreationSuccessMessage = "{0} {1} {2} " +
            "created successfully!";

        public const string ExistingCrewMemberEntityErrorMessage = "The {0} {1} {2} already exists!";

        public const string NewRecordFailedSaveErrorMessage = "Error, couldn't save " +
            "the new {0} record";

        public const string CrewMemberEntityUpdateSuccessMessage = "{0} {1} {2} saved successfully!";

        public const string RecordFailedUpdateSaveErrorMessage = "Error, couldn't " +
            "save the current {0} update";

        public const string RecordFailedDeletionErrorMessage = "Error, couldn't delete the {0}!";

        public const string CrewMemberEntityDeletionSuccessMessage = "{0} {1} {2} " +
            "deleted successfully";
    }
}