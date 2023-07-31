import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-subscribe-trip',
  templateUrl: './subscribe-trip.component.html',
  styleUrls: ['./subscribe-trip.component.css']
})
export class SubscribeTripComponent implements OnInit {

  form: FormGroup

  constructor(private formbuilder: FormBuilder) {
    
  }

  ngOnInit(): void {
    this.form = this.formbuilder.group({
      email: new FormControl<string>('', [Validators.required, Validators.email])
    });
  }

  get email() {
    return this.form.get('email');
  }

  submit() {

  }

  get fc() {
    return this.form.controls;
  }
}
