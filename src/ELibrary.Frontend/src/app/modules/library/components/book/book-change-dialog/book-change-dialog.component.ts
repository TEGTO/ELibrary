import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BookResponse, dateOfBirthValidator as minDateValidator } from '../../../../shared';

@Component({
  selector: 'book-change-dialog',
  templateUrl: './book-change-dialog.component.html',
  styleUrl: './book-change-dialog.component.scss'
})
export class BookChangeDialogComponent {
  formGroup!: FormGroup;

  get titleInput() { return this.formGroup.get('title')!; }
  get publicationDateInput() { return this.formGroup.get('publicationDate')!; }

  constructor(@Inject(MAT_DIALOG_DATA) public book: BookResponse, private dialogRef: MatDialogRef<BookChangeDialogComponent>) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        title: new FormControl(this.book.title, [Validators.required, Validators.maxLength(256)]),
        publicationDate: new FormControl(this.book.publicationDate, [Validators.required, minDateValidator()]),
      });
  }

  sendDetails() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const author: BookResponse = {
        id: this.book.id,
        title: formValues.title,
        publicationDate: formValues.publicationDate,
        author: {
          id: 0,
          name: "",
          lastName: "",
          dateOfBirth: new Date()
        },
        genre: {
          id: 0,
          name: ""
        }
      };
      this.dialogRef.close(author);
    }
  }
}
