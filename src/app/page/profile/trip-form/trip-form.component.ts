import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ITrip } from 'src/app/models/Trip';
import { IUserInfo } from 'src/app/models/UserInfo';
import { TripService } from 'src/app/services/trip.service';
import { AccountService } from 'src/app/services/account.service';
//
@Component({
  selector: 'app-trip-form',
  templateUrl: './trip-form.component.html',
  styleUrls: ['./trip-form.component.css']
})
export class TripFormComponent implements OnInit {

  constructor(private tripService: TripService, private accountService: AccountService) {

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
    price: new FormControl<string>('', [
      Validators.required,
    ]),
    image: new FormControl<string>('', [
      Validators.required
    ])
  })

  userInfo: IUserInfo

  ngOnInit(): void {

  }

  tripImage: File

  onSubmit() {
    this.accountService.getInfo().subscribe({
      next: response => {
        this.userInfo = response
      }
    });

    const formData = new FormData();
    formData.append('userId', this.userInfo.id);
    formData.append('title', this.tripForm.value.title || "");
    formData.append('description', this.tripForm.value.description || "");
    formData.append('dateFrom', this.tripForm.value.dateStart || "");
    formData.append('dateTo', this.tripForm.value.dateEnd || "");
    formData.append('price', this.tripForm.value.price || "");
    formData.append('author', this.userInfo.userName);
    formData.append('image', this.tripImage);

    this.tripService.addTrip(formData).subscribe({
      next: response => {
        location.reload()
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
