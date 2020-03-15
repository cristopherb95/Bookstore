using Bookstore.Models;

namespace Bookstore.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository
    {
        void Update(ShoppingCart shoppingCart);
    }
}