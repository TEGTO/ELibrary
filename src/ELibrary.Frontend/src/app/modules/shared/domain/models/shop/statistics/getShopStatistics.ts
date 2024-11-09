import { Book } from "../../../..";

export interface GetShopStatistics {
    fromUTC: Date | null,
    toUTC: Date | null,
    includeBooks: Book[]
}

export function getDefaultGetShopStatistics(): GetShopStatistics {
    return {
        fromUTC: null,
        toUTC: null,
        includeBooks: []
    }
}