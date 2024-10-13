import { BookStatistics } from "../../../..";

export interface BookStatisticsResponse {
    inOrderCopies: number;
    inCartCopies: number;
    soldCopies: number;
    canceledOrders: number;
    averagePrice: number;
    stockAmount: number;
    earnedMoney: number;
}

export function mapBookStatisticsResponseToBookStatistics(response: BookStatisticsResponse): BookStatistics {
    return {
        inOrderCopies: response.inOrderCopies,
        inCartCopies: response.inCartCopies,
        soldCopies: response.soldCopies,
        canceledOrders: response.canceledOrders,
        averagePrice: response.averagePrice,
        stockAmount: response.stockAmount,
        earnedMoney: response.earnedMoney,
    }
}