import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

export function rangeMinValidator(min: number, maxControlName: string | undefined): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const price = control.value;
        const maxPrice = maxControlName ? formGroup.get(maxControlName)?.value : null;

        if (price !== null && price < min) {
            return { min: true };
        }
        if (price !== null && maxPrice !== null && price > maxPrice) {
            return { rangeMinInvalid: true };
        }

        return null;
    };
}
export function rangeMaxValidator(max: number, minControlName: string | undefined): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const formGroup = control.parent as FormGroup;
        if (!formGroup) return null;

        const minPrice = minControlName ? formGroup.get(minControlName)?.value : null;
        const price = control.value;

        if (price !== null && price > max) {
            return { max: true };
        }
        if (minPrice !== null && price !== null && price < minPrice) {
            return { priceRangeMaxInvalid: true };
        }

        return null;
    };
}