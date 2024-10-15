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
    orderAmountInDays: { date: Date, count: number }[];
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
        orderAmountInDays: new Map(
            Object.entries(response.orderAmountInDays).map(
                ([date, count]) => [new Date(date), count as unknown as number]
            )
        )
    }
}