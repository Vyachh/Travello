import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  isVisible$ = new BehaviorSubject<boolean>(false)

  loginFormVisible: boolean = false;
  signupFormVisible: boolean = false;
  birthDateFormVisible: boolean = false;
  subscribeFormVisible: boolean = false;
  editTripFormVisible: boolean = false;

  onLoginButtonClick(): void {
    this.setFormVisibility(true, false, false, false, false);
  }

  onSignupButtonClick(): void {
    this.setFormVisibility(false, true, false, false, false);
  }

  onBirthDateChange(): void {
    this.setFormVisibility(false, false, true, false, false);
  }

  onSubscribeButtonClick(): void {
    this.setFormVisibility(false, false, false, true, false);
  }

  onEditTripButtonClick(): void {
    this.setFormVisibility(false, false, false, false, true);
  }

  private setFormVisibility(login: boolean, signup: boolean, birthDate: boolean, subscribe: boolean, trip: boolean): void {
    this.isVisible$.next(true);
    this.loginFormVisible = login;
    this.signupFormVisible = signup;
    this.birthDateFormVisible = birthDate;
    this.subscribeFormVisible = subscribe;
    this.editTripFormVisible = trip
  }

  close() {
    this.isVisible$.next(false)
  }
}
