import { ShopStatistics } from "../../../..";

export interface ShopStatisticsResponse {
    inCartCopies: number;
    inOrderCopies: number;
    soldCopies: number;
    canceledCopies: number;
    orderAmount: number;
    canceledOrderAmount: number;
    averagePrice: number;
    earnedMoney: number;
    orderAmountInDays: { date: Date, count: number }[];
}

export function mapShopStatisticsResponseToShopStatistics(response: ShopStatisticsResponse): ShopStatistics {
    return {
        inCartCopies: response.inCartCopies,
        inOrderCopies: response.inOrderCopies,
        soldCopies: response.soldCopies,
        canceledCopies: response.canceledCopies,
        orderAmount: response.orderAmount,
        canceledOrderAmount: response.canceledOrderAmount,
        averagePrice: response.averagePrice,
        earnedMoney: response.earnedMoney,
        orderAmountInDays: new Map(
            Object.entries(response.orderAmountInDays).map(
                ([date, count]) => [new Date(date), count as unknown as number]
            )
        )
    }
}