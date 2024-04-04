import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[PreventDefaultStopPropagation]',
})
export class PreventDefaultStopPropagationDirective {
  constructor() {}

  @HostListener('click', ['$event'])
  onClick(event: MouseEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }
}
