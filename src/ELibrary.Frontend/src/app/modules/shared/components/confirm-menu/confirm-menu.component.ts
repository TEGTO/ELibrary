import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-menu',
  standalone: true,
  imports: [MatDialogModule, CommonModule],
  templateUrl: './confirm-menu.component.html',
  styleUrl: './confirm-menu.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmMenuComponent {

}
