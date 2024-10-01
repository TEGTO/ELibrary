import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { LibraryFilterComponent } from './library-filter.component';

describe('LibraryFilterComponent', () => {
    let component: LibraryFilterComponent;
    let fixture: ComponentFixture<LibraryFilterComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [LibraryFilterComponent],
            imports: [ReactiveFormsModule],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(LibraryFilterComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize with an empty inputControl', () => {
        expect(component.inputControl.value).toBe('');
    });

    it('should emit filterChange event on input change', () => {
        spyOn(component.filterChange, 'emit');

        const inputElement = fixture.debugElement.query(By.css('input')).nativeElement;
        inputElement.value = 'New Filter';
        inputElement.dispatchEvent(new Event('input'));

        fixture.detectChanges();

        expect(component.filterChange.emit).toHaveBeenCalledWith({
            containsName: 'New Filter',
            pageNumber: 0,
            pageSize: 0
        });
    });

    it('should emit default filter request when input is empty', () => {
        spyOn(component.filterChange, 'emit');

        component.inputControl.setValue('');
        component.onInputChange();

        expect(component.filterChange.emit).toHaveBeenCalledWith({
            containsName: '',
            pageNumber: 0,
            pageSize: 0
        });
    });
});