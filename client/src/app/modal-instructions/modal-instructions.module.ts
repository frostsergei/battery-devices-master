import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatSliderModule } from '@angular/material/slider';

import { ModalInstructionsComponent } from './modal-instructions.component';

@NgModule({
  declarations: [ModalInstructionsComponent],
  imports: [CommonModule, MatDialogModule, MatSliderModule],
  providers: [{ provide: MatDialogRef, useValue: {} }],
  exports: [ModalInstructionsComponent],
})
export class ModalInstructionsModule {}
