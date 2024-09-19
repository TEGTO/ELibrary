/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthData, AuthToken, UserAuthenticationRequest, UserData, UserRegistrationRequest, UserUpdateRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthenticationService {
    //Registration
    abstract registerUser(userRegistrationData: UserRegistrationRequest): Observable<boolean>;
    abstract getRegistrationErrors(): Observable<any>;
    //Auth
    abstract getAuthData(): Observable<AuthData>;
    abstract getAuthErrors(): Observable<any>;
    abstract singInUser(authRequest: UserAuthenticationRequest): void;
    abstract logOutUser(): void;
    abstract refreshToken(authToken: AuthToken): Observable<boolean>;
    abstract deleteUser(): void;
    //User
    abstract getUserData(): Observable<UserData>;
    abstract updateUserData(req: UserUpdateRequest): Observable<boolean>;
    abstract getUserErrors(): Observable<any>;
}
