import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { FormComponent } from './form/form.component';
import { FormGenDemoComponent } from './form-gen-demo/form-gen-demo.component';

import { SchemaSelectorComponent } from '~/schema-selector/schema-selector.component';
import { SharedModule } from '~/shared/shared.module';

@NgModule({
  declarations: [FormComponent, FormGenDemoComponent, SchemaSelectorComponent],
  exports: [FormComponent, FormGenDemoComponent],
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
    MatFormFieldModule,
    MatInputModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatTabsModule,
  ],
})
export class SandboxModule {}
