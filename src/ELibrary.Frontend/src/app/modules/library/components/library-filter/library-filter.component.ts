import { ChangeDetectionStrategy, Component, EventEmitter, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { LibraryFilterRequest, defaultLibraryFilterRequest } from '../../../shared';

@Component({
  selector: 'app-library-filter',
  templateUrl: './library-filter.component.html',
  styleUrl: './library-filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LibraryFilterComponent {
  @Output() filterChange = new EventEmitter<LibraryFilterRequest>();

  inputControl = new FormControl('');

  onInputChange(): void {
    const req: LibraryFilterRequest = {
      ...defaultLibraryFilterRequest(),
      pageNumber: 0,
      pageSize: 0,
      containsName: this.inputControl.value ?? ""
    };
    this.filterChange.emit(req);
  }
}
