import { Component, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-add-participants',
  templateUrl: './add-participants.component.html',
  styleUrls: ['./add-participants.component.css']
})
export class AddParticipantsComponent implements OnInit {

  userInfos: IUserInfo[]
  trips: ITrip[]

  constructor(public accountService: AccountService, public tripService: TripService) {

  }

  ngOnInit(): void {
    this.accountService.getAll().subscribe({
      next: response => {
        console.log(response);
      }
    })
    this.tripService.getTripList().subscribe({
      next: response => {
        console.log(response);
      }
    })
  }


}
