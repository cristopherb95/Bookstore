using Bookstore.Models;

namespace Bookstore.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository
    {
        void Update(OrderDetails orderDetails);
    }
}