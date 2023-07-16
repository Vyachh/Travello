import { Component } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';


@Component({
  selector: 'app-testimonials',
  templateUrl: './testimonials.component.html',
  styleUrls: ['./testimonials.component.css'],
  animations: [
    trigger('blockAnimation', [
      state('active', style({
        opacity: 1,
        transform: 'translateY(0)',
        filter: 'none'

      })),
      state('blur', style({
        opacity: 1,
        transform: "translate(50%, 50%)",
        filter: 'blur(100px)'
      })),
      state('hidden', style({
        opacity: 0,
        transform: "translate(-50%, -50%)",
        filter: 'blur(50px)' 
      })),

      transition('active <=> blur', animate('500ms ease-in-out')),
      transition('blur <=> hidden', animate('500ms ease-in-out')),
      transition('hidden <=> active', animate('500ms ease-in-out')),
    ])
  ]

})
export class TestimonialsComponent {
  currentIconIndex: number = 0;

  activeClass: string[] = ['active', 'hidden', 'hidden'];
  iconColors: string[] = ['text-blue-950', 'text-gray-200', 'text-gray-200'];

  onUpArrowClick(): void {
    if (this.currentIconIndex > 0) {
      this.currentIconIndex--;
      this.swapIconColors(this.currentIconIndex, this.currentIconIndex + 1);
      this.swapActiveClass(this.currentIconIndex, this.currentIconIndex + 1);
    }
    else {
      this.currentIconIndex = 2;
      this.swapIconColors(this.currentIconIndex - 2, this.currentIconIndex);
      this.swapActiveClass(this.currentIconIndex - 2, this.currentIconIndex);
    }
    this.animateBlockTransition();
  }
  onDownArrowClick(): void {
    if (this.currentIconIndex < this.iconColors.length - 1) {
      this.currentIconIndex++;
      this.swapIconColors(this.currentIconIndex, this.currentIconIndex - 1);
      this.swapActiveClass(this.currentIconIndex, this.currentIconIndex + 2);
    }
    else {
      this.currentIconIndex = 0;
      this.swapIconColors(this.currentIconIndex, this.currentIconIndex + 2);
      this.swapActiveClass(this.currentIconIndex, this.currentIconIndex + 2);
    }
    this.animateBlockTransition();
  }
  swapIconColors(index1: number /* 1 */, index2: number): void {
    const tempColor = this.iconColors[index1];
    this.iconColors[index1] = this.iconColors[index2];
    this.iconColors[index2] = tempColor;
  }

  swapActiveClass(index1: number, index2: number): void {
    const tempClass = this.activeClass[index1];             // 1
    this.activeClass[index1] = this.activeClass[index2];    // 2 => 1
    this.activeClass[index2] = tempClass;                   // 1 => 2
  }

  animateBlockTransition(): void {
    // Вызовите переключение состояний для анимации
    this.activeClass = this.activeClass.map((value, index) => (index === this.currentIconIndex) ? 'active' : 'hidden');
  }
}
