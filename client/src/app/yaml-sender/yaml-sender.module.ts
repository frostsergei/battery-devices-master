import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { YamlSenderComponent } from './yaml-sender.component';

import { YamlEditorModule } from '~/yaml-editor/yaml-editor.module';

@NgModule({
  declarations: [YamlSenderComponent],
  imports: [CommonModule, YamlEditorModule, FormsModule],
  exports: [YamlSenderComponent],
})
export class YamlSenderModule {}
