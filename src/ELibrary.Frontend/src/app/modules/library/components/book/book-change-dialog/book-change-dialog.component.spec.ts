/* eslint-disable @typescript-eslint/no-explicit-any */
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Book } from '../../../../shared';
import { BookChangeDialogComponent } from './book-change-dialog.component';

describe('BookChangeDialogComponent', () => {
  let component: BookChangeDialogComponent;
  let fixture: ComponentFixture<BookChangeDialogComponent>;
  let dialogRef: jasmine.SpyObj<MatDialogRef<BookChangeDialogComponent>>;
  const mockBook: Book = {
    id: 1,
    name: 'Mock Book',
    publicationDate: new Date('2020-01-01'),
    price: 100,
    coverType: 1,
    pageAmount: 250,
    stockAmount: 50,
    coverImgUrl: 'http://example.com/image.jpg',
    author: { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1970-01-01') },
    genre: { id: 1, name: 'Fiction' },
    publisher: { id: 1, name: 'Publisher 1' }
  };

  beforeEach(async () => {
    const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);

    await TestBed.configureTestingModule({
      declarations: [BookChangeDialogComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatDialogModule,
        MatSelectModule,
        MatOptionModule,
        ScrollingModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: mockBook },
        { provide: MatDialogRef, useValue: dialogRefSpy },
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    dialogRef = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<BookChangeDialogComponent>>;
    fixture = TestBed.createComponent(BookChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with book data', () => {
    expect(component.formGroup).toBeDefined();
    expect(component.nameInput.value).toBe(mockBook.name);
    expect(component.publicationDateInput.value).toEqual(mockBook.publicationDate);
    expect(component.priceInput.value).toBe(mockBook.price);
    expect(component.pageAmountInput.value).toBe(mockBook.pageAmount);
    expect(component.coverImgUrlInput.value).toBe(mockBook.coverImgUrl);
    expect(component.stockAmountInput.value).toBe(mockBook.stockAmount);
    expect(component.coverTypeInput.value).toBe(mockBook.coverType);
  });

  it('should validate required fields', () => {
    component.nameInput.setValue('');
    component.publicationDateInput.setValue(null);
    component.priceInput.setValue(null);
    component.pageAmountInput.setValue(null);

    expect(component.formGroup.invalid).toBeTrue();

    component.nameInput.setValue('Updated Book');
    component.publicationDateInput.setValue(new Date('2021-01-01'));
    component.priceInput.setValue(200);
    component.pageAmountInput.setValue(300);

    expect(component.formGroup.valid).toBeTrue();
  });

  it('should call close method with book data when form is valid', () => {
    component.sendDetails();

    expect(dialogRef.close).toHaveBeenCalledWith({
      id: mockBook.id,
      name: mockBook.name,
      publicationDate: mockBook.publicationDate,
      price: mockBook.price,
      coverType: mockBook.coverType,
      pageAmount: mockBook.pageAmount,
      stockAmount: mockBook.stockAmount,
      coverImgUrl: mockBook.coverImgUrl,
      author: mockBook.author,
      genre: mockBook.genre,
      publisher: mockBook.publisher,
    });
  });

  it('should not call close method when form is invalid', () => {
    component.nameInput.setValue('');
    component.sendDetails();

    expect(dialogRef.close).not.toHaveBeenCalled();
  });

  it('should mark all fields as touched when form is submitted', () => {
    const markAllAsTouchedSpy = spyOn(component.formGroup, 'markAllAsTouched');
    component.sendDetails();

    expect(markAllAsTouchedSpy).toHaveBeenCalled();
  });

  it('should display validation messages for invalid fields', () => {
    component.nameInput.setValue('');
    fixture.detectChanges();

    const nameErrorElement = fixture.debugElement.query(By.css('mat-error')).nativeElement;
    expect(nameErrorElement.textContent).toContain('required');
  });

  it('should initialize form with default values if no book is passed', () => {
    component = new BookChangeDialogComponent(null as any, dialogRef, {} as any);
    component.ngOnInit();

    expect(component.nameInput.value).toBe('');
    expect(component.priceInput.value).toBe(0);
    expect(component.stockAmountInput.value).toBe(0);
  });

  it('should render the HTML structure correctly', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('h2')?.textContent).toContain('Book');
    expect(compiled.querySelector('mat-form-field')).toBeTruthy();
    expect(compiled.querySelector('button#send-button')).toBeTruthy();
  });
});
