using Bookstore.Models;

namespace Bookstore.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository
    {
        void Update(OrderHeader orderHeader);
    }
}