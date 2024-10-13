import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { SnackbarManager } from '../../../shared';

@Directive({
  selector: '[appInputMaxStock]',
})
export class InputMaxStockDirective {
  @Input() min!: number;
  @Input() max!: number;
  @Input() duration = 6;

  constructor(
    private readonly elementRef: ElementRef,
    private readonly snackbarManager: SnackbarManager
  ) { }

  @HostListener('input', ['$event.target.value'])
  onInput(value: string) {
    let numericValue = parseInt(value, 10);

    if (isNaN(numericValue)) {
      numericValue = this.min;
    }

    if (numericValue < this.min) {
      numericValue = this.min;
    } else if (numericValue > this.max) {
      numericValue = this.max;
      this.snackbarManager.openInfoSnackbar(
        `Input can't be greater than stock amount! (Current stock amount: ${this.max})`,
        this.duration
      );
    }

    this.elementRef.nativeElement.value = numericValue;
  }
}
