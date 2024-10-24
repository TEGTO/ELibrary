import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, of, Subject, takeUntil } from 'rxjs';
import { OAUTH_LOGIN_COMMAND_HANDLER, OAuthLoginCommand } from '../..';
import { CommandHandler, RedirectorService } from '../../../shared';

@Component({
  selector: 'app-oauth-callback',
  templateUrl: './oauth-callback.component.html',
  styleUrl: './oauth-callback.component.scss'
})
export class OAuthCallbackComponent implements OnInit, OnDestroy {
  destroy$ = new Subject<void>();

  constructor(
    private readonly route: ActivatedRoute,
    private readonly redirector: RedirectorService,
    @Inject(OAUTH_LOGIN_COMMAND_HANDLER) private readonly oauthLoginHandler: CommandHandler<OAuthLoginCommand>
  ) { }

  ngOnInit(): void {
    this.route.queryParams
      .pipe(
        takeUntil(this.destroy$),
        catchError((error) => {
          console.error('Error:', error);
          this.redirector.redirectToHome();
          return of();
        })
      )
      .subscribe(params => {
        const code = params['code'];
        if (code) {
          const command: OAuthLoginCommand =
          {
            code: code
          };
          this.oauthLoginHandler.dispatch(command);
        }
        this.redirector.redirectToHome();
        return of();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
