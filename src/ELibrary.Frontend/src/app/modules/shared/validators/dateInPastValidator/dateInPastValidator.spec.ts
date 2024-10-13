import { FormControl } from "@angular/forms";
import { dateInPastValidator } from "./dateInPastValidator";

describe('dateInPastValidator', () => {

    it('should return null for a valid date in the past', () => {
        const control = new FormControl('1990-01-01');
        const validator = dateInPastValidator();
        const result = validator(control);
        expect(result).toBeNull();
    });

    it('should return an error object for a date in the future', () => {
        const futureDate = new Date();
        futureDate.setDate(futureDate.getDate() + 1);
        const control = new FormControl(futureDate.toISOString().split('T')[0]);
        const validator = dateInPastValidator();
        const result = validator(control);
        expect(result).toEqual({ dateInPast: 'Date must be in the past' });
    });

    it('should return null for a valid date format and a date in the past', () => {
        const control = new FormControl('2000-12-31');
        const validator = dateInPastValidator();
        const result = validator(control);
        expect(result).toBeNull();
    });

});