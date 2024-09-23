import { CdkVirtualScrollViewport } from "@angular/cdk/scrolling";
import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { AbstractControl, FormControl, FormGroup, Validators } from "@angular/forms";
import { debounceTime, filter, map, Observable, pairwise, Subject, switchMap, takeUntil, throttleTime } from "rxjs";
import { noSpaces, notEmptyString, ValidationMessage } from "../../../../../shared";

@Component({
    template: ''
})
export abstract class BaseSelectInputComponent<T extends { id: number }> implements OnInit, OnDestroy, AfterViewInit {
    @Input({ required: true }) formGroup!: FormGroup;
    @ViewChild('scroller') scroller!: CdkVirtualScrollViewport;

    readonly itemHeight = 48;
    readonly pageAmount = 12;
    readonly amountItemsInView = 3;

    items: T[] = [];
    pageIndex = 0;
    private fetchedIds = new Set<number>();

    private destroy$ = new Subject<void>();

    get input() { return this.formGroup.get(this.getControlName())! as FormControl; }
    get selectionSize() {
        return this.calculateSelectionSize(this.items.length);
    }

    abstract getControlName(): string;
    abstract fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<T[]>;
    abstract displayWith(item?: T): string;

    constructor(protected readonly validateInput: ValidationMessage) { }

    ngOnInit(): void {
        this.initializeForm();
        this.initValueChange();
        this.loadItems();
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    ngAfterViewInit(): void {
        this.setupScrollListeners(() => this.loadItems());
    }

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    validateInputField(input: AbstractControl<any, any>): { hasError: boolean, message: string } {
        return this.validateInput.getValidationMessage(input);
    }

    private initializeForm(): void {
        const controlName = this.getControlName();
        if (!this.formGroup.contains(controlName)) {
            this.formGroup.addControl(controlName, new FormControl("", [Validators.required, notEmptyString, noSpaces]));
        } else {
            const item = this.formGroup.get(controlName)?.value;
            if (item) {
                this.items = [...this.items, ...this.getUniqueItems([item], this.fetchedIds)];
            }
        }
    }

    private initValueChange() {
        this.input.valueChanges.pipe(
            takeUntil(this.destroy$),
            debounceTime(100),
            switchMap(value => {
                const containsName = typeof value === 'string' ? value : '';
                return this.fetchItems(containsName, 1, this.pageAmount);
            })
        ).subscribe(items => {
            this.fetchedIds = new Set<number>();
            this.pageIndex = 0;
            this.items = this.getUniqueItems(items, this.fetchedIds);
        });
    }

    private loadItems(): void {
        this.pageIndex++;
        const containsName = typeof this.input.value === 'string' ? this.input.value : '';
        this.fetchItems(containsName || '', this.pageIndex, this.pageAmount).pipe(
            takeUntil(this.destroy$)
        ).subscribe(items => {
            this.items = [...this.items, ...this.getUniqueItems(items, this.fetchedIds)];
        });
    }

    private setupScrollListeners(loadMoreCallback: () => void): void {
        this.scroller.elementScrolled().pipe(
            map(() => this.scroller.measureScrollOffset('bottom')),
            pairwise(),
            filter(([previous, current]) => current < previous && current < 2 * this.itemHeight),
            throttleTime(200),
            takeUntil(this.destroy$)
        ).subscribe(() => loadMoreCallback());
    }

    private getUniqueItems(items: T[], fetchedIds: Set<number>): T[] {
        const uniqueItems: T[] = [];
        items.forEach(item => {
            if (!fetchedIds.has(item.id)) {
                fetchedIds.add(item.id);
                uniqueItems.push(item);
            }
        });
        return uniqueItems;
    }

    private calculateSelectionSize(length: number): number {
        return length > this.amountItemsInView
            ? this.amountItemsInView * this.itemHeight
            : length * this.itemHeight + 5;
    }
}
