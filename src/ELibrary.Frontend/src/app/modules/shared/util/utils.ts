import { FormGroup } from "@angular/forms";

export function combineDateTime(date: Date, time: Date): Date {
    const hours = time.getHours();
    const minutes = time.getMinutes();
    const newDateTime = new Date(date);
    newDateTime.setHours(hours, minutes, 0, 0);
    return newDateTime;
}

export function getDateOrNull(date: Date | null, time: Date | null): Date | null {
    if (date !== null) {
        const t = time ?? new Date(0, 0, 0, 0);
        return combineDateTime(date, t);
    }
    return null;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function withPlaceholder(value: any, placeholder = '—'): string {
    return value === null || value === undefined || value === '' ? placeholder : value.toString();
}

export function getFormValidationErrors(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {
        const controlErrors = formGroup.get(key)!.errors;
        if (controlErrors != null) {
            Object.keys(controlErrors).forEach(keyError => {
                console.log('Key control: ' + key + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
            });
        }
    });
}