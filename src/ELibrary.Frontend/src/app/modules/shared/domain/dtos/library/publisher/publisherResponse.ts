export interface PublisherResponse {
    id: number;
    name: string;
}
export function getDefaultPublisherResponse(): PublisherResponse {
    return {
        id: 0,
        name: "",
    }
}