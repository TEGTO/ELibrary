import { FormControl } from "@angular/forms";
import { minDateValidator } from "./minDateValidator";

describe('minDateValidator', () => {

    it('should return null for a valid date in the past', () => {
        const control = new FormControl('1990-01-01');
        const validator = minDateValidator();
        const result = validator(control);
        expect(result).toBeNull();
    });

    it('should return an error object for a date in the future', () => {
        const futureDate = new Date();
        futureDate.setDate(futureDate.getDate() + 1);
        const control = new FormControl(futureDate.toISOString().split('T')[0]);
        const validator = minDateValidator();
        const result = validator(control);
        expect(result).toEqual({ invalidDateOfBirth: 'Date must be in the past' });
    });

    it('should return null for a valid date format and a date in the past', () => {
        const control = new FormControl('2000-12-31');
        const validator = minDateValidator();
        const result = validator(control);
        expect(result).toBeNull();
    });

});