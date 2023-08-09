import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-trip-edit',
  templateUrl: './trip-edit.component.html',
  styleUrls: ['./trip-edit.component.css']
})
export class TripEditComponent {


  tripImage: File


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

  constructor(public accountService: AccountService, public tripService: TripService) {

  }

  onSubmit() {
    const formData = new FormData();
    formData.append('id', this.tripService.selectedTrip.toString());
    formData.append('userId', this.accountService.userInfo.id);
    formData.append('title', this.tripForm.value.title || "");
    formData.append('description', this.tripForm.value.description || "");
    formData.append('dateFrom', this.tripForm.value.dateStart || "");
    formData.append('dateTo', this.tripForm.value.dateEnd || "");
    formData.append('price', this.tripForm.value.price || "");
    formData.append('author', this.accountService.userInfo.userName);
    formData.append('image', this.tripImage);

    this.tripService.updateTrip(formData).subscribe({
      next: response => {
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
