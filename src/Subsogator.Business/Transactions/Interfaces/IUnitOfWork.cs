using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Transactions.Interfaces
{
    public interface IUnitOfWork
    {
        bool CommitSaveChanges();
    }
}
