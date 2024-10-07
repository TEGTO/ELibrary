import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { CommandHandler, RedirectorService } from '../../../shared';
import { ADD_CLIENT_COMMAND_HANDLER, AddClientCommand, ClientService } from '../../../shop';

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
    @Inject(ADD_CLIENT_COMMAND_HANDLER) private readonly addClientHandler: CommandHandler<AddClientCommand>
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
    const command: AddClientCommand = {}
    this.addClientHandler.dispatch(command);
  }
}
