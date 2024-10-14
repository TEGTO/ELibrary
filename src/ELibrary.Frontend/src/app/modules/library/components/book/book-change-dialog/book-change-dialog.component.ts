import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Book, CoverType, getDefaultBook, noSpaces, notEmptyString, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-book-change-dialog',
  templateUrl: './book-change-dialog.component.html',
  styleUrl: './book-change-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookChangeDialogComponent implements OnInit {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')! as FormControl; }
  get publicationDateInput() { return this.formGroup.get('publicationDate')! as FormControl; }
  get priceInput() { return this.formGroup.get('price')! as FormControl; }
  get coverTypeInput() { return this.formGroup.get('coverType')! as FormControl; }
  get pageAmountInput() { return this.formGroup.get('pageAmount')! as FormControl; }
  get stockAmountInput() { return this.formGroup.get('stockAmount')! as FormControl; }
  get coverImgUrlInput() { return this.formGroup.get('coverImgUrl')! as FormControl; }

  constructor(
    @Inject(MAT_DIALOG_DATA) private readonly book: Book | null,
    private readonly dialogRef: MatDialogRef<BookChangeDialogComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  initializeForm(): void {
    this.formGroup = new FormGroup({
      name: new FormControl(this.book?.name, [Validators.required, notEmptyString, Validators.maxLength(256)]),
      publicationDate: new FormControl(this.book?.publicationDate ?? new Date(), [Validators.required]),
      price: new FormControl(this.book?.price, [Validators.required, Validators.min(0)]),
      coverType: new FormControl(this.book?.coverType ?? CoverType.Hard, [Validators.required]),
      pageAmount: new FormControl(this.book?.pageAmount, [Validators.required, Validators.min(1)]),
      stockAmount: (new FormControl(this.book?.stockAmount ?? 0, [Validators.required, Validators.min(0)])),
      coverImgUrl: new FormControl(this.book?.coverImgUrl, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(1024)]),
      author: new FormControl(this.book?.author, [Validators.required]),
      genre: new FormControl(this.book?.genre, [Validators.required]),
      publisher: new FormControl(this.book?.publisher, [Validators.required]),
    });

    this.stockAmountInput.disable();

  }

  sendDetails(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };

      const book: Book = {
        id: this.book ? this.book.id : getDefaultBook().id,
        name: formValues.name,
        publicationDate: formValues.publicationDate,
        price: formValues.price,
        coverType: formValues.coverType,
        pageAmount: formValues.pageAmount,
        stockAmount: formValues.stockAmount,
        coverImgUrl: formValues.coverImgUrl,
        author: formValues.author,
        genre: formValues.genre,
        publisher: formValues.genre,
      };

      this.dialogRef.close(book);
    }
    this.formGroup.markAllAsTouched();
  }
}