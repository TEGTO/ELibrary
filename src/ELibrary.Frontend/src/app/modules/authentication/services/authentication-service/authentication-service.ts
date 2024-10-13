/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthToken, UserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthenticationService {
    abstract registerUser(req: UserRegistrationRequest): Observable<boolean>;
    abstract getUserAuth(): Observable<UserAuth>;
    abstract signInUser(req: UserAuthenticationRequest): void;
    abstract logOutUser(): void;
    abstract refreshToken(authToken: AuthToken): Observable<boolean>;
    abstract deleteUserAuth(): void;
    abstract updateUserAuth(req: UserUpdateRequest): Observable<boolean>;
    abstract getAuthErrors(): Observable<any>;
}
