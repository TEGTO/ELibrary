import { LOCALE_ID } from "@angular/core";
import { TestBed } from "@angular/core/testing";
import { LocalizedDatePipe } from "./localizedDatePipe";

describe('LocalizedDatePipe', () => {
    let pipe: LocalizedDatePipe;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                LocalizedDatePipe,
                { provide: LOCALE_ID, useValue: 'en-US' }
            ]
        });

        pipe = TestBed.inject(LocalizedDatePipe);
    });

    it('should return null for null or undefined input', () => {
        expect(pipe.transform(null)).toBeNull();
        expect(pipe.transform(undefined)).toBeNull();
    });

    it('should return null for an invalid date string', () => {
        expect(pipe.transform('invalid-date')).toBeNull();
    });

    it('should correctly transform a Date object', () => {
        const date = new Date('2024-09-01');
        expect(pipe.transform(date)).toBe('09/01/2024');
    });

    it('should correctly transform a date string', () => {
        const dateString = '2024-09-01';
        expect(pipe.transform(dateString)).toBe('09/01/2024');
    });

    it('should correctly transform a timestamp number', () => {
        const timestamp = 1735766400000;
        expect(pipe.transform(timestamp)).toBe('01/01/2025');
    });

    it('should return null for a non-parsable number', () => {
        expect(pipe.transform(NaN)).toBeNull();
    });

    it('should correctly transform a date with a different locale', () => {
        TestBed.resetTestingModule();
        TestBed.configureTestingModule({
            providers: [
                LocalizedDatePipe,
                { provide: LOCALE_ID, useValue: 'de-DE' }
            ]
        });
        const pipeWithLocale = TestBed.inject(LocalizedDatePipe);

        const date = new Date('2024-09-01');
        expect(pipeWithLocale.transform(date)).toBe('01.09.2024');
    });
});