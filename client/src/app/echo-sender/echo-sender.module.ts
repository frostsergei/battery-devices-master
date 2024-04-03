import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { EchoSenderComponent } from './echo-sender.component';

@NgModule({
  declarations: [EchoSenderComponent],
  imports: [CommonModule, FormsModule],
  exports: [EchoSenderComponent],
})
export class EchoSenderModule {}
