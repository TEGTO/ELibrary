import { GetBookStatistics, mapBookToStatisticsBook, StatisticsBook } from "../../../..";

export interface GetBookStatisticsRequest {
    fromUTC: Date | null,
    toUTC: Date | null,
    includeBooks: StatisticsBook[]
}

export function mapGetBookStatisticsToGetBookStatisticsRequest(statistics: GetBookStatistics): GetBookStatisticsRequest {
    return {
        fromUTC: statistics.fromUTC,
        toUTC: statistics.toUTC,
        includeBooks: statistics.includeBooks.map(x => mapBookToStatisticsBook(x))
    }
}