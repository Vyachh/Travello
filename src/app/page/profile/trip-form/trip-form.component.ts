import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ITrip } from 'src/app/models/Trip';
import { IUserInfo } from 'src/app/models/UserInfo';
import { TripService } from 'src/app/services/trip.service';
import { DatePipe } from '@angular/common';
import { PhotoService } from 'src/app/services/photo.service';
import { FileType } from 'src/app/enum/filetype.enum';
//
@Component({
  selector: 'app-trip-form',
  templateUrl: './trip-form.component.html',
  styleUrls: ['./trip-form.component.css']
})
export class TripFormComponent implements OnInit {

  @Input() userInfo: IUserInfo
  @Input() trip: ITrip
  /**
   *
   */
  constructor(private tripService: TripService, private photoService: PhotoService) {

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
    dateStart: new FormControl<string>('', [
      Validators.required,
    ]),
    dateEnd: new FormControl<string>('', [
      Validators.required,
    ]),
    image: new FormControl<string>('', [
      Validators.required
    ])
  })

  ngOnInit(): void {

  }

  tripImage: File

  onSubmit() {
    this.trip.userId = this.userInfo.id
    this.trip.description = this.tripForm.value.description || ""
    this.trip.title = this.tripForm.value.title || ""
    this.trip.dateFrom = this.tripForm.value.dateStart || ""
    this.trip.dateTo = this.tripForm.value.dateEnd || ""
    this.trip.author = this.userInfo.userName
    this.trip.image = this.tripImage
    console.log(this.tripForm.value);

    const formData = new FormData();
    formData.append('userId', this.trip.userId);
    formData.append('title', this.trip.title);
    formData.append('description', this.trip.description);
    formData.append('dateFrom', this.trip.dateFrom);
    formData.append('dateTo', this.trip.dateTo);
    formData.append('author', this.trip.author);
    formData.append('image', this.tripImage);


    // this.tripService.uploadPhoto(this.tripImage, this.userInfo.id, FileType.TripImage).subscribe();

    this.tripService.addTrip(formData).subscribe({
      next: response => {
      },
      error: e => {
        console.error(e);
      }
    });
  }

  onSavePhoto(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.tripImage = inputElement.files[0];
    }
  }
}
