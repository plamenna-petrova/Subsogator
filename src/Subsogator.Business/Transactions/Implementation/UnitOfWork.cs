using Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Subsogator.Business.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Transactions.Implementation
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _applicatioDbContext;

        private readonly ILogger _logger;

        public UnitOfWork(ApplicationDbContext applicationDbContext, ILogger<UnitOfWork> logger)
        {
            _applicatioDbContext = applicationDbContext;
            _logger = logger;
        }

        public bool CommitSaveChanges()
        {
            try
            {
                int rowsAffected = _applicatioDbContext.SaveChanges();

                if (rowsAffected > 0)
                {
                    return true;
                }

                return false;
            }
            catch (DbUpdateException dbUpdateException) 
            {
                _logger.LogError("Exception: " + dbUpdateException.Message + 
                            "\n" + "Inner Exception :" +
                    dbUpdateException.InnerException.Message ?? "");

                return false;
            }
        }
    }
}
