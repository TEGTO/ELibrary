import { ChangeDetectionStrategy, Component } from '@angular/core';
import { pathes } from '../../../shared';

@Component({
  selector: 'app-manager-table',
  templateUrl: './manager-table.component.html',
  styleUrl: './manager-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ManagerTableComponent {

  get bookStockPath() { return pathes.manager_bookstock; }
  get bookPath() { return pathes.manager_books; }
  get authorPath() { return pathes.manager_authors; }
  get genrePath() { return pathes.manager_genres; }
  get publisherPath() { return pathes.manager_publishers; }
  get orderPath() { return pathes.manager_orders; }
  get statisticsPath() { return pathes.manager_statistics; }
}
