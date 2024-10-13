import { Book } from "../../../..";

export interface GetBookStatistics {
    fromUTC: Date | null,
    toUTC: Date | null,
    includeBooks: Book[]
}

export function getDefaultGetBookStatistics(): GetBookStatistics {
    return {
        fromUTC: null,
        toUTC: null,
        includeBooks: []
    }
}