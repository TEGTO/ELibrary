import { Component, ElementRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { SnackbarManager } from '../../../shared';
import { InputMaxStockDirective } from './input-max-stock.directive';

@Component({
    template: `<input appInputMaxStock [min]="min" [max]="max" />`,
})
class TestComponent {
    min = 1;
    max = 10;
}

describe('InputMaxStockDirective', () => {
    let elementRef: ElementRef<HTMLInputElement>;
    let snackbarManager: SnackbarManager;
    let directive: InputMaxStockDirective;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [InputMaxStockDirective, TestComponent],
            imports: [FormsModule],
            providers: [
                { provide: ElementRef, useValue: new ElementRef(document.createElement('input')) },
                {
                    provide: SnackbarManager,
                    useValue: {
                        openInfoSnackbar: jasmine.createSpy('openInfoSnackbar'),
                    },
                },
            ],
        }).compileComponents();

        const fixture = TestBed.createComponent(TestComponent);
        elementRef = TestBed.inject(ElementRef);
        snackbarManager = TestBed.inject(SnackbarManager);
        directive = new InputMaxStockDirective(elementRef, snackbarManager);

        fixture.detectChanges();
    });

    it('should set value to min if input is less than min', () => {
        directive.min = 1;
        directive.max = 10;

        directive.onInput('0');

        expect(elementRef.nativeElement.value).toBe("1");
    });

    it('should set value to max if input is greater than max', () => {
        directive.min = 1;
        directive.max = 10;

        directive.onInput('15');

        expect(elementRef.nativeElement.value).toBe("10");
        expect(snackbarManager.openInfoSnackbar).toHaveBeenCalledWith(
            `Input can't be greater than stock amount! (Current stock amount: 10)`,
            directive.duration
        );
    });

    it('should set value to min if input is NaN', () => {
        directive.min = 1;
        directive.max = 10;

        directive.onInput('abc');

        expect(elementRef.nativeElement.value).toBe("1");
    });

    it('should accept valid input within range', () => {
        directive.min = 1;
        directive.max = 10;

        directive.onInput('5');

        expect(elementRef.nativeElement.value).toBe("5");
        expect(snackbarManager.openInfoSnackbar).not.toHaveBeenCalled();
    });
});
