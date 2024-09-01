import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AuthorResponse, minDateValidator } from '../../../../shared';

@Component({
  selector: 'author-change-dialog',
  templateUrl: './author-change-dialog.component.html',
  styleUrl: './author-change-dialog.component.scss'
})
export class AuthorChangeDialogComponent {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }
  get lastNameInput() { return this.formGroup.get('lastName')!; }
  get dateOfBirthInput() { return this.formGroup.get('dateOfBirth')!; }

  constructor(@Inject(MAT_DIALOG_DATA) public author: AuthorResponse, private dialogRef: MatDialogRef<AuthorChangeDialogComponent>) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.author.name, [Validators.required, Validators.maxLength(256)]),
        lastName: new FormControl(this.author.lastName, [Validators.required, Validators.maxLength(256)]),
        dateOfBirth: new FormControl(this.author.dateOfBirth, [Validators.required, minDateValidator()]),
      });
  }

  sendDetails() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const author: AuthorResponse = {
        id: this.author.id,
        name: formValues.name,
        lastName: formValues.lastName,
        dateOfBirth: formValues.dateOfBirth,
      };
      this.dialogRef.close(author);
    }
  }
}
