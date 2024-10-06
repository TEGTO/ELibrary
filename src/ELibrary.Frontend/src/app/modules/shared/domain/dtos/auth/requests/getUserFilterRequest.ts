import { PaginatedRequest } from "../../../..";

export interface GetUserFilterRequest extends PaginatedRequest {
    containsInfo: string;
}