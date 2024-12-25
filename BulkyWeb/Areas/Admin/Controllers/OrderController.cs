using AutoMapper;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork, ILogger<OrderController> logger, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        #region ACTIONS


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            if (orderId == 0) return ResourceNotFound();
            var order = _unitOfWork.OrderRepo.GetFunc(
                filter: o => o.Id == orderId,
                include: query => query
                 .Include(o => o.ApplicationUser)
                 .Include(o => o.PurchaseItems).ThenInclude(i => i.Product)
                 .Include(o => o.Delivery).ThenInclude(d => d.Address)
                 .Include(o => o.Payment));
            if (order == null) return ResourceNotFound();

            return View(_mapper.Map<OrderVM>(order));
        }

        [HttpPost]
        [Authorize(Roles = $"{CONST_Roles.Admin},{CONST_Roles.Employee}")]
        public IActionResult UpdateDeliveryDetails(OrderVM ordeVM)
        {
            try
            {
                var orderDb = _unitOfWork.OrderRepo.GetFunc(
                filter: o => o.Id == ordeVM.Id,
                include: query => query
                 .Include(o => o.Delivery));
                if (orderDb == null) return ResourceNotFound();

                orderDb.Delivery.RecipientName = ordeVM.Delivery.RecipientName;
                orderDb.Delivery.PhoneNumber = ordeVM.Delivery.PhoneNumber;
                orderDb.Delivery.Address.StreetAddress = ordeVM.Delivery.Address.StreetAddress;
                orderDb.Delivery.Address.City = ordeVM.Delivery.Address.City;
                orderDb.Delivery.Address.State = ordeVM.Delivery.Address.State;
                orderDb.Delivery.Address.PostalCode = ordeVM.Delivery.Address.PostalCode;
                orderDb.UpdatedOn = DateTime.Now;

                _unitOfWork.OrderRepo.Update(orderDb);
                _unitOfWork.Save();

                TempData["successMessage"] = "Order updated successfully";
                return RedirectToAction(nameof(Details), new { orderid = orderDb.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Error in UpdateOrder");
                TempData["errorMessage"] = "Something went wrong but don't be sad, it wasn't your fault.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{CONST_Roles.Admin},{CONST_Roles.Employee}")]
        public IActionResult StartProcessing(int orderId)
        {
            try
            {
                var orderDb = _unitOfWork.OrderRepo.GetFunc(filter: o => o.Id == orderId);
                if (orderDb == null) return ResourceNotFound();
                orderDb.Status = CONST_OrderStatus.InProcess;

                _unitOfWork.OrderRepo.Update(orderDb);
                _unitOfWork.Save();

                TempData["successMessage"] = "Order Detail is in process";
                return RedirectToAction(nameof(Details), new { orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Error in StartProcessing");
                TempData["errorMessage"] = "Something went wrong but don't be sad, it wasn't your fault.";
                return RedirectToAction(nameof(Details), new { orderId });
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{CONST_Roles.Admin},{CONST_Roles.Employee}")]
        public IActionResult ShipOrder(OrderVM orderVM)
        {
            try
            {
                var orderDb = _unitOfWork.OrderRepo.GetFunc(
                    filter: o => o.Id == orderVM.Id,
                    include: query => query
                     .Include(o => o.Delivery)
                     .Include(o => o.Payment));
                if (orderDb == null) return ResourceNotFound();

                orderDb.Delivery.TrackingNumber = orderVM.Delivery.TrackingNumber;
                orderDb.Delivery.CarrierName = orderVM.Delivery.CarrierName;
                orderDb.Status = CONST_OrderStatus.Shipped;
                orderDb.Delivery.DateDelivery = DateTime.Now;
                if (orderDb.Payment.PaymentStatus == CONST_PaymentStatus.DelayedPayment)
                    orderDb.Payment.PaymentDueDate = DateTime.Now.AddDays(30);

                _unitOfWork.OrderRepo.Update(orderDb);
                _unitOfWork.Save();

                TempData["successMessage"] = "Order Shipped Successfuly";
                return RedirectToAction(nameof(Details), new { orderId = orderVM.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error in ShipOrder");
                TempData["errorMessage"] = "Something went wrong but don't be sad, it wasn't your fault.";
                return RedirectToAction(nameof(Details), new { orderId = orderVM.Id });
            }
        }

        [HttpPost]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            try
            {
                var orderDb = _unitOfWork.OrderRepo.GetFunc(
                    filter: o => o.Id == orderVM.Id,
                    include: query => query
                     .Include(o => o.Delivery)
                     .Include(o => o.Payment));
                if (orderDb == null) return ResourceNotFound();

                if (orderDb.Payment.PaymentStatus == CONST_PaymentStatus.Approved)
                {
                    var refundOptions = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderDb.Payment.PaymentIntentId
                    };
                    var Refund = new RefundService().Create(refundOptions);

                    orderDb.Payment.PaymentStatus = CONST_PaymentStatus.Refunded;
                    orderDb.Status = CONST_OrderStatus.Cancelled;
                }
                else
                {
                    orderDb.Payment.PaymentStatus = CONST_PaymentStatus.Cancelled;
                    orderDb.Status = CONST_OrderStatus.Cancelled;
                }

                _unitOfWork.OrderRepo.Update(orderDb);
                _unitOfWork.Save();

                TempData["successMessage"] = "Order cancelled successfully";
                return RedirectToAction(nameof(Details), new { orderId = orderVM.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error in CancelOrder");
                TempData["errorMessage"] = "Something went wrong but don't be sad, it wasn't your fault.";
                return RedirectToAction(nameof(Details), new{ orderId = orderVM.Id });
            }
        }

        [HttpPost]
        public IActionResult PayNow(int orderId)
        {
            try
            {
                var orderDb = _unitOfWork.OrderRepo.GetFunc(
                    filter: o => o.Id == orderId,
                    include: query => query
                     .Include(o => o.Payment)
                     .Include(o=>o.PurchaseItems).ThenInclude(i=>i.Product));
                if (orderDb == null) return ResourceNotFound();

                var session = PaymentForStripe(orderDb);
                if (session.Id is null)
                {
                    TempData["errorMessage"] = "Payment failed";
                    return RedirectToAction(nameof(Details), new { orderId });
                }
                orderDb.Payment.SessionId = session.Id;

                _unitOfWork.OrderRepo.Update(orderDb);
                _unitOfWork.Save();

                Response.Headers.Location = session.Url;
                return new StatusCodeResult(303);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error in PayNowForCompanyOrder");
                TempData["errorMessage"] = "Something went wrong but don't be sad, it wasn't your fault.";
                return RedirectToAction(nameof(Details), new { orderId });
            }
        }

        public async Task<IActionResult> PaymentConfirmation(int orderId)
        {
            try
            {
                var orderDb = _unitOfWork.OrderRepo.GetFunc(
                    filter: o => o.Id == orderId,
                    include: query => query
                     .Include(o => o.Payment)
                     .Include(o => o.PurchaseItems).ThenInclude(i => i.Product)
                     .Include(o => o.ApplicationUser));
                if (orderDb?.Payment == null || orderDb?.PurchaseItems == null || orderDb?.ApplicationUser == null) 
                    return ResourceNotFound();

                if (orderDb.Payment.PaymentStatus == CONST_PaymentStatus.Approved)
                {
                    TempData["errorMessage"] = "Order has already been paid";
                    return RedirectToAction(nameof(Details), new { orderId });
                }

                var stripeSession = new SessionService().Get(orderDb.Payment.SessionId);
                if (stripeSession.PaymentStatus.ToLower() == "paid")
                {
                    orderDb.Payment.PaymentIntentId = stripeSession.PaymentIntentId;
                    orderDb.Payment.PaymentStatus = CONST_PaymentStatus.Approved;
                    orderDb.Payment.PaymentDate = DateTime.Now;
                    orderDb.UpdatedOn = DateTime.Now;

                    var userNotCompanyRole = !await _userManager.IsInRoleAsync(orderDb.ApplicationUser, CONST_Roles.Company);
                    if (userNotCompanyRole)
                    {
                        orderDb.Status = CONST_OrderStatus.Approved;
                    }

                    _unitOfWork.OrderRepo.Update(orderDb);
                    _unitOfWork.Save();
                } else
                {
                    TempData["errorMessage"] = "Payment failed";
                    return RedirectToAction(nameof(Details), new { orderId });
                }

                return View(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Error in PaymentConfirmation");
                TempData["errorMessage"] = "Something went wrong but don't be sad, it wasn't your fault.";
                return RedirectToAction(nameof(Details), new { orderId });
            }
        }


        #endregion

        #region API's
        [HttpGet]
        [Route("API/[area]/[controller]/GetAll")]
        public IActionResult GetAll(string? filterStatus = "all")
        {
            IEnumerable<Order> orders;
            if (User.IsInRole(CONST_Roles.Admin) || User.IsInRole(CONST_Roles.Employee))
            {
                orders = _unitOfWork.OrderRepo.GetAllFunc(
                    include: query => query
                     .Include(o => o.ApplicationUser)
                     .Include(o => o.PurchaseItems)
                     .Include(o => o.Delivery)
                     .Include(o => o.Payment));
            }
            else
            {
                var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                orders = _unitOfWork.OrderRepo.GetAllFunc(
                    filter: o => o.ApplicationUserId == userId,
                    include: query => query
                    .Include(o => o.ApplicationUser)
                    .Include(o => o.PurchaseItems)
                    .Include(o => o.Delivery)
                    .Include(o => o.Payment));
            }

            var filteredOrders = filterStatus switch
            {
                "all" => orders,
                "inProcess" => orders.Where(o => o.Status == CONST_OrderStatus.InProcess),
                "pending" => orders.Where(o => o.Status == CONST_OrderStatus.Pending),
                "approved" => orders.Where(o => o.Status == CONST_OrderStatus.Approved),
                "completed" => orders.Where(o => o.Status == CONST_OrderStatus.Shipped),
                _ => orders
            };

            return Json(new { success = true, data = filteredOrders });
        }

        #endregion


        #region PRIVATE METHODS


        private Session PaymentForStripe(Order order)
        {
            var host = $"{Request.Scheme}://{Request.Host.Value}";
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{host}/Admin/Order/PaymentConfirmation?orderId={order.Id}",
                CancelUrl = $"{host}/Admin/Order/details?orderId={order.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            var sessionLineItems = order.PurchaseItems
                .Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.UnitPrice * 100),
                        Currency = "brl",
                        ProductData = new SessionLineItemPriceDataProductDataOptions { Name = item.Product?.Title }
                    },
                    Quantity = item.Quantity
                })
                .ToList();

            options.LineItems.AddRange(sessionLineItems);

            Session stripeSession = new SessionService().Create(options);
            return stripeSession;
        }

        public IActionResult ResourceNotFound()
        {
            TempData["errorMessage"] = "Resource not found";
            return RedirectToAction(nameof(Index));
        }


        #endregion
    }
}
