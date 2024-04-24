import { Directive, ElementRef, Input, OnDestroy, Renderer2 } from '@angular/core';
import { Subscription } from 'rxjs';
import { ThemeService } from '../services/theme.service';

@Directive({
  selector: '[appTheme]'
})
export class ThemeDirective implements OnDestroy {
  private subscription: Subscription;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    private themeService: ThemeService
  ) {
    this.subscription = this.themeService.isDarkTheme.subscribe((darkTheme) => {
      this.setTheme(darkTheme);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  private setTheme(darkTheme: boolean) {
    if (darkTheme) {
      this.renderer.addClass(this.el.nativeElement, 'dark-theme');
    } else {
      this.renderer.removeClass(this.el.nativeElement, 'dark-theme');
    }
  }
}
