import { PaginatedRequest } from "../../paginatedRequest";

export interface GetOrdersFilter extends PaginatedRequest {
    clientId: string | undefined;
}

export function getDefaultGetOrdersFilter(): GetOrdersFilter {
    return {
        pageNumber: 0,
        pageSize: 0,
        clientId: undefined
    }
}