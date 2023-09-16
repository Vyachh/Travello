import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-admin-sidebar',
  templateUrl: './admin-sidebar.component.html',
  styleUrls: ['./admin-sidebar.component.css']
})
export class AdminSidebarComponent {

  @Input() Role: string

  constructor(private router: Router,public accountService:AccountService) {

  }

  onRedirect(page: string) {
    this.router.navigate([`/${page}`])
  }
}
