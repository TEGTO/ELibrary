import { BookStatistics } from "../../../..";

export interface BookStatisticsResponse {
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

export function mapBookStatisticsResponseToBookStatistics(response: BookStatisticsResponse): BookStatistics {
    return {
        inCartCopies: response.inCartCopies,
        inOrderCopies: response.inOrderCopies,
        soldCopies: response.soldCopies,
        canceledCopies: response.canceledCopies,
        orderAmount: response.orderAmount,
        canceledOrderAmount: response.canceledOrderAmount,
        averagePrice: response.averagePrice,
        stockAmount: response.stockAmount,
        earnedMoney: response.earnedMoney,
    }
}