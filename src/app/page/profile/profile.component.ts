import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { IUserInfo } from 'src/app/models/UserInfo';
import { IUser } from 'src/app/models/User';
import { ITrip } from 'src/app/models/Trip';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TripService } from 'src/app/services/trip.service';
import { PhotoService } from 'src/app/services/photo.service';
import { FileType } from 'src/app/enum/filetype.enum';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private accountService: AccountService,
    private photoService: PhotoService
  ) {
    this.isLoggedIn = accountService.isLoggedIn;
  }
  
  isLoggedIn = false;
  isInTrip = false;

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
    dateFrom: '',
    dateTo: '',
    author: '',
    image: null,
    isOngoingTrip: false,
    isNextTrip: false,
    imageUrl: ''
  }



  ngOnInit(): void {
    if (this.isLoggedIn) {
      this.accountService.getInfo()
        .subscribe({
          next: userInfo => {
            this.userInfo = userInfo,
            console.log(this.userInfo);
            
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

  

  onUploadPhoto(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      const file = inputElement.files[0];
      this.photoService.uploadPhoto(file, this.userInfo.id, FileType.AvatarImage).subscribe({
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


}
