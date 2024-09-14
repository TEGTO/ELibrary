import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { AuthenticationValidationMessage } from '../..';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationValidationMessageService implements AuthenticationValidationMessage {
  getEmailValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string } {
    if (input.hasError('required')) {
      return { hasError: true, message: "Email is required." };
    }
    else if (input.hasError('email')) {
      return { hasError: true, message: "Email must be valid." };
    }
    return { hasError: false, message: "" };
  }
  getPasswordValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string } {
    if (input.hasError('required')) {
      return { hasError: true, message: "Password is required ." };
    }
    else if (input.hasError('minlength')) {
      return { hasError: true, message: "Password must be at least 8 characters." };
    }
    else if (input.hasError('nonAlphanumeric')) {
      return { hasError: true, message: "Password must have non alphanumeric characters." };
    }
    else if (input.hasError('digit')) {
      return { hasError: true, message: "Password must have digits." };
    }
    else if (input.hasError('digit')) {
      return { hasError: true, message: "Password must have upper case characters." };
    }
    return { hasError: false, message: "" };
  }
  getConfirmPasswordValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string } {
    if (input.hasError('required')) {
      return { hasError: true, message: "Password is required ." };
    }
    else if (input.hasError('passwordNoMatch')) {
      return {
        hasError: true, message: "Passwords don'tm match."
      };
    }
    return { hasError: false, message: "" };
  }
  getOldPasswordValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string } {
    if (input.hasError('required')) {
      return { hasError: true, message: "Password is required ." };
    }
    return { hasError: false, message: "" };
  }
}
