import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TableColumn } from '../..';
import { GenericTableComponent } from './generic-table.component';

describe('GenericTableComponent', () => {
  let component: GenericTableComponent;
  let fixture: ComponentFixture<GenericTableComponent>;

  const mockColumns: TableColumn[] = [
    { header: 'Name', field: 'name' },
    { header: 'Age', field: 'age' },
    { header: 'Address', field: 'address' }
  ];

  const mockItems = [
    { name: 'John Doe', age: 30, address: '123 Main St' },
    { name: 'Jane Doe', age: 25, address: '456 Elm St' }
  ];

  const mockTotalItemAmount = 2;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenericTableComponent, MatPaginatorModule, BrowserAnimationsModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenericTableComponent);
    component = fixture.componentInstance;
    component.columns = mockColumns;
    component.items = mockItems;
    component.totalItemAmount = mockTotalItemAmount;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render table with columns and items', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    const headers = compiled.querySelectorAll('th');
    expect(headers[0].textContent?.trim()).toBe('Name');
    expect(headers[1].textContent?.trim()).toBe('Age');
    expect(headers[2].textContent?.trim()).toBe('Address');

    const firstRowCells = compiled.querySelectorAll('tbody tr:nth-child(1) td');
    expect(firstRowCells[0].textContent?.trim()).toBe('John Doe');
    expect(firstRowCells[1].textContent?.trim()).toBe('30');
    expect(firstRowCells[2].textContent?.trim()).toBe('123 Main St');
  });

  it('should emit createItem event when "Create" button is clicked', () => {
    spyOn(component.createItem, 'emit');

    const createButton = fixture.debugElement.query(By.css('thead button'));
    createButton.triggerEventHandler('click', null);

    expect(component.createItem.emit).toHaveBeenCalled();
  });

  it('should emit editItem event when "Edit" button is clicked', () => {
    spyOn(component.editItem, 'emit');

    const editButton = fixture.debugElement.queryAll(By.css('tbody button'))[0];
    editButton.triggerEventHandler('click', mockItems[0]);

    expect(component.editItem.emit).toHaveBeenCalledWith(mockItems[0]);
  });

  it('should emit deleteItem event when "Delete" button is clicked', () => {
    spyOn(component.deleteItem, 'emit');

    const deleteButton = fixture.debugElement.queryAll(By.css('tbody button'))[1];
    deleteButton.triggerEventHandler('click', mockItems[0]);

    expect(component.deleteItem.emit).toHaveBeenCalledWith(mockItems[0]);
  });

  it('should emit pageChange event on page change', fakeAsync(() => {
    spyOn(component.pageChange, 'emit');

    const paginator = fixture.debugElement.query(By.directive(MatPaginator)).componentInstance;
    paginator.page.emit({ pageIndex: 1, pageSize: 10 } as PageEvent);
    tick();

    expect(component.pageChange.emit).toHaveBeenCalledWith({ pageIndex: 2, pageSize: 10 });
  }));
});
