import { AbstractControl, ValidationErrors } from "@angular/forms";

export function inputSelectValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;

    if (!value) {
        return null;
    }

    if (value && typeof value === 'object' && 'id' in value) {
        return null;
    }

    return { invalidSelectInput: true };

}