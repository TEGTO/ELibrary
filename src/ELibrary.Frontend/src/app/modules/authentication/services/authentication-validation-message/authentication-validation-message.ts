import { Injectable } from "@angular/core";
import { AbstractControl } from "@angular/forms";

@Injectable({
    providedIn: 'root'
})
export abstract class AuthenticationValidationMessage {
    abstract getEmailValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string };
    abstract getPasswordValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string };
    abstract getConfirmPasswordValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string };
    abstract getOldPasswordValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string };
}
