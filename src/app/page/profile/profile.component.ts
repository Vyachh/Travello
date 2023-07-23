import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { IUserInfo } from 'src/app/models/userInfo';
import { IUser } from 'src/app/models/user';
import { ITrip } from 'src/app/models/trip';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TripService } from 'src/app/services/trip.service';
import { PhotoService } from 'src/app/services/photo.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService,
    private tripService: TripService,
    private photoService: PhotoService
  ) {
    this.isLoggedIn = accountService.isLoggedIn;
  }
  isLoggedIn = false;
  isInTrip = false;
  isEdit = true;

  date = "01.01.2000";

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
    role: 'user',
    email: '',
    birthdate: '',
    image: ''
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
    if (this.isLoggedIn) {
      this.accountService.getInfo()
        .subscribe({
          next: userInfo => {
            this.userInfo = userInfo
          },
          error: error => {
            console.error(error);
          }
        })

    }

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

  onUploadPhoto(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      const file = inputElement.files[0];
      this.photoService.uploadPhoto(file, this.userInfo.id).subscribe({
        next: response => {
          // Обработка ответа от сервера после загрузки фото
          console.log('Фото успешно загружено:', response);
        },
        error: error => {
          console.error('Ошибка при загрузке фото:', error);
        }
      }
      );

    }
  }

  onLogout() {
    localStorage.clear();
    location.reload();
  }
}
