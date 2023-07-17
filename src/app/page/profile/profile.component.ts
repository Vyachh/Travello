import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { IUserInfo } from 'src/app/models/userInfo';
import { IUser } from 'src/app/models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService,
  ) {
    this.isLoggedIn = accountService.isLoggedIn;
  }
  isLoggedIn: boolean;
  isInTrip: boolean;

  password: string;
  user: IUser = {
    userName: '',
    password: ''
  }
  userInfo: IUserInfo = {
    id: '',
    userName: '',
    currentTripId: 0,
    tripList: []
  }

  ngOnInit(): void {
    this.accountService
      .getInfo()
      .subscribe(
        {
          next: response => (
            this.userInfo = response
          ),
          error: e => {
            console.error(e);
          }
        }
      );
    if (this.userInfo.currentTripId > 0) {
      this.isInTrip = true
    }
  }

  onChangePasswordClick() {
    this.user.userName = this.userInfo.userName
    this.user.password = this.password
    this.accountService
      .сhangePassword(this.user)
      .subscribe(
        {
          next: response => {
            console.log(response);
            localStorage.setItem('bearer', response)
            location.reload();
          }, error: e => {
            console.error(e);
          }
        }
      );
  }
}
