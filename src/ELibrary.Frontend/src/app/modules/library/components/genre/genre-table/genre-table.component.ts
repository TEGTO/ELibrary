import { Component } from '@angular/core';

@Component({
  selector: 'genre-table',
  templateUrl: './genre-table.component.html',
  styleUrl: './genre-table.component.scss'
})
export class GenreTableComponent {
  items = [
    { name: 'Fiction' },
    { name: 'Non-fiction' },
    { name: 'Sci-fi' },
  ];

  columns = [
    { header: 'Name', field: 'name' }
  ];
}
