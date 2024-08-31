import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, PipeTransform, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, tap } from 'rxjs';
import { TableColumn } from '../..';

@Component({
  selector: 'generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GenericTableComponent {
  @Input({ required: true }) items$!: Observable<any[]>;
  @Input({ required: true }) columns!: TableColumn[];

  @Output() editItem = new EventEmitter<any>();
  @Output() deleteItem = new EventEmitter<any>();
  @Output() createItem = new EventEmitter<void>();
  @Output() pageChange = new EventEmitter<{ pageIndex: number, pageSize: number }>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  emittedItems$!: Observable<any[]>;
  pageSize: number = 10;
  paginatedItems: any[] = [];

  ngOnChanges(): void {
    if (this.items$) {
      this.emittedItems$ = this.items$.pipe(
        tap(items => { this.updatePaginatedItems(items) })
      );
    }
  }

  updatePaginatedItems(items: any[]) {
    const startIndex = 0;
    const endIndex = this.pageSize;
    this.paginatedItems = items.slice(startIndex, endIndex);
  }

  onPageChange(event: PageEvent): void {
    let pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.pageChange.emit({ pageIndex: pageIndex, pageSize: this.pageSize });
  }

  onEdit(item: any): void {
    this.editItem.emit(item);
  }

  onDelete(item: any): void {
    this.deleteItem.emit(item);
  }

  onCreate(): void {
    this.createItem.emit();
  }

  applyPipe(value: any, pipe?: PipeTransform, pipeArgs?: any[]): any {
    return pipe ? pipe.transform(value, ...(pipeArgs || [])) : value;
  }
}