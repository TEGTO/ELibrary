import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function phoneValidator(minLength: number, maxLength: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!control.value) {
            return null;
        }

        const phone = control.value;
        const isNumeric = /^\d*$/.test(phone);

        if (!isNumeric) {
            return { invalidPhone: 'Phone number must contain only numeric characters.' };
        }
        if (phone.length < minLength) {
            return { minlength: `Phone number must be at least ${minLength} characters.` };
        }
        if (phone.length > maxLength) {
            return { maxlength: `Phone number cannot exceed ${maxLength} characters.` };
        }

        return null;
    };
}