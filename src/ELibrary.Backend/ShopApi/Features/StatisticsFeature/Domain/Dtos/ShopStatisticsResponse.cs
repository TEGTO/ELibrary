﻿namespace ShopApi.Features.StatisticsFeature.Domain.Dtos
{
    public class ShopStatisticsResponse
    {
        public long InCartCopies { get; set; }
        public long InOrderCopies { get; set; }
        public long SoldCopies { get; set; }
        public long CanceledCopies { get; set; }
        public long OrderAmount { get; set; }
        public long CanceledOrderAmount { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal EarnedMoney { get; set; }
        public Dictionary<DateTime, long> OrderAmountInDays { get; set; } = new();
    }
}