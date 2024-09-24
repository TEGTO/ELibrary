import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { RedirectorService } from '../../../shared';
import { ClientService, ShopCommand, ShopCommandObject, ShopCommandRole, ShopCommandType } from '../../../shop';

@Component({
  selector: 'app-create-client',
  templateUrl: './create-client.component.html',
  styleUrl: './create-client.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateClientComponent implements OnInit, OnDestroy {
  redirectUrl: string | null = null;

  destroy$ = new Subject<void>();

  constructor(
    private readonly clientService: ClientService,
    private readonly route: ActivatedRoute,
    private readonly redirector: RedirectorService,
    private readonly shopCommand: ShopCommand
  ) { }

  ngOnInit(): void {
    this.clientService.getClient()
      .pipe(takeUntil(this.destroy$))
      .subscribe((client) => {
        if (client) {
          if (this.redirectUrl) {
            this.redirector.redirectTo(this.redirectUrl)
          }
          else {
            this.redirector.redirectToHome();
          }
        }
      })
    this.route.queryParams
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        this.redirectUrl = params['redirectTo'];
      });
  }
  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }

  addClient() {
    this.shopCommand.dispatchCommand(ShopCommandObject.Client, ShopCommandType.Add, ShopCommandRole.Client, this);
  }
}
