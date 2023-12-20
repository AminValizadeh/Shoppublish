﻿using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Common;

namespace Shop.Application.Interfaces
{
    public interface IPaymentService
    {
        PaymentStatus CreatePaymentRequest(string merchantId, int amount, string description,
           string callbackUrl, ref string redirectUrl, string? userEmail = null, string? userMobile = null);
        PaymentStatus PaymentVerification(string merchantId, string authority, int amount, ref long refId);
        string? GetAuthorityCodeFromCallback(HttpContext context);
    }
}
