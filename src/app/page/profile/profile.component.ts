import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { IUserInfo } from 'src/app/models/UserInfo';
import { IUser } from 'src/app/models/User';
import { ITrip } from 'src/app/models/Trip';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TripService } from 'src/app/services/trip.service';
import { PhotoService } from 'src/app/services/photo.service';
import { FileType } from 'src/app/enum/filetype.enum';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  constructor(
    private accountService: AccountService,
    private photoService: PhotoService,
    private tripService: TripService,
    private notifier: ToastrService,
    private router:Router
  ) {
    this.isLoggedIn = accountService.isLoggedIn;
  }

  isLoggedIn = false;
  isInTrip = false;

  userInfo: IUserInfo;
  trip: ITrip;

  ngOnInit(): void {
    if (this.isLoggedIn) {
      this.accountService.getInfo().subscribe({
        next: (userInfo) => {
          this.userInfo = userInfo;
          this.accountService.getCurrentTripId(userInfo.id).subscribe({
            next: (response: number) => {
              this.userInfo.currentTripId = response;              
              if (this.userInfo.currentTripId > 0) {
                this.isInTrip = true;
                this.tripService
                  .getById(this.userInfo.currentTripId)
                  .subscribe({
                    next: (response: ITrip) => {
                      this.trip = response;
                    },
                    error: (error) => {
                      this.notifier.error(
                        `User's Trip info has not been loaded.`
                      );
                    },
                  });
              }
            },
            error:error =>{
              this.notifier.error(`User's Trip info has not been loaded.`)
            }
          });
        },
        error: (error) => {
          console.error(error);
          this.notifier.error('User info has not been loaded.');
        },
      });
    }
  }

  onUploadPhoto(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      const file = inputElement.files[0];
      this.photoService
        .uploadPhoto(file, this.userInfo.id, FileType.AvatarImage)
        .subscribe({
          next: (response) => {
            location.reload();
          },
          error: (error) => {
            console.error('Ошибка при загрузке фото:', error);
          },
        });
    }
  }

  navigateToDetails(id: number) {
    this.router.navigate(['trip', id]);
  }

}
