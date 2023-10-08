import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ITrip } from 'src/app/models/ITrip';
import { ITripCount } from 'src/app/models/TripCount';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-add-participants',
  templateUrl: './add-participants.component.html',
  styleUrls: ['./add-participants.component.css'],
})
export class AddParticipantsComponent implements OnInit {
  userInfos: IUserInfo[];
  trips: ITrip[] = [];
  defaultTrip: ITrip[] = [];

  selectedTrip: ITrip;

  selectedUsers: IUserInfo[] = [];

  usersInTrip: IUserInfo[] = [];

  onGoingCount: any;

  searchTrip: string;
  searchUser: string;

  constructor(
    public accountService: AccountService,
    public tripService: TripService,
    private notifier: ToastrService
  ) {}

  ngOnInit(): void {
    this.accountService.getAll().subscribe({
      next: (response) => {
        this.userInfos = response.result;
        this.getTripList();
      },
      error: (error) => {
        this.notifier.error('', 'Users Information has not loaded.');
      },
    });
  }

  private getTripList() {
    this.tripService.getTripList().subscribe({
      next: (response: ITrip[]) => {
        let tripArray = response;
        if (this.accountService.userInfo.role == 'Organizer') {
          tripArray = response.filter(
            (t) =>
              t.isApproved == true &&
              t.userId == this.accountService.userInfo.id
          );
        } else {
          tripArray = response.filter((t) => t.isApproved == true);
        }
        this.accountService.getOngoingCount().subscribe({
          next: (response: any) => {
            response.forEach((element: any) => {
              let trip = tripArray.find((t) => t.id == element.tripId);
              if (trip) {
                if (trip.id == element.tripId) {
                  trip.onGoingCount = element.count;
                  this.trips.push(trip);
                }
              }
            });
          },
        });
      },
    });
  }

  selectTrip(trip: ITrip) {
    trip.isSelected = true;
    this.selectedTrip = trip;
    this.selectedUsers = [];

    this.usersInTrip = this.getUsersInTrips(trip.id);

    this.trips.forEach((element) => {
      if (element !== trip) {
        element.isSelected = false;
      }
    });
    this.userInfos.forEach((element) => {
      element.isSelected = false;
    });
  }

  getUsersInTrips(index: number) {
    return this.userInfos.filter((u) => u.currentTripId === index);
  }

  selectUserTrip(user: IUserInfo) {
    if (
      this.selectedTrip &&
      !this.selectedUsers.find((s) => s.id === user.id)
    ) {
      user.currentTripId = this.selectedTrip.id;
      this.selectedUsers.push(user);
      user.isSelected = true;
    } else {
      const index = this.selectedUsers.findIndex((s) => s.id === user.id);
      if (index !== -1) {
        this.selectedUsers.splice(index, 1);
      }
      user.isSelected = false;
    }
  }

  addUsersToTrip() {
    this.accountService.addUsersToTrip(this.selectedUsers).subscribe({
      next: (response) => {
        location.reload();
      },
      error: (error) => {
        this.notifier.error('', `Users not added to trip`);
      },
    });
  }

  getOngoingCount() {
    this.accountService.getOngoingCount().subscribe({
      next: (response: any) => {
        return response;
      },
    });
  }

  onSearchTripChange() {
    if (this.searchTrip == '') {
      this.trips = [];
      this.getTripList();
    }
    this.trips = this.trips.filter((t) => t.title.includes(this.searchTrip));
  }
}
