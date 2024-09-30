// /* eslint-disable @typescript-eslint/no-explicit-any */
// import { CdkVirtualScrollViewport } from "@angular/cdk/scrolling";
// import { Component } from "@angular/core";
// import { ComponentFixture, fakeAsync, TestBed, tick } from "@angular/core/testing";
// import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
// import { Observable, of } from "rxjs";
// import { ValidationMessage } from "../../../../../shared";
// import { BaseSelectInputComponent } from "./base-select-input-component.component";

// @Component({
//     template: ''
// })
// class TestSelectInputComponent extends BaseSelectInputComponent<{ id: number, name: string }> {
//     getControlName(): string {
//         return 'testControl';
//     }

//     // eslint-disable-next-line @typescript-eslint/no-unused-vars
//     fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<{ id: number, name: string }[]> {
//         return of([{ id: 1, name: 'Item 1' }, { id: 2, name: 'Item 2' }]);
//     }

//     displayWith(item?: { id: number, name: string }): string {
//         return item ? item.name : '';
//     }
// }


// describe('BaseSelectInputComponent', () => {
//     let component: TestSelectInputComponent;
//     let fixture: ComponentFixture<TestSelectInputComponent>;
//     let validationMessageService: jasmine.SpyObj<ValidationMessage>;

//     beforeEach(async () => {
//         validationMessageService = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

//         await TestBed.configureTestingModule({
//             declarations: [TestSelectInputComponent],
//             providers: [
//                 { provide: ValidationMessage, useValue: validationMessageService }
//             ],
//             imports: [ReactiveFormsModule]
//         }).compileComponents();
//     });

//     beforeEach(() => {
//         fixture = TestBed.createComponent(TestSelectInputComponent);
//         component = fixture.componentInstance;
//         fixture.detectChanges(); // Trigger initial data binding
//     });

//     it('should create the component', () => {
//         expect(component).toBeTruthy();
//     });

//     it('should initialize form control if it does not exist', () => {
//         const formGroup = new FormGroup({});
//         component.formGroup = formGroup;
//         component.ngOnInit();

//         expect(formGroup.contains('testControl')).toBeTrue();
//     });

//     it('should add initial item to fetched items if formGroup contains value', () => {
//         const formGroup = new FormGroup({
//             testControl: new FormControl({ id: 1, name: 'Item 1' })
//         });
//         component.formGroup = formGroup;
//         component.ngOnInit();

//         expect(component.items.length).toBe(1);
//         expect(component.items[0].id).toBe(1);
//     });

//     it('should call fetchItems when value changes', fakeAsync(() => {
//         spyOn(component, 'fetchItems').and.callThrough();
//         component.ngOnInit();

//         component.input.setValue('new item');
//         tick(100); // Advance debounceTime
//         fixture.detectChanges();

//         expect(component.fetchItems).toHaveBeenCalledWith('new item', 1, component.pageAmount);
//     }));

//     it('should load items and append them to the existing ones', fakeAsync(() => {
//         const mockItems = [{ id: 3, name: 'Item 3' }, { id: 4, name: 'Item 4' }];
//         spyOn(component, 'fetchItems').and.returnValue(of(mockItems));

//         component["loadItems"]();
//         tick();
//         fixture.detectChanges();

//         expect(component.items.length).toBeGreaterThan(0);
//         expect(component.items[2].id).toBe(3);
//         expect(component.items[3].id).toBe(4);
//     }));

//     it('should calculate selection size correctly', () => {
//         component.items = [{ id: 1, name: 'Item 1' }, { id: 2, name: 'Item 2' }];
//         const selectionSize = component.selectionSize;

//         expect(selectionSize).toEqual(component.amountItemsInView * component.itemHeight);
//     });

//     it('should set up scroll listeners for loading more items', () => {
//         spyOn<any>(component, "loadItems");
//         const scroller = {
//             elementScrolled: () => of(0),
//             measureScrollOffset: () => 0
//         } as unknown as CdkVirtualScrollViewport;

//         component.scroller = scroller;
//         component.ngAfterViewInit();

//         expect(component["loadItems"]()).toHaveBeenCalled();
//     });

//     it('should validate input field', () => {
//         const control = new FormControl();
//         validationMessageService.getValidationMessage.and.returnValue({
//             hasError: true,
//             message: 'Error message'
//         });

//         const validation = component.validateInputField(control);

//         expect(validationMessageService.getValidationMessage).toHaveBeenCalledWith(control);
//         expect(validation.hasError).toBeTrue();
//         expect(validation.message).toBe('Error message');
//     });

//     it('should mark destroy$ as complete on ngOnDestroy', () => {
//         const destroySpy = spyOn(component['destroy$'], 'complete').and.callThrough();
//         component.ngOnDestroy();

//         expect(destroySpy).toHaveBeenCalled();
//     });
// });
