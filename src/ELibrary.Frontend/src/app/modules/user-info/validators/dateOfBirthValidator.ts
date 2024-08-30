import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function dateOfBirthValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const selectedDate = new Date(control.value);
        const today = new Date();
        if (selectedDate >= today) {
            return { invalidDateOfBirth: 'Date of birth must be in the past' };
        }
        return null;
    };
}