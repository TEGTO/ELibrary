import { Publisher } from "../../../..";

export interface CreatePublisherRequest {
    name: string;
}
export function mapPublisherToCreatePublisherRequest(publisher: Publisher): CreatePublisherRequest {
    return {
        name: publisher.name
    }
}