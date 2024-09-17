/* eslint-disable @typescript-eslint/no-explicit-any */
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, PipeTransform, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { TableColumn } from '../..';

@Component({
  selector: 'app-generic-table',
  standalone: true,
  imports: [MatPaginator, CommonModule],
  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GenericTableComponent implements OnInit {
  @Input({ required: true }) items!: any[];
  @Input({ required: true }) columns!: TableColumn[];
  @Input({ required: true }) totalItemAmount!: number;
  @Input() pageSizeOptions: number[] = [5, 10, 20];
  @Input() initialPageSize = 10;

  @Output() editItem = new EventEmitter<any>();
  @Output() deleteItem = new EventEmitter<any>();
  @Output() createItem = new EventEmitter<void>();
  @Output() pageChange = new EventEmitter<{ pageIndex: number, pageSize: number }>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  pageSize!: number;
  private defaultPagination = { pageIndex: 0, pageSize: this.initialPageSize };

  ngOnInit(): void {
    this.pageSize = this.initialPageSize;
  }

  onPageChange(event: PageEvent): void {
    const pageIndex = event.pageIndex + 1;
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
  resetPagination() {
    if (this.paginator) {
      this.paginator.pageIndex = this.defaultPagination.pageIndex;
      this.pageSize = this.defaultPagination.pageSize;
    }
  }
}