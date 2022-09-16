using Data.DataAccess;
using Subsogator.Business.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Transactions.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext applicatioDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this.applicatioDbContext = applicationDbContext;
        }

        public bool CommitChanges()
        {
            if (applicatioDbContext.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
