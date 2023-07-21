import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { IUserInfo } from 'src/app/models/userInfo';
import { IUser } from 'src/app/models/user';
import { ITrip } from 'src/app/models/trip';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService,
    private tripService: TripService
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
    tripList: [],
    role: 'user'
  }
  trip: ITrip = {
    userId: '',
    title: '',
    description: '',
    travelTime: 0,
    author: '',
    image: ''
  }

  tripForm = new FormGroup({
    userId: new FormControl<string>('', [
      Validators.required
    ]),
    title: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(6)
    ]),
    description: new FormControl<string>('', [
      Validators.required,
    ]),
    travelTime: new FormControl<number>(0, [
      Validators.required,
      Validators.minLength(2),
    ]),
    image: new FormControl<string>('', [
      Validators.required
    ])
  })

  ngOnInit(): void {
    this.accountService.getInfo().subscribe({
      next: userInfo => {
        this.userInfo = userInfo
      },
      error: error => {
        console.error(error);
      }
    })
    if (this.userInfo.currentTripId > 0) {
      this.isInTrip = true
    }
  }

  onSubmit() {
    this.trip.userId = this.userInfo.id
    this.trip.description = this.tripForm.value.description || ""
    this.trip.title = this.tripForm.value.title || ""
    this.trip.travelTime = this.tripForm.value.travelTime || 0
    this.trip.author = this.userInfo.userName
    this.trip.image = this.tripForm.value.image || ""
    this.tripService.addTrip(this.trip).subscribe({
      next: response => {
        console.log(response);
      },
      error: e => {
        console.error(e);
      }
    });

  }

  onChangePasswordClick() {
    this.user.userName = this.userInfo.userName
    this.user.password = this.password
    this.accountService
      .сhangePassword(this.user)
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

  onLogout() {
    localStorage.clear();
    location.reload();
  }
}
