using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private AppDbContext _context;

        public OrderHeaderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateOrderPaymentStatus(int id, string? orderStatus = null, string? paymentStatus = null)
        {
            var orderHeaderDB = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (orderHeaderDB == null) return;
            
            if (!String.IsNullOrEmpty(orderStatus)) orderHeaderDB.OrderStatus = orderStatus;
            if (!String.IsNullOrEmpty(paymentStatus)) orderHeaderDB.PaymentStatus = paymentStatus;
        }

        public void UpdateStripePaymentId(int id, string sessionId, string paymentItentId)
        {
            var orderHeaderDB = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (orderHeaderDB == null) return;

            if (!String.IsNullOrEmpty(sessionId)) orderHeaderDB.SessionId = sessionId;
            if (!String.IsNullOrEmpty(paymentItentId))
            {
                orderHeaderDB.PaymentIntentId = paymentItentId;
                orderHeaderDB.PaymentDate = DateTime.Now;
            }
        }
    }
}
