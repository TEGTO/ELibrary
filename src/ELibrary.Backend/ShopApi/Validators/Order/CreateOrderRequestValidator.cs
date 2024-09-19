﻿
using FluentValidation;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi.Validators.Order
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator(IConfiguration configuration)
        {
            int maxAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
            RuleFor(x => x.DeliveryAddress).NotNull().NotEmpty().MaximumLength(512);
            RuleFor(x => x.DeliveryTime).NotNull().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.OrderBooks)
              .NotNull()
              .NotEmpty()
              .Must(orderBooks => orderBooks != null && orderBooks.Count <= maxAmount)
              .WithMessage($"The maximum number of books in an order is {maxAmount}.");
        }
    }
}
