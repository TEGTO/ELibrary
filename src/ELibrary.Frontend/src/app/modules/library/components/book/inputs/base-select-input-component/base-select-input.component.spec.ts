/* eslint-disable @typescript-eslint/no-explicit-any */
import { CdkVirtualScrollViewport, ScrollingModule } from "@angular/cdk/scrolling";
import { Component } from "@angular/core";
import { ComponentFixture, fakeAsync, TestBed, tick } from "@angular/core/testing";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatFormFieldModule } from "@angular/material/form-field";
import { Observable, of, Subject } from "rxjs";
import { ValidationMessage } from "../../../../../shared";
import { BaseSelectInputComponent } from "./base-select-input-component.component";

@Component({
    template: `
    <mat-form-field>
    <input type="text" matInput [formControl]="input" [matAutocomplete]="auto">
    <mat-autocomplete #auto="matAutocomplete" [displayWith]="displayWith">
        <cdk-virtual-scroll-viewport #scroller [itemSize]="itemHeight" [style.height.px]="selectionSize">
            <mat-option *cdkVirtualFor="let item of items; trackBy: trackById" [value]="item">
                {{ item.name }}
            </mat-option>
        </cdk-virtual-scroll-viewport>
    </mat-autocomplete>
    <mat-error *ngIf="validateInputField(input).hasError">{{ validateInputField(input).message
        }}</mat-error>
    </mat-form-field>
  `
})
class TestSelectInputComponent extends BaseSelectInputComponent<{ id: number, name: string }> {
    getControlName(): string {
        return 'testControl';
    }

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<{ id: number, name: string }[]> {
        return of([{ id: 1, name: 'Item 1' }, { id: 2, name: 'Item 2' }]);
    }

    displayWith(item?: { id: number, name: string }): string {
        return item ? item.name : '';
    }

    trackById(index: number, item: { id: number, name: string }): number {
        return item.id;
    }
}

describe('BaseSelectInputComponent', () => {
    let component: TestSelectInputComponent;
    let fixture: ComponentFixture<TestSelectInputComponent>;
    let validationMessageService: jasmine.SpyObj<ValidationMessage>;

    beforeEach(async () => {
        validationMessageService = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

        validationMessageService.getValidationMessage.and.returnValue({ hasError: false, message: "" });

        await TestBed.configureTestingModule({
            declarations: [TestSelectInputComponent],
            providers: [
                { provide: ValidationMessage, useValue: validationMessageService }
            ],
            imports: [
                ReactiveFormsModule,
                ScrollingModule,
                MatFormFieldModule,
                MatAutocompleteModule
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(TestSelectInputComponent);
        component = fixture.componentInstance;
        component.formGroup = new FormGroup({});
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form control if it does not exist', () => {
        const formGroup = new FormGroup({});
        component.formGroup = formGroup;
        component.ngOnInit();

        expect(formGroup.contains('testControl')).toBeTrue();
    });

    it('should add initial item to fetched items if formGroup contains value', () => {
        const formGroup = new FormGroup({
            testControl: new FormControl({ id: 3, name: 'Item 3' })
        });
        component.formGroup = formGroup;
        component.ngOnInit();

        expect(component.items.length).toBe(3);
        expect(component.items[0].id).toBe(3);
    });

    it('should call fetchItems when value changes', fakeAsync(() => {
        spyOn(component, 'fetchItems').and.callThrough();
        component.ngOnInit();

        component.input.setValue('new item');
        tick(100);

        expect(component.fetchItems).toHaveBeenCalledWith('new item', 1, component.pageAmount);
    }));

    it('should load items and append them to the existing ones', fakeAsync(() => {
        component.ngOnInit();

        const mockItems = [{ id: 3, name: 'Item 3' }, { id: 4, name: 'Item 4' }];
        spyOn(component, 'fetchItems').and.returnValue(of(mockItems));

        component["loadItems"]();
        tick();

        expect(component.items.length).toBeGreaterThan(0);
        expect(component.items[2].id).toBe(3);
        expect(component.items[3].id).toBe(4);
    }));

    it('should calculate selection size correctly', () => {
        component.items = [{ id: 1, name: 'Item 1' }, { id: 2, name: 'Item 2' }];
        const selectionSize = component.selectionSize;

        expect(selectionSize).toEqual(component.items.length > component.amountItemsInView
            ? component.amountItemsInView * component.itemHeight
            : component.items.length * component.itemHeight + 5);
    });

    it('should set up scroll listeners for loading more items', fakeAsync(() => {
        const scrollerElementScrolled$ = new Subject<Event>();

        component.scroller = {
            elementScrolled: () => scrollerElementScrolled$.asObservable(),
            measureScrollOffset: () => 100 // Adjusted value to simulate a scroll offset
        } as unknown as CdkVirtualScrollViewport;

        component.ngOnInit();

        spyOn(component as any, 'loadItems').and.callThrough();

        component.ngAfterViewInit();

        // Simulate two scroll events to trigger pairwise and filter
        scrollerElementScrolled$.next(new Event('scroll')); // First scroll event
        component.scroller.measureScrollOffset = () => 50; // Simulate the scroll position change
        scrollerElementScrolled$.next(new Event('scroll')); // Second scroll event

        tick(200);

        expect(component['loadItems']).toHaveBeenCalled();
    }));

    it('should validate input field', () => {
        const control = new FormControl();
        validationMessageService.getValidationMessage.and.returnValue({
            hasError: true,
            message: 'Error message'
        });

        const validation = component.validateInputField(control);

        expect(validationMessageService.getValidationMessage).toHaveBeenCalledWith(control);
        expect(validation.hasError).toBeTrue();
        expect(validation.message).toBe('Error message');
    });

    it('should mark destroy$ as complete on ngOnDestroy', () => {
        const destroySpy = spyOn(component['destroy$'], 'complete').and.callThrough();
        component.ngOnDestroy();

        expect(destroySpy).toHaveBeenCalled();
    });
});
