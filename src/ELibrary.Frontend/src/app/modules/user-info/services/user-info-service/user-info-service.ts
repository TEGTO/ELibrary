import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { GetCurrentUserResponse } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class UserInfoService {
    abstract getUserInfo(): Observable<GetCurrentUserResponse>;
}