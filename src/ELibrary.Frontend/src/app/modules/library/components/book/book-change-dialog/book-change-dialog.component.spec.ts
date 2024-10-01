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
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Book, CoverType, getDefaultAuthor, getDefaultBook, getDefaultGenre, getDefaultPublisher, ValidationMessage } from '../../../../shared';
import { BookChangeDialogComponent } from './book-change-dialog.component';

describe('BookChangeDialogComponent', () => {
    let component: BookChangeDialogComponent;
    let fixture: ComponentFixture<BookChangeDialogComponent>;
    let mockDialogRef: jasmine.SpyObj<MatDialogRef<BookChangeDialogComponent>>;
    let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;
    const mockBook: Book = getDefaultBook();

    beforeEach(async () => {
        mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);
        mockValidationMessage = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);

        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: "" });

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
                NoopAnimationsModule,
                MatRadioModule,
            ],
            providers: [
                { provide: MAT_DIALOG_DATA, useValue: mockBook },
                { provide: MatDialogRef, useValue: mockDialogRef },
                { provide: ValidationMessage, useValue: mockValidationMessage },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

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

        setValidData();

        expect(component.formGroup.valid).toBeTrue();
    });

    it('should call close method with book data when form is valid', () => {
        setValidData();

        component.sendDetails();

        expect(mockDialogRef.close).toHaveBeenCalledWith(jasmine.objectContaining({
            id: jasmine.any(Number),
        }));
    });

    it('should not call close method when form is invalid', () => {
        component.nameInput.setValue('');
        component.sendDetails();

        expect(mockDialogRef.close).not.toHaveBeenCalled();
    });

    it('should mark all fields as touched when form is submitted', () => {
        const markAllAsTouchedSpy = spyOn(component.formGroup, 'markAllAsTouched');
        component.sendDetails();

        expect(markAllAsTouchedSpy).toHaveBeenCalled();
    });

    it('should display validation messages for invalid fields', () => {
        component.nameInput.setValue('');
        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: true, message: "required" });
        fixture.detectChanges();

        const nameErrorElement = fixture.debugElement.query(By.css('mat-error')).nativeElement;
        expect(nameErrorElement.textContent).toContain('required');
    });

    it('should initialize form with default values if no book is passed', () => {
        component = new BookChangeDialogComponent(null as any, mockDialogRef, {} as any);
        component.ngOnInit();

        expect(component.publicationDateInput.value).not.toBe(null);
        expect(component.coverTypeInput.value).toBe(CoverType.Hard);
        expect(component.stockAmountInput.value).toBe(0);
    });

    it('should render the HTML structure correctly', () => {
        const compiled = fixture.nativeElement as HTMLElement;

        expect(compiled.querySelector('h2')?.textContent).toContain('Book');
        expect(compiled.querySelector('mat-form-field')).toBeTruthy();
        expect(compiled.querySelector('button#send-button')).toBeTruthy();
    });

    function setValidData() {
        component.nameInput.setValue('Updated Book');
        component.publicationDateInput.setValue(new Date('2021-01-01'));
        component.priceInput.setValue(200);
        component.coverTypeInput.setValue(CoverType.Hard);
        component.pageAmountInput.setValue(300);
        component.stockAmountInput.setValue(0);
        component.coverImgUrlInput.setValue("smt");
        component.formGroup.get('author')!.setValue(getDefaultAuthor());
        component.formGroup.get('genre')!.setValue(getDefaultGenre());
        component.formGroup.get('publisher')!.setValue(getDefaultPublisher());
    }
});
