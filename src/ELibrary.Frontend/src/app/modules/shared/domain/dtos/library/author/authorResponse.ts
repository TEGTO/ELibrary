import { UpdateAuthorRequest } from "../../../..";

export interface AuthorResponse {
    id: number;
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function mapAuthorData(resp: AuthorResponse): AuthorResponse {
    return {
        ...resp,
        dateOfBirth: new Date(resp.dateOfBirth)
    }
}
export function getAuthorFromUpdateRequest(request: UpdateAuthorRequest): AuthorResponse {
    return {
        id: request.id,
        name: request.name,
        lastName: request.lastName,
        dateOfBirth: request.dateOfBirth
    }
}