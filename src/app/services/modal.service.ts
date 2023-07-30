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
  emailFormVisible: boolean = false;

  onLoginButtonClick(): void {
    this.setFormVisibility(true, false, false, false);
  }

  onSignupButtonClick(): void {
    this.setFormVisibility(false, true, false, false);
  }
  onBirthDateChange(): void {
    this.setFormVisibility(false, false, true, false);
  }

  private setFormVisibility(login: boolean, signup: boolean, birthDate: boolean, email: boolean): void {
    this.isVisible$.next(true);
    this.loginFormVisible = login;
    this.signupFormVisible = signup;
    this.birthDateFormVisible = birthDate;
    this.emailFormVisible = email;
  }

  close() {
    this.isVisible$.next(false)
  }
}
