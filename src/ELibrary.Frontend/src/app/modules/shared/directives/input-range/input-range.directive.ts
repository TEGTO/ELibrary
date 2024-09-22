import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[appInputRange]'
})
export class InputRangeDirective {
  @Input() min!: number;
  @Input() max!: number;

  constructor(private elementRef: ElementRef) { }

  @HostListener('input', ['$event.target.value'])
  onInput(value: string) {
    let numericValue = parseInt(value, 10);

    if (numericValue < this.min) {
      numericValue = this.min;
    } else if (numericValue > this.max) {
      numericValue = this.max;
    }

    this.elementRef.nativeElement.value = numericValue;
  }
}
