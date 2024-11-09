
export interface ShopStatistics {
    inCartCopies: number;
    inOrderCopies: number;
    soldCopies: number;
    canceledCopies: number;
    orderAmount: number;
    canceledOrderAmount: number;
    averagePrice: number;
    earnedMoney: number;
    orderAmountInDays: Map<Date, number>;
}

export function getDefaultShopStatistics(): ShopStatistics {
    return {
        inCartCopies: 0,
        inOrderCopies: 0,
        soldCopies: 0,
        canceledCopies: 0,
        orderAmount: 0,
        canceledOrderAmount: 0,
        averagePrice: 0,
        earnedMoney: 0,
        orderAmountInDays: new Map<Date, number>()
    }
}