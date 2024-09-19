export interface Author {
    id: number;
    name: string;
    lastName: string;
    dateOfBirth: Date;
}

export function getDefaultAuthor(): Author {
    return {
        id: 0,
        name: "",
        lastName: "",
        dateOfBirth: new Date()
    }
}