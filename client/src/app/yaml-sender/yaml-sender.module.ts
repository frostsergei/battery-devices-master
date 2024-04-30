import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';

import { YamlSenderComponent } from './yaml-sender.component';

import { ModalInstructionsModule } from '~/modal-instructions/modal-instructions.module';
import { MarkdownLoaderService } from '~/services/MarkdownLoaderService';
import { YamlEditorModule } from '~/yaml-editor/yaml-editor.module';

@NgModule({
  declarations: [YamlSenderComponent],
  imports: [
    CommonModule,
    YamlEditorModule,
    FormsModule,
    MatDialogModule,
    ModalInstructionsModule,
  ],
  providers: [MarkdownLoaderService],
  exports: [YamlSenderComponent],
})
export class YamlSenderModule {}
