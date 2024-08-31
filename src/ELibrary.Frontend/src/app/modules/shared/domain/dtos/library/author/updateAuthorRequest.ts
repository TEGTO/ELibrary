import { AuthorResponse } from "./authorResponse";

export interface UpdateAuthorRequest {
    id: number;
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function authorToUpdateRequest(author: AuthorResponse): UpdateAuthorRequest {
    return {
        id: author.id,
        name: author.name,
        lastName: author.lastName,
        dateOfBirth: author.dateOfBirth
    }
}