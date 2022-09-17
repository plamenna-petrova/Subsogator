using Data.DataAccess;
using Subsogator.Business.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Transactions.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _applicatioDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicatioDbContext = applicationDbContext;
        }

        public bool CommitSaveChanges()
        {
            int rowsAffected = _applicatioDbContext.SaveChanges();

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }
    }
}
