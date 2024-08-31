import { AuthorResponse } from "../../../..";

export interface CreateAuthorRequest {
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function authorToCreateRequest(author: AuthorResponse): CreateAuthorRequest {
    return {
        name: author.name,
        lastName: author.lastName,
        dateOfBirth: author.dateOfBirth
    }
}