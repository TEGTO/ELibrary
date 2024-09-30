// import { ComponentFixture, TestBed } from '@angular/core/testing';

// import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
// import { ReactiveFormsModule } from '@angular/forms';
// import { MatNativeDateModule } from '@angular/material/core';
// import { MatDatepickerModule } from '@angular/material/datepicker';
// import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { MatInputModule } from '@angular/material/input';
// import { By } from '@angular/platform-browser';
// import { NoopAnimationsModule } from '@angular/platform-browser/animations';
// import { Author } from '../../../../shared';
// import { AuthorChangeDialogComponent } from './author-change-dialog.component';

// describe('AuthorChangeDialogComponent', () => {
//   let component: AuthorChangeDialogComponent;
//   let fixture: ComponentFixture<AuthorChangeDialogComponent>;
//   let dialogRef: jasmine.SpyObj<MatDialogRef<AuthorChangeDialogComponent>>;

//   const mockAuthor: Author = {
//     id: 1,
//     name: 'John',
//     lastName: 'Doe',
//     dateOfBirth: new Date('2000-01-01')
//   };

//   beforeEach(async () => {

//     const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

//     await TestBed.configureTestingModule({
//       declarations: [AuthorChangeDialogComponent],
//       imports: [
//         ReactiveFormsModule,
//         MatFormFieldModule,
//         MatInputModule,
//         MatDatepickerModule,
//         MatNativeDateModule,
//         NoopAnimationsModule,
//         MatDialogModule
//       ],
//       providers: [
//         { provide: MAT_DIALOG_DATA, useValue: mockAuthor },
//         { provide: MatDialogRef, useValue: dialogRefSpy },
//       ],
//       schemas: [CUSTOM_ELEMENTS_SCHEMA]
//     }).compileComponents();
//   });

//   beforeEach(() => {
//     fixture = TestBed.createComponent(AuthorChangeDialogComponent);
//     component = fixture.componentInstance;
//     dialogRef = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<AuthorChangeDialogComponent>>;
//     fixture.detectChanges();
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });

//   it('should initialize form with dialog data', () => {
//     expect(component.formGroup).toBeDefined();
//     expect(component.nameInput.value).toBe(mockAuthor.name);
//     expect(component.lastNameInput.value).toBe(mockAuthor.lastName);
//     expect(component.dateOfBirthInput.value).toEqual(mockAuthor.dateOfBirth);
//   });

//   it('should validate form fields', () => {
//     component.formGroup.get('name')?.setValue('');
//     component.formGroup.get('lastName')?.setValue('');
//     component.formGroup.get('dateOfBirth')?.setValue(null);

//     expect(component.formGroup.invalid).toBeTrue();

//     component.formGroup.get('name')?.setValue('New Name');
//     component.formGroup.get('lastName')?.setValue('New LastName');
//     component.formGroup.get('dateOfBirth')?.setValue(new Date('1990-01-01'));

//     expect(component.formGroup.valid).toBeTrue();
//   });

//   it('should close dialog with correct data when form is valid', () => {
//     component.formGroup.get('name')?.setValue('Updated Name');
//     component.formGroup.get('lastName')?.setValue('Updated LastName');
//     component.formGroup.get('dateOfBirth')?.setValue(new Date('1990-01-01'));

//     component.sendDetails();

//     expect(dialogRef.close).toHaveBeenCalledWith({
//       id: mockAuthor.id,
//       name: 'Updated Name',
//       lastName: 'Updated LastName',
//       dateOfBirth: new Date('1990-01-01')
//     });
//   });

//   it('should not close dialog if form is invalid', () => {
//     component.formGroup.get('name')?.setValue('');
//     component.formGroup.get('lastName')?.setValue('');
//     component.formGroup.get('dateOfBirth')?.setValue(null);

//     component.sendDetails();

//     expect(dialogRef.close).not.toHaveBeenCalled();
//   });

//   it('should display validation errors when form fields are invalid', () => {
//     component.formGroup.get('name')?.setValue('');
//     fixture.detectChanges();

//     expect(component.formGroup.valid).toBeFalse();
//     expect(component.nameInput.hasError('required')).toBeTruthy();
//   });

//   it('should render the HTML structure correctly', () => {
//     const compiled = fixture.nativeElement as HTMLElement;

//     expect(compiled.querySelector('h2')?.textContent).toContain('Author');

//     expect(compiled.querySelector('mat-form-field')).toBeTruthy();
//     expect(compiled.querySelector('button#send-button')).toBeTruthy();
//   });

//   it('should close dialog with updated author data on form submission', () => {
//     component.formGroup.get('name')?.setValue(mockAuthor.name);

//     component.sendDetails();

//     expect(dialogRef.close).toHaveBeenCalledWith(mockAuthor);
//   });

//   it('should close the dialog when the close button is clicked', () => {
//     const closeButton = fixture.debugElement.query(By.css('.close-button')).nativeElement;
//     closeButton.click();
//     fixture.detectChanges();
//     expect(dialogRef.close).toHaveBeenCalled();
//   });
// });
