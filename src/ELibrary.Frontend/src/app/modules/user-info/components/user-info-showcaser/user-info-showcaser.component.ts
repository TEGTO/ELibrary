import { ChangeDetectionStrategy, Component } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserInfoService } from '../..';
import { UserInfo } from '../../../shared';

@Component({
  selector: 'user-info-showcaser',
  templateUrl: './user-info-showcaser.component.html',
  styleUrl: './user-info-showcaser.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserInfoShowcaserComponent {
  userInfo$!: Observable<UserInfo>;

  constructor(private readonly infoService: UserInfoService) { }

  ngOnInit(): void {
    this.userInfo$ = this.infoService.getUserInfo().pipe(
      map(resp => {
        return resp.userInfo;
      })
    );
  }
}