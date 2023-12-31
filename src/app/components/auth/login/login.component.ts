import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { IUser } from 'src/app/models/User';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  constructor(
    private accountService: AccountService,
    private notifier: ToastrService
  ) {}

  user: IUser = {
    userName: '',
    password: '',
  };

  authToken: string;

  form = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    password: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  submit() {
    this.user.userName = this.form.value.name || '';
    this.user.password = this.form.value.password || '';

    this.accountService.login(this.user).subscribe({
      next: (response) => {
        localStorage.setItem('bearer', response);
        location.reload();
      },
      error: (e) => {
        console.error(e);
        this.notifier.error(`${e.error}`, `${e.name}, status: ${e.status}`);
      },
    });
  }
}
