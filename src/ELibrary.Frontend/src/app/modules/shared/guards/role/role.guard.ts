/* eslint-disable @typescript-eslint/no-unused-vars */
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Observable, map } from 'rxjs';
import { Policy, PolicyType, RedirectorService } from '../..';
import { AuthenticationService } from '../../../authentication';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(
    private readonly authService: AuthenticationService,
    private readonly redirector: RedirectorService
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {

    const expectedPolicies = route.data['policy'] as PolicyType[];

    return this.authService.getUserAuth().pipe(
      map((data) => {
        let hasRights = true;
        expectedPolicies.forEach(policy => {
          hasRights = Policy.checkPolicy(policy, data.roles);
        });
        if (!hasRights) {
          this.redirector.redirectTo('');
          return false;
        }
        return true;
      })
    );
  }
}