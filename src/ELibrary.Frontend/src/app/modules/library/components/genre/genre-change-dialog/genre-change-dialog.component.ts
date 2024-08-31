import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GenreResponse } from '../../../../shared';

@Component({
  selector: 'genre-change-dialog',
  templateUrl: './genre-change-dialog.component.html',
  styleUrl: './genre-change-dialog.component.scss'
})
export class GenreChangeDialogComponent {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }

  constructor(@Inject(MAT_DIALOG_DATA) public genre: GenreResponse, private dialogRef: MatDialogRef<GenreChangeDialogComponent>) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.genre.name, [Validators.required, Validators.maxLength(256)]),
      });
  }

  sendDetails() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const genre: GenreResponse = {
        id: this.genre.id,
        name: formValues.name,
      };
      this.dialogRef.close(genre);
    }
  }
}
