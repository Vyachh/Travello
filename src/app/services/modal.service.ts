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
    this.isVisible$.next(true)
    this.loginFormVisible = true;
    this.signupFormVisible = false;
  }

  onSignupButtonClick(): void {
    this.isVisible$.next(true)
    this.loginFormVisible = false;
    this.signupFormVisible = true;
  }
  onBirthDateChange(): void {
    this.isVisible$.next(true)
    this.birthDateFormVisible = true;
    this.loginFormVisible = false;
    this.signupFormVisible = false;

  }


  close() {
    this.isVisible$.next(false)
  }
  constructor() { }
}
