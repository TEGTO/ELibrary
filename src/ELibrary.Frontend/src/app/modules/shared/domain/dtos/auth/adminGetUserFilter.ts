import { PaginatedRequest } from "../../..";

export interface AdminGetUserFilter extends PaginatedRequest {
    containsInfo: string;
}

export function getDefaultAdminGetUserFilter(): AdminGetUserFilter {
    return {
        pageNumber: 0,
        pageSize: 0,
        containsInfo: ""
    }
}