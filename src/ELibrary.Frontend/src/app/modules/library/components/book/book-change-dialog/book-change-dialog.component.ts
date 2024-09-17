import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { BookResponse, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-book-change-dialog',
  templateUrl: './book-change-dialog.component.html',
  styleUrl: './book-change-dialog.component.scss'
})
export class BookChangeDialogComponent implements OnInit, OnDestroy {
  formGroup!: FormGroup;

  private destroy$ = new Subject<void>();

  get nameInput() { return this.formGroup.get('name')!; }
  get publicationDateInput() { return this.formGroup.get('publicationDate')!; }
  get priceInput() { return this.formGroup.get('price')!; }
  get coverTypeInput() { return this.formGroup.get('coverType')!; }
  get pageAmountInput() { return this.formGroup.get('pageAmount')!; }
  get stockAmountInput() { return this.formGroup.get('stockAmount')!; }

  constructor(
    @Inject(MAT_DIALOG_DATA) private readonly book: BookResponse,
    private readonly dialogRef: MatDialogRef<BookChangeDialogComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  initializeForm(): void {
    this.formGroup = new FormGroup({
      name: new FormControl(this.book.name, [Validators.required, Validators.maxLength(256)]),
      publicationDate: new FormControl(this.book.publicationDate, [Validators.required]),
      price: new FormControl(this.book.price, [Validators.required, Validators.min(0)]),
      coverType: new FormControl(this.book.coverType, [Validators.required]),
      pageAmount: new FormControl(this.book.pageAmount, [Validators.required, Validators.min(1)]),
      stockAmount: new FormControl(this.book.stockAmount, [Validators.required, Validators.min(0)]),
    });
  }

  sendDetails(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };

      const updatedBook: BookResponse = {
        id: this.book.id,
        name: formValues.name,
        publicationDate: formValues.publicationDate,
        price: formValues.price,
        coverType: formValues.coverType,
        pageAmount: formValues.pageAmount,
        stockAmount: formValues.stockAmount,
        author: formValues.author,
        genre: formValues.genre,
        publisher: formValues.genre,
      };

      this.dialogRef.close(updatedBook);
    }
  }
}