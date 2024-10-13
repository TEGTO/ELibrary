import { Book } from "../../../..";

export interface StatisticsBook {
    id: number;
}

export function mapBookToStatisticsBook(book: Book): StatisticsBook {
    return {
        id: book.id
    }
}