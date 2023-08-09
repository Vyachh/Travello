import { Component } from '@angular/core';
import { EmailService } from 'src/app/services/email.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent {
  email: string

  constructor(private emailService: EmailService) {

  }

  subscribe() {
    const data = new FormData()
    data.append("To", this.email)
    
    this.emailService.subscribeToNews(data).subscribe()
  }
}
