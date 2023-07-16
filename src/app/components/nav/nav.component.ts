import { Component } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {

  isLoggedIn: boolean
  constructor(public modalService: ModalService, public accountService: AccountService) {
    this.isLoggedIn = accountService.isLoggedIn;
  }
}