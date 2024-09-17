/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { ValidationMessage } from './validation-message';

@Injectable({
  providedIn: 'root'
})
export class ValidationMessageService implements ValidationMessage {

  getValidationMessage(input: AbstractControl<any, any>): { hasError: boolean, message: string } {
    if (input.hasError('required')) {
      return { hasError: true, message: "Input is required." };
    }
    else if (input.hasError('email')) {
      return { hasError: true, message: "Email must be valid." };
    }
    else if (input.hasError('minlength')) {
      return { hasError: true, message: "Input must be at least 8 characters." };
    }
    else if (input.hasError('nonAlphanumeric')) {
      return { hasError: true, message: "Input must have non alphanumeric characters." };
    }
    else if (input.hasError('digit')) {
      return { hasError: true, message: "Input must have digits." };
    }
    else if (input.hasError('uppercase')) {
      return { hasError: true, message: "Input must have upper case characters." };
    }
    else if (input.hasError('passwordNoMatch')) {
      return { hasError: true, message: "Passwords don't match." };
    }
    else if (input.hasError('invalidMinDate')) {
      return { hasError: true, message: "Date must be in the past." };
    }
    else if (input.hasError('min')) {
      return { hasError: true, message: "Field must be bigger." };
    }
    return { hasError: false, message: "" };
  }
}
