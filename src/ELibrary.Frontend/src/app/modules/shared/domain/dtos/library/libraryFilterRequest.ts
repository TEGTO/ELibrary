import { PaginatedRequest as PaginationRequest } from "../../..";

export interface LibraryFilterRequest extends PaginationRequest {
    containsName: string;
}

export function defaultLibraryFilterRequest(): LibraryFilterRequest {
    return {
        containsName: "",
        pageNumber: 0,
        pageSize: 0,
    };
}