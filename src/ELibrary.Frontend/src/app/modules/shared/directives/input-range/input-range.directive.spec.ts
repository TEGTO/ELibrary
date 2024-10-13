import { Component, ElementRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { InputMaxStockDirective } from '../../../shop';
import { InputRangeDirective } from './input-range.directive';

@Component({
  template: `<input appInputRange [min]="min" [max]="max" />`,
})
class TestComponent {
  min = 1;
  max = 10;
}

describe('InputRangeDirective', () => {
  let elementRef: ElementRef<HTMLInputElement>;
  let directive: InputRangeDirective;


  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InputMaxStockDirective, TestComponent],
      imports: [FormsModule],
      providers: [
        { provide: ElementRef, useValue: new ElementRef(document.createElement('input')) },
      ],
    }).compileComponents();

    const fixture = TestBed.createComponent(TestComponent);
    elementRef = TestBed.inject(ElementRef);
    directive = new InputRangeDirective(elementRef);

    fixture.detectChanges();
  });

  it('should create an instance', () => {
    const directive = new InputRangeDirective(elementRef);
    expect(directive).toBeTruthy();
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
  });
});
