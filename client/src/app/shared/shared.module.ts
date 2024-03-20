import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

import { InputComponent } from './input/input.component';
import { PreventDefaultStopPropagationDirective } from './input/prevent-default-stop-propagation.directive';

@NgModule({
  declarations: [InputComponent, PreventDefaultStopPropagationDirective],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDividerModule,
    MatButtonModule,
  ],
  exports: [InputComponent],
})
export class SharedModule { }
