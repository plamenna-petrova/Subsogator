namespace Subsogator.Business.Transactions.Interfaces
{
    public interface IUnitOfWork
    {
        bool CommitSaveChanges();
    }
}
