import { Component } from '@angular/core';
import { ModalService } from './services/modal.service';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Travello';
  /**
   *
   */
  constructor(public modalService: ModalService, public accountService: AccountService) {

  }
}
