import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { GenreService } from '../../../..';
import { Genre, ValidationMessage, defaultLibraryFilterRequest } from '../../../../../shared';
import { BaseSelectInputComponent } from "../base-select-input-component/base-select-input-component.component";

@Component({
  selector: 'app-genre-input',
  templateUrl: './genre-input.component.html',
  styleUrl: './genre-input.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GenreInputComponent extends BaseSelectInputComponent<Genre> {

  constructor(
    private readonly genreService: GenreService,
    protected override readonly validateInput: ValidationMessage
  ) {
    super(validateInput);
  }

  getControlName(): string {
    return 'genre';
  }

  fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<Genre[]> {
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: pageIndex,
      pageSize: pageSize,
      containsName: containsName
    };
    return this.genreService.getPaginated(req);
  }

  displayWith(genre?: Genre): string {
    return genre ? `${genre.name}` : '';
  }

  trackByGenre(index: number, genre: Genre): number {
    return genre.id;
  }
}