
export interface BookStatistics {
    inCartCopies: number;
    inOrderCopies: number;
    soldCopies: number;
    canceledCopies: number;
    orderAmount: number;
    canceledOrderAmount: number;
    averagePrice: number;
    stockAmount: number;
    earnedMoney: number;
}

export function getDefaultBookStatistics(): BookStatistics {
    return {
        inCartCopies: 0,
        inOrderCopies: 0,
        soldCopies: 0,
        canceledCopies: 0,
        orderAmount: 0,
        canceledOrderAmount: 0,
        averagePrice: 0,
        stockAmount: 0,
        earnedMoney: 0,
    }
}