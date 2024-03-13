import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { YamlEditorComponent } from './yaml-editor.component';

@NgModule({
  declarations: [YamlEditorComponent],
  imports: [CommonModule],
  exports: [YamlEditorComponent],
})
export class YamlEditorModule {}
