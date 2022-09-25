using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Common.GlobalConstants
{
    public class NotificationMessages
    {
        public const string NewRecordFailedSaveErrorMessage = "Error, couldn't save " +
            "the new {0} record";

        public const string RecordFailedUpdateSaveErrorMessage = "Error, couldn't " +
            "save the current {0} update";

        public const string RecordFailedDeletionErrorMessage = "Error, couldn't delete the {0}!";

        public const string ExistingRecordErrorMessage = "Error, the {0} {1} already exists!";

        public const string RecordCreationSuccessMessage = "{0} {1} " +
            "created successfully!";

        public const string RecordUpdateSuccessMessage = "{0} {1} saved successfully!";

        public const string RecordDeletionSuccessMessage = "{0} {1} deleted successfully!";
    }
}
