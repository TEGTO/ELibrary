export interface Publisher {
    id: number;
    name: string;
}
export function getDefaultPublisher(): Publisher {
    return {
        id: 0,
        name: "",
    }
}