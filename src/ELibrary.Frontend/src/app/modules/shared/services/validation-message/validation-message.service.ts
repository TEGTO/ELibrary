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
    else if (input.hasError('max')) {
      return { hasError: true, message: 'Field must be smaller.' };
    }
    else if (input.hasError('dateRangeFromInvalid')) {
      return { hasError: true, message: '"From" date must be before or equal to "To" date.' };
    }
    else if (input.hasError('dateRangeToInvalid')) {
      return { hasError: true, message: '"To" date must be after or equal to "From" date.' };
    }
    else if (input.hasError('priceRangeMinInvalid')) {
      return { hasError: true, message: '"Min" Price must be less than or equal to "Max".' };
    }
    else if (input.hasError('priceRangeMaxInvalid')) {
      return { hasError: true, message: '"Max" Price must be greater than or equal to "Min".' };
    }
    else if (input.hasError('pageAmountRangeMinInvalid')) {
      return { hasError: true, message: '"Min" Amount must be less than or equal to "Max".' };
    }
    else if (input.hasError('pageAmountRangeMaxInvalid')) {
      return { hasError: true, message: '"Max" Amount must be greater than or equal to "Min".' };
    }
    return { hasError: false, message: "" };
  }
}
