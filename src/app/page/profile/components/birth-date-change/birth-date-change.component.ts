import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-birth-date-change',
  templateUrl: './birth-date-change.component.html',
  styleUrls: ['./birth-date-change.component.css']
})
export class BirthDateChangeComponent implements OnInit {

  userInfo: IUserInfo

  constructor(private accountService: AccountService) {

  }

  ngOnInit(): void {
    this.form = new FormGroup({
      birthdate: new FormControl<string>(''),
      email: new FormControl<string>('', [Validators.required])
    });

  }

  form: FormGroup

  submit() {
    this.userInfo = this.accountService.userInfo

    this.userInfo.birthdate = this.form.value.birthdate || ''
    this.userInfo.email = this.form.value.email || ''

    this.accountService.changeInfo(this.userInfo).subscribe({
      next: (response:string) => {
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
