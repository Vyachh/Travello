import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ITrip } from 'src/app/models/Trip';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-admin-trip-form',
  templateUrl: './admin-trip-form.component.html',
  styleUrls: ['./admin-trip-form.component.css']
})
export class AdminTripFormComponent {
  constructor(private tripService: TripService, public accountService: AccountService,
    private router: Router
  ) {

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


  ngOnInit(): void {

  }

  tripImage: File

  onSubmit() {

    const formData = new FormData();
    formData.append('userId', this.accountService.userInfo.id);
    formData.append('title', this.tripForm.value.title || "");
    formData.append('description', this.tripForm.value.description || "");
    formData.append('dateFrom', this.tripForm.value.dateStart || "");
    formData.append('dateTo', this.tripForm.value.dateEnd || "");
    formData.append('price', this.tripForm.value.price || "");
    formData.append('author', this.accountService.userInfo.userName);
    formData.append('image', this.tripImage);

    this.tripService.addTrip(formData).subscribe({
      next: response => {
        this.router.navigate(['/adminpanel'])
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