import { Component, Input } from '@angular/core';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-admin-sidebar',
  templateUrl: './admin-sidebar.component.html',
  styleUrls: ['./admin-sidebar.component.css']
})
export class AdminSidebarComponent {

@Input() Role: string

  /**
   *
   */
  constructor(private router: Router) {

  }

  onRedirect(page: string) {
    this.router.navigate([`/${page}`])
  }
}
