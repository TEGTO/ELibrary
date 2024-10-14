import { ClientResponse, GetClient, mapClientResponseToClient } from "../../../..";

export interface GetClientResponse {
    client: ClientResponse | null
}

export function mapGetClientResponseToGetClient(response: GetClientResponse): GetClient {
    return {
        client: response?.client === null ? null : mapClientResponseToClient(response.client),
    }
}