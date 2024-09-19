import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Genre } from '../../../../shared';
import { GenreChangeDialogComponent } from './genre-change-dialog.component';

describe('GenreChangeDialogComponent', () => {
  let component: GenreChangeDialogComponent;
  let fixture: ComponentFixture<GenreChangeDialogComponent>;
  let dialogRef: jasmine.SpyObj<MatDialogRef<GenreChangeDialogComponent>>;
  const mockGenre: Genre = { id: 1, name: 'Horror' };

  beforeEach(async () => {
    const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [GenreChangeDialogComponent],
      imports: [
        MatDialogModule,
        ReactiveFormsModule,
        FormsModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: mockGenre },
        { provide: MatDialogRef, useValue: dialogRefSpy }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenreChangeDialogComponent);
    component = fixture.componentInstance;
    dialogRef = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<GenreChangeDialogComponent>>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with provided genre data', () => {
    expect(component.formGroup).toBeTruthy();
    expect(component.formGroup.get('name')?.value).toBe(mockGenre.name);
  });

  it('should render form with name input field', () => {
    const nameInput = fixture.debugElement.query(By.css('input[formControlName="name"]'));
    expect(nameInput).toBeTruthy();
    expect(nameInput.nativeElement.value).toBe(mockGenre.name);
  });

  it('should display validation error message when name is empty', () => {
    component.formGroup.get('name')?.setValue('');
    fixture.detectChanges();

    const errorMessage = fixture.debugElement.query(By.css('mat-error'));
    expect(errorMessage).toBeTruthy();
    expect(errorMessage.nativeElement.textContent).toContain('Name is required.');
  });

  it('should close dialog with updated genre data on form submission', () => {
    const updatedGenre: Genre = { id: 1, name: 'Thriller' };
    component.formGroup.get('name')?.setValue(updatedGenre.name);

    component.sendDetails();

    expect(dialogRef.close).toHaveBeenCalledWith(updatedGenre);
  });

  it('should close dialog without data if form is invalid and submit button is clicked', () => {
    component.formGroup.get('name')?.setValue('');

    component.sendDetails();

    expect(dialogRef.close).not.toHaveBeenCalled();
  });

  it('should close the dialog when the close button is clicked', () => {
    const closeButton = fixture.debugElement.query(By.css('.close-button')).nativeElement;
    closeButton.click();
    fixture.detectChanges();
    expect(dialogRef.close).toHaveBeenCalled();
  });

  it('should have correct labels and buttons', () => {
    const titleElement = fixture.debugElement.query(By.css('h2[mat-dialog-title]'));
    expect(titleElement.nativeElement.textContent).toBe('Genre');

    const confirmButton = fixture.debugElement.query(By.css('button#send-button'));
    expect(confirmButton.nativeElement.textContent).toBe('Confirm');
  });
});