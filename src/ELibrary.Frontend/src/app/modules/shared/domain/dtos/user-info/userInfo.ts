
export interface UserInfo {
    name: string;
    lastName: string;
    dateOfBirth: Date;
    address: string;
}
export function mapUserInfoDate(info: UserInfo): UserInfo {
    return {
        ...info,
        dateOfBirth: new Date(info.dateOfBirth)
    }
}