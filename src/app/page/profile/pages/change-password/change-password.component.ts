import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IUser } from 'src/app/models/User';
import { AccountService } from 'src/app/services/account.service';
import { ProfileComponent } from '../../profile.component';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {

  constructor(private accountService: AccountService) {

  }

  @Input() UserName: string;
  userName: string
  password: string;

  onChangePasswordClick(password: string) {

    let user: IUser = {
      userName: '',
      password: ''
    }

    user.userName = this.accountService.userInfo.userName
    user.password = password
    this.accountService
      .сhangePassword(user)
      .subscribe(
        {
          next: response => {
            localStorage.setItem('bearer', response)
            location.reload();
          }, error: e => {
            console.error(e);
          }
        }
      );
  }
}
