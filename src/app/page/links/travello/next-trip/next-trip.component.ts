import { Component, Input, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-next-trip',
  templateUrl: './next-trip.component.html',
  styleUrls: ['./next-trip.component.css']
})
export class NextTripComponent implements OnInit {
  @Input() trip: ITrip

  constructor(public accountService: AccountService) {

  }

  userInfo: IUserInfo

  ngOnInit(): void {
    this.accountService.getInfo().subscribe({
      next: response => {
        this.userInfo = response
      }
    })
  }

}
