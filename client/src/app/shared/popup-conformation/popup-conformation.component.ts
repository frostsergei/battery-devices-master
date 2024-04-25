import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-popup-conformation',
  templateUrl: './popup-conformation.component.html',
  styleUrls: ['./popup-conformation.component.scss'],
  animations: [
    trigger('fadeOut', [
      state(
        'visible',
        style({
          opacity: 1,
        }),
      ),
      state(
        'hidden',
        style({
          opacity: 0,
        }),
      ),
      transition('visible => hidden', [animate('3s ease-out')]),
    ]),
  ],
})
export class PopupConformationComponent {
  @Input() message: string = '';
  isShown: boolean = false;

  onAnimationDone() {
    this.isShown = false;
  }
}
