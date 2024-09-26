using BulkyBook.Models.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader order);

        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);

        void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);
    }
}
