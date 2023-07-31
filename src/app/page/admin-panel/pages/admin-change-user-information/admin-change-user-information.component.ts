import { Component } from '@angular/core';
import { Role } from 'src/app/enum/role.enum';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-admin-change-user-information',
  templateUrl: './admin-change-user-information.component.html',
  styleUrls: ['./admin-change-user-information.component.css']
})
export class AdminChangeUserInformationComponent {
  isRolesDropdown: boolean = false

  userInfos: IUserInfo[]
  role: string = 'User'

  constructor(private accountService: AccountService) {
    accountService.getAll().subscribe({
      next: response => {
        this.userInfos = response.result
      }
    })
  }

  selectUser(user: IUserInfo) {

  }

  selectRole(id: number) {
    this.role = Role[id]
    }
}
