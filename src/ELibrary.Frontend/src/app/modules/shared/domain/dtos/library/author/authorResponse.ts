export interface AuthorResponse {
    id: number;
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function mapAuthorResponseData(resp: AuthorResponse): AuthorResponse {
    return {
        ...resp,
        dateOfBirth: new Date(resp.dateOfBirth)
    }
}