import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-client-view',
  templateUrl: './client-view.component.html',
  styleUrl: './client-view.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClientViewComponent {

}
