
export interface Genre {
    id: number;
    name: string;
}
export function getDefaultGenre(): Genre {
    return {
        id: 0,
        name: "",
    }
}