import { Injectable, OnDestroy } from '@angular/core';
import { Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService, OAuthLoginCommand } from '../..';
import { CommandHandler, LoginOAuthRequest, RedirectorService } from '../../../shared';

@Injectable({
  providedIn: 'root'
})
export class OAuthLoginCommandHandlerService extends CommandHandler<OAuthLoginCommand> implements OnDestroy {
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

  dispatch(command: OAuthLoginCommand): void {

    this.authService.getOAuthParams().pipe(
      takeUntil(this.destroy$),
      tap(params => {
        if (params) {
          const req: LoginOAuthRequest = {
            code: command.code,
            codeVerifier: params.codeVerifier,
            redirectUrl: params.redirectUrl,
            oAuthLoginProvider: params.oAuthLoginProvider
          };

          this.authService.oauthSignIn(req);
        }
        this.redirector.redirectToHome();
        this.cleanUp();
      }),
    ).subscribe();
  }
}