import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function minDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const selectedDate = new Date(control.value);
        const today = new Date();
        if (selectedDate >= today) {
            return { invalidMinDate: 'Date must be in the past' };
        }
        return null;
    };
}