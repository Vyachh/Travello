import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ITrip } from 'src/app/models/Trip';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-admin-trip-form',
  templateUrl: './admin-trip-form.component.html',
  styleUrls: ['./admin-trip-form.component.css'],
})
export class AdminTripFormComponent {
  constructor(
    private tripService: TripService,
    public accountService: AccountService,
    private router: Router,
    private notifier: ToastrService
  ) {}

  tripForm = new FormGroup({
    userId: new FormControl<string | null>('', [Validators.required]),
    title: new FormControl<string | null>('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    description: new FormControl<string | null>('', [Validators.required]),
    dateStart: new FormControl<Date | null>(null, [Validators.required]),
    dateEnd: new FormControl<Date | null>(null, [Validators.required]),
    price: new FormControl<string | null>('', [Validators.required]),
    image: new FormControl<string | null>('', [Validators.required]),
  });

  ngOnInit(): void {}

  tripImage: File;

  onSubmit() {

    const dateStart: Date = this.tripForm.value.dateStart || new Date();
    const dateEnd: Date = this.tripForm.value.dateEnd || new Date();
    let isDateValid: boolean = this.dateValidate(dateStart, dateEnd);

    if (!isDateValid) {
      this.notifier.error('Date is not valid!');
      return;
    }

    if (this.tripForm.status === 'VALID') {
      const formData = new FormData();
      formData.append('userId', this.accountService.userInfo.id);
      formData.append('title', this.tripForm.value.title || '');
      formData.append('description', this.tripForm.value.description || '');
      formData.append('dateFrom', dateStart?.toString() || '');
      formData.append('dateTo', dateEnd?.toString() || '');
      formData.append('price', this.tripForm.value.price || '');
      formData.append('author', this.accountService.userInfo.userName);
      formData.append('image', this.tripImage);

      this.tripService.addTrip(formData).subscribe({
        next: () => {
          this.router.navigate(['/adminpanel']);
        },
      });
    }
    else{
      console.error(this.tripForm);
      this.notifier.error('Please enter all information!')
    }
  }

  onSavePhoto(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files.length > 0) {
      this.tripImage = inputElement.files[0];
    }
  }

  dateValidate(dateStart: Date, dateEnd: Date): boolean {

    const today = new Date();

    // Проверяем, если dateStart и dateEnd не являются объектами Date, преобразуем их
    const startDate =
      dateStart instanceof Date ? dateStart : new Date(dateStart);

    // Проверяем корректность даты
    const isStartDateCorrect: boolean = startDate.getTime() > today.getTime();

    if (!isStartDateCorrect) {
      this.notifier.error('You must enter start date later than today!');
      return false;
    }

    let isDataNotValid: boolean = dateStart > dateEnd;

    if (isDataNotValid) {
      this.notifier.error(
        'The date start must be earlier than date of trip end!'
      );
      return false;
    }

    return true;
  }
}
