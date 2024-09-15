import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AuthenticationService } from '../../../authentication';
import { Policy, PolicyType } from '../../identity/policy';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(
    private readonly authService: AuthenticationService,
    private router: Router
  ) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {

    const expectedPolicies = route.data['policy'] as PolicyType[];

    return this.authService.getAuthData().pipe(
      map((data) => {
        let hasRights = true;
        expectedPolicies.forEach(policy => {
          hasRights = Policy.checkPolicy(policy, data.roles);
        });
        if (!hasRights) {
          this.router.navigate(['']);
          return false;
        }
        return true;
      })
    );
  }
}