import { animate, style, transition, trigger } from '@angular/animations';
import { Component, Input } from '@angular/core';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('0.1s', style({ opacity: 1 })),
      ]),
      transition(':leave', [
        animate('0.1s', style({ opacity: 0 }))
      ]),
    ]),
  ],
})
export class ModalComponent {
  @Input() title: string
  constructor(public modalService: ModalService) {

  }
  modalVisible = false;
}
