import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FormComponent } from './form/form.component';
import { FormGenDemoComponent } from './form-gen-demo/form-gen-demo.component';

import { SharedModule } from '~/shared/shared.module';

@NgModule({
  declarations: [FormComponent, FormGenDemoComponent],
  exports: [FormComponent, FormGenDemoComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
    MatFormFieldModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatButtonModule,
  ],
})
export class SandboxModule {}
