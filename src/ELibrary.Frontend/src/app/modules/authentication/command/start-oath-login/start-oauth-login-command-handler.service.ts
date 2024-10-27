import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService, StartOAuthLoginCommand } from '../..';
import { CommandHandler, getFullOAuthRedirectPath, GetOAuthUrlQueryParams, RedirectorService } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class StartOAuthLoginCommandHandlerService extends CommandHandler<StartOAuthLoginCommand> implements OnDestroy {
  private readonly destroy$ = new Subject<void>();

  constructor(
    private readonly authService: AuthenticationService,
    private readonly redirector: RedirectorService,
  ) {
    super();
  }

  ngOnDestroy(): void {
    this.cleanUp();
  }

  cleanUp() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  dispatch(command: StartOAuthLoginCommand): void {
    const codeVerifier = this.generateCodeVerifier();

    const req: GetOAuthUrlQueryParams = {
      codeVerifier: codeVerifier,
      redirectUrl: getFullOAuthRedirectPath(),
      oAuthLoginProvider: command.oAuthLoginProvider
    };

    this.authService.setOAuthParams(req);

    this.authService.getOAuthUrl(req).pipe(
      takeUntil(this.destroy$),
      tap(response => {
        this.redirector.redirectToExternalUrl(response.url);
        this.cleanUp();
      }),
    ).subscribe();
  }

  private generateCodeVerifier(): string {
    return crypto.randomUUID();
  }
}