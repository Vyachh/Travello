import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IUser } from 'src/app/models/User';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {

  constructor(private accountService: AccountService) {

  }
  user: IUser = {
    userName: '',
    password: ''
  }

  authToken: string

  form = new FormGroup({
    name: new FormControl<string>('', [
      Validators.required,

    ]),
    password: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(6)
    ])
  })

  submit() {
    this.user.userName = this.form.value.name || '';
    this.user.password = this.form.value.password || '';

    this.accountService.signUp(this.user).subscribe({
      next: (response: string) => {
        localStorage.setItem('bearer', response);
      location.reload();
      },
      error: (e: any) => {
        console.error(e);
      }
    });

  }
}
