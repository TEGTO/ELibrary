
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
export function getDefaultAuthorResponse(): AuthorResponse {
    return {
        id: 0,
        name: "",
        lastName: "",
        dateOfBirth: new Date()
    }
}