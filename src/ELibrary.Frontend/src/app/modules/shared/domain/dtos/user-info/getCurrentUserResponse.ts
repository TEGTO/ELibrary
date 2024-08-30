import { mapUserInfoDate, UserInfo } from "../../..";

export interface GetCurrentUserResponse {
    userName: string;
    userInfo: UserInfo;
}
export function mapGetCurrentUserResponseData(resp: GetCurrentUserResponse): GetCurrentUserResponse {
    return {
        ...resp,
        userInfo: mapUserInfoDate(resp.userInfo)
    }
}