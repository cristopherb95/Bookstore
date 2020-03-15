using System;
namespace Bookstore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }

        ISP_Call SP_Call { get; }

        ICoverTypeRepository CoverType { get; }

        IProductRepository Product { get; }

        ICompanyRepository Company { get; }
        
        IShoppingCartRepository ShoppingCart { get; }
        
        IOrderDetailsRepository OrderDetails { get; }
        
        IOrderHeaderRepository OrderHeader { get; }

        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
