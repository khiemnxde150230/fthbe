﻿using backend.DTOs;
using backend.Models;

namespace backend.Repositories.PaymentRepository
{
    public interface IPaymentRepository
    {
        object ReturnAvaliableTicket(int eventId);
        object Payment(PaymentDTO paymentDTO);

        object ReturnPaymentUrl(HttpContext context, int _orderId, string _discounrCode);
        object PaymentExcute(IQueryCollection collections);
        object DeleteTimeOutOrder(PaymentDTO paymentDTO);
        object CheckInputCoupon(int eventId, string coupon);
        object CancelOrderOfUser(int userId);
        object CheckOrderdOfUser(int userId, int eventId);
    }
}
