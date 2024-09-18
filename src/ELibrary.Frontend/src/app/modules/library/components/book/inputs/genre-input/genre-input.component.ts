import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { GenreService } from '../../../..';
import { GenreResponse, ValidationMessage, defaultLibraryFilterRequest } from '../../../../../shared';
import { BaseSelectInputComponent } from "../base-select-input-component/base-select-input-component.component";

@Component({
  selector: 'app-genre-input',
  templateUrl: './genre-input.component.html',
  styleUrl: './genre-input.component.scss'
})
export class GenreInputComponent extends BaseSelectInputComponent<GenreResponse> {

  constructor(
    private readonly genreService: GenreService,
    protected override readonly validateInput: ValidationMessage
  ) {
    super(validateInput);
  }

  getControlName(): string {
    return 'genre';
  }

  fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<GenreResponse[]> {
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: pageIndex,
      pageSize: pageSize,
      containsName: containsName
    };
    return this.genreService.getPaginated(req);
  }

  displayWith(genre?: GenreResponse): string {
    return genre ? `${genre.name}` : '';
  }

  trackByGenre(index: number, genre: GenreResponse): number {
    return genre.id;
  }
}