import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-birth-date-change',
  templateUrl: './birth-date-change.component.html',
  styleUrls: ['./birth-date-change.component.css']
})
export class BirthDateChangeComponent implements OnInit {

  userInfo: IUserInfo
  form: FormGroup

  emailRegEx: string = "[a-z0-9._%+\-]+@[a-z0-9.\-]+\.[a-z]{2,}$"

  constructor(private accountService: AccountService, private formbuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.form = this.formbuilder.group({
      birthdate: new FormControl<string>('', [Validators.required, this.ageValidator]),
      email: new FormControl<string>('', [Validators.required, Validators.email])
    });
  }

  ageValidator(control: FormControl): { [s: string]: boolean } | null {
    if (control.value) {
      const birthDate = new Date(control.value)
      const today = new Date()
      const diff = today.getTime() - birthDate.getTime();
      const age = diff / (1000 * 60 * 60 * 24 * 365.25);


      if (age < 14) {
        return { tooYoung: true };
      }
    }
    return null;
  }

  get fc(){
    return this.form.controls;
  }

  get email() {
    return this.form.get('email');
  }

  submit() {
    if (this.form.valid) {
      this.userInfo = this.accountService.userInfo

      this.userInfo.birthdate = this.form.value.birthdate || ''
      this.userInfo.email = this.form.value.email || ''

      this.accountService.changeInfo(this.userInfo).subscribe({
        next: (response: string) => {
          localStorage.removeItem('bearer');
          localStorage.setItem('bearer', response);
          location.reload();
        },
        error: (e: any) => {
          console.error(e);
        }
      })
    }

  }
}
