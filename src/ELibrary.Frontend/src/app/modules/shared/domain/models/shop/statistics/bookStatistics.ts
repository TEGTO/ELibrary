
export interface BookStatistics {
    inOrderCopies: number;
    inCartCopies: number;
    soldCopies: number;
    canceledOrders: number;
    averagePrice: number;
    stockAmount: number;
    earnedMoney: number;
}

export function getDefaultBookStatistics() {
    return {
        inOrderCopies: 0,
        inCartCopies: 0,
        soldCopies: 0,
        canceledOrders: 0,
        averagePrice: 0,
        stockAmount: 0,
        earnedMoney: 0,
    }
}