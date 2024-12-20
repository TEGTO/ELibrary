﻿namespace ShopApi.Features.StatisticsFeature.Domain.Models
{
    public class GetShopStatisticsFilter
    {
        public DateTime? FromUTC { get; set; }
        public DateTime? ToUTC { get; set; }
        public StatisticsBook[] IncludeBooks { get; set; } = [];
    }
}
