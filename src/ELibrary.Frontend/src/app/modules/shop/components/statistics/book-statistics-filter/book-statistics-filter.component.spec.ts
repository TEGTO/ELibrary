import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CurrencyPipeApplier, getDefaultBook, ValidationMessage } from '../../../../shared';
import { BookStatisticsFilterComponent } from './book-statistics-filter.component';

describe('BookStatisticsFilterComponent', () => {
  let component: BookStatisticsFilterComponent;
  let fixture: ComponentFixture<BookStatisticsFilterComponent>;
  let mockValidationMessageService: jasmine.SpyObj<ValidationMessage>;
  let mockCurrencyPipeApplier: jasmine.SpyObj<CurrencyPipeApplier>;

  beforeEach(async () => {
    mockValidationMessageService = jasmine.createSpyObj<ValidationMessage>('ValidationMessage', ['getValidationMessage']);
    mockCurrencyPipeApplier = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);

    mockValidationMessageService.getValidationMessage.and.returnValue({ hasError: false, message: "" });

    await TestBed.configureTestingModule({
      declarations: [BookStatisticsFilterComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        BrowserAnimationsModule,
        NgxMatTimepickerModule,
        NgxMatNativeDateModule,
        NgxMatDatetimePickerModule,
      ],
      providers: [
        { provide: ValidationMessage, useValue: mockValidationMessageService },
        { provide: CurrencyPipeApplier, useValue: mockCurrencyPipeApplier }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookStatisticsFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form group and controls on init', () => {
    component.ngOnInit();
    expect(component.formGroup).toBeTruthy();
    expect(component.formGroup.get('fromUTC')).toBeTruthy();
    expect(component.formGroup.get('toUTC')).toBeTruthy();
    expect(component.formGroup.get('book')).toBeTruthy();
  });

  it('should emit filterChange event when form is valid and changes', () => {
    spyOn(component.filterChange, 'emit');

    component.ngOnInit();

    const values = {
      fromUTC: new Date(0, 0, 0, 0),
      toUTC: new Date(1, 1, 1, 1),
      book: getDefaultBook()
    };

    component.formGroup.setValue(values);

    component.onFilterChange();

    expect(component.formGroup.valid).toBeTrue();
    expect(component.filterChange.emit).toHaveBeenCalled();
  });

  it('should add a book to the includeBooks array when a valid book is selected', () => {
    const book = getDefaultBook();
    component.ngOnInit();

    component.bookFormControl.setValue(book);
    expect(component.includeBooks.length).toBe(1);
    expect(component.includeBooks[0].id).toBe(book.id);
  });

  it('should not add duplicate books to the includeBooks array', () => {
    const book = getDefaultBook();
    component.ngOnInit();

    component.bookFormControl.setValue(book);  // First add
    component.bookFormControl.setValue(book);  // Second add (same book)

    expect(component.includeBooks.length).toBe(1);  // No duplicate book
  });

  it('should remove a book from the includeBooks array when deleteBook is called', () => {
    const book = getDefaultBook();
    component.ngOnInit();
    component.includeBooks = [book];

    component.deleteBook(book);
    expect(component.includeBooks.length).toBe(0);
  });

  it('should validate date range fields correctly', () => {
    component.ngOnInit();

    const fromUTCControl = component.formGroup.get('fromUTC') as FormControl;
    const toUTCControl = component.formGroup.get('toUTC') as FormControl;

    fromUTCControl.setValue(new Date('2024-01-01'));
    toUTCControl.setValue(new Date('2023-12-31'));  // Invalid: To date is before From date

    expect(component.formGroup.valid).toBeFalse();  // Invalid form due to date range issue
  });

  it('should call currency applier for book price formatting', () => {
    const book = getDefaultBook();
    component.applyCurrencyPipe(book.price);
    expect(mockCurrencyPipeApplier.applyCurrencyPipe).toHaveBeenCalledWith(book.price);
  });

  it('should return validation message when validateInputField is called', () => {
    const control = new FormControl();
    mockValidationMessageService.getValidationMessage.and.returnValue({
      hasError: true,
      message: 'Invalid date range'
    });

    const result = component.validateInputField(control);
    expect(result.hasError).toBeTrue();
    expect(result.message).toBe('Invalid date range');
  });

  it('should return the correct item size for the selection', () => {
    const size = component.calculateSelectionSize();
    expect(size).toBe(component.scollSize);
  });

});