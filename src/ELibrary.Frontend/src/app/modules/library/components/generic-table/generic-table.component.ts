import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, PipeTransform, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { TableColumn } from '../..';

@Component({
  selector: 'generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GenericTableComponent {
  @Input({ required: true }) items!: any[];
  @Input({ required: true }) columns!: TableColumn[];
  @Input({ required: true }) totalItemAmount!: number;

  @Output() editItem = new EventEmitter<any>();
  @Output() deleteItem = new EventEmitter<any>();
  @Output() createItem = new EventEmitter<void>();
  @Output() pageChange = new EventEmitter<{ pageIndex: number, pageSize: number }>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  pageSize: number = 10;

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