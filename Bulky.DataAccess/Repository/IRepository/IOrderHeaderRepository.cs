using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateOrderPaymentStatus (int id, string? orderStatus = null, string? paymentStatus = null);
        void UpdateStripePaymentId(int id, string sessionId, string paymentItentId);
    }
}
