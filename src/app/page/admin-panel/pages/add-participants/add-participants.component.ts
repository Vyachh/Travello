import { Component, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-add-participants',
  templateUrl: './add-participants.component.html',
  styleUrls: ['./add-participants.component.css'],
})
export class AddParticipantsComponent implements OnInit {

  userInfos: IUserInfo[]
  trips: ITrip[]

  selectedTrip: ITrip

  selectedUsers: IUserInfo[] = [];

  usersInTrip: IUserInfo[] = [];

  constructor(public accountService: AccountService, public tripService: TripService) {

  }

  ngOnInit(): void {
    this.accountService.getAll().subscribe({
      next: response => {
        this.userInfos = response.result
      }
    })
    this.tripService.getTripList().subscribe({
      next: (response: any) => {
        this.trips = response
      }
    })
  }


  selectTrip(trip: ITrip) {
    trip.isSelected = true;
    this.selectedTrip = trip;
    this.selectedUsers = [];

    this.usersInTrip = this.getUsersInTrips(trip.id)
    this.trips.forEach(element => {
      if (element !== trip) {
        element.isSelected = false
      }
    });
    this.userInfos.forEach(element => {
      element.isSelected = false
    });
  }

  getUsersInTrips(index: number) {
    return this.userInfos.filter(u => u.currentTripId === index)
  }

  selectUserTrip(user: IUserInfo) {
    if (this.selectedTrip && !this.selectedUsers.find(s => s.id === user.id)) {
      user.currentTripId = this.selectedTrip.id
      this.selectedUsers.push(user);
      user.isSelected = true
    } else {
      const index = this.selectedUsers.findIndex(s => s.id === user.id);
      if (index !== -1) {
        this.selectedUsers.splice(index, 1)
      }
      user.isSelected = false
    }
  }

  addUsersToTrip() {
    this.accountService.addUsersToTrip(this.selectedUsers).subscribe({
      next: response => {
        location.reload();

      }
    })
  }
}