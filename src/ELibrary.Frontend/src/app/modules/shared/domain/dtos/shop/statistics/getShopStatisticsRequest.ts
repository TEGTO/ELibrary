import { GetShopStatistics, mapBookToStatisticsBook, StatisticsBook } from "../../../..";

export interface GetShopStatisticsRequest {
    fromUTC: Date | null,
    toUTC: Date | null,
    includeBooks: StatisticsBook[]
}

export function mapGetShopStatisticsToGetShopStatisticsRequest(statistics: GetShopStatistics): GetShopStatisticsRequest {
    return {
        fromUTC: statistics.fromUTC,
        toUTC: statistics.toUTC,
        includeBooks: statistics.includeBooks.map(x => mapBookToStatisticsBook(x))
    }
}