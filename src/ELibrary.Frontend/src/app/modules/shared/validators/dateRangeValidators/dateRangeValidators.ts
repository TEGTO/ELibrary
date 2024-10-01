import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

export function dateRangeValidatorFrom(toControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const from = control.value;
        const to = formGroup.get(toControlName)?.value;

        if (from && to && new Date(from) > new Date(to)) {
            return { dateRangeFromInvalid: true };
        }

        return null;
    };
}
export function dateRangeValidatorTo(fromControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const from = formGroup.get(fromControlName)?.value;
        const to = control.value;

        if (from && to && new Date(from) > new Date(to)) {
            return { dateRangeToInvalid: true };
        }

        return null;
    };
}