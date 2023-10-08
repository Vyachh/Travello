import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';
import { TripService } from 'src/app/services/trip.service';
import { TripForm } from './TripForm';

@Component({
  selector: 'app-admin-trip-form',
  templateUrl: './admin-trip-form.component.html',
  styleUrls: ['./admin-trip-form.component.css'],
})
export class AdminTripFormComponent implements OnInit{
  tripImage: File;
  tripForm: FormGroup;

  constructor(
    public accountService: AccountService,
    private fb: FormBuilder,
    private tripService: TripService,
    private router: Router,
    private notifier: ToastrService
  ) {}


  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.tripForm = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      dateStart: [null, [Validators.required]],
      dateEnd: [null, [Validators.required]],
      price: ['', [Validators.required]],
    });
  }

  onSubmit() {
    const dateStart: Date = this.tripForm.value.dateStart || new Date();
    const dateEnd: Date = this.tripForm.value.dateEnd || new Date();
    let isDateValid: boolean = this.dateValidate(dateStart, dateEnd);

    if (!isDateValid) {
      this.notifier.error('Date is not valid!');
      return;
    }

    if (this.tripForm.status === 'VALID') {

      let formData = new FormData();
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
