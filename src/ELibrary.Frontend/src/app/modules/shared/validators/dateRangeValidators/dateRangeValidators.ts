import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";
import { getDateOrNull } from "../..";

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

export function dateRangeWithTimeValidatorFrom(fromControlName: string, fromTimeControlName: string, toControlName: string, toTimeControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const result = checkDateWithTime(formGroup, fromControlName, fromTimeControlName, toControlName, toTimeControlName)
        if (!result) {
            return { dateRangeFromInvalid: true };
        }
        return null;
    };
}
export function dateRangeWithTimeValidatorTo(fromControlName: string, fromTimeControlName: string, toControlName: string, toTimeControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const result = checkDateWithTime(formGroup, fromControlName, fromTimeControlName, toControlName, toTimeControlName)
        if (!result) {
            return { dateRangeToInvalid: true };
        }
        return null;
    };
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function checkDateWithTime(formGroup: FormGroup<any>, fromControlName: string, fromTimeControlName: string, toControlName: string, toTimeControlName: string) {
    const from = getDateOrNull(formGroup.get(fromControlName)?.value, formGroup.get(fromTimeControlName)?.value);
    const to = getDateOrNull(formGroup.get(toControlName)?.value, formGroup.get(toTimeControlName)?.value);

    if (from && to) {
        if (from && to && new Date(from) > new Date(to)) {
            return false;
        }
    }

    return true;
}