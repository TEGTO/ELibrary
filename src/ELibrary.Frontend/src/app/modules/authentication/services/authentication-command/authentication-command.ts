import { Injectable } from "@angular/core";

export enum AuthenticationCommandType {
    SignUp, SignIn, LogOut, Update
}
@Injectable({
    providedIn: 'root'
})
export abstract class AuthenticationCommand {
    abstract dispatchCommand(commandType: AuthenticationCommandType, dispatchedFrom: any, ...params: any[]): void;
}
