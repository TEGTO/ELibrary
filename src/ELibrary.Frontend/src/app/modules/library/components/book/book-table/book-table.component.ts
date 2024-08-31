import { Component } from '@angular/core';

@Component({
  selector: 'book-table',
  templateUrl: './book-table.component.html',
  styleUrl: './book-table.component.scss'
})
export class BookTableComponent {
  items = [
    { title: 'Book A', date: '2021-11-25', author: 'Author A', genre: 'Fiction' },
    { title: 'Book B', date: '2021-10-22', author: 'Author B', genre: 'Non-fiction' },
  ];

  columns = [
    { header: 'Title', field: 'title' },
    { header: 'Publication Date', field: 'date' },
    { header: 'Author', field: 'author' },
    { header: 'Genre', field: 'genre' }
  ];
}