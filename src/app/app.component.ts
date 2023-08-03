import { Component, OnInit } from '@angular/core';
import { ModalService } from './services/modal.service';
import { AccountService } from './services/account.service';
import { Role } from './enum/role.enum';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Travello';

  constructor(public modalService: ModalService, public accountService: AccountService) {

  }
  ngOnInit(): void {
    if (this.accountService.isLoggedIn) {
      this.accountService.getInfo().subscribe({

        error: error => {
          console.error(error);
        }
      })
    }

  }


}
