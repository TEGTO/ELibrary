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
export function priceRangeMinValidator(maxControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const minPrice = control.value;
        const maxPrice = formGroup.get(maxControlName)?.value;;

        if (minPrice !== null && minPrice < 0) {
            return { min: true };
        }
        if (maxPrice !== null && maxPrice < 0) {
            return { min: true };
        }
        if (minPrice !== null && maxPrice !== null && minPrice > maxPrice) {
            return { priceRangeMinInvalid: true };
        }

        return null;
    };
}
export function priceRangeMaxValidator(minControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const minPrice = formGroup.get(minControlName)?.value;
        const maxPrice = control.value;

        if (minPrice !== null && minPrice < 0) {
            return { min: true };
        }
        if (maxPrice !== null && maxPrice < 0) {
            return { min: true };
        }
        if (minPrice !== null && maxPrice !== null && minPrice > maxPrice) {
            return { priceRangeMaxInvalid: true };
        }

        return null;
    };
}
export function pageAmountRangeMinValidator(maxControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const minPageAmount = control.value
        const maxPageAmount = formGroup.get(maxControlName)?.value;

        if (minPageAmount !== null && minPageAmount < 0) {
            return { min: true };
        }
        if (maxPageAmount !== null && maxPageAmount < 0) {
            return { min: true };
        }
        if (minPageAmount !== null && maxPageAmount !== null && minPageAmount > maxPageAmount) {
            return { pageAmountRangeMinInvalid: true };
        }

        return null;
    };
}
export function pageAmountRangeMaxValidator(minControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const minPageAmount = formGroup.get(minControlName)?.value;
        const maxPageAmount = control.value;

        if (minPageAmount !== null && minPageAmount < 0) {
            return { min: true };
        }
        if (maxPageAmount !== null && maxPageAmount < 0) {
            return { min: true };
        }
        if (minPageAmount !== null && maxPageAmount !== null && minPageAmount > maxPageAmount) {
            return { pageAmountRangeMaxInvalid: true };
        }

        return null;
    };
}