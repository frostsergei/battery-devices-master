import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

import * as marked from 'marked';

@Component({
  selector: 'app-modal-instructions',
  templateUrl: './modal-instructions.component.html',
  styleUrl: './modal-instructions.component.scss',
})
export class ModalInstructionsComponent {
  htmlContent: string | Promise<string> = '';
  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: { markdownContent: string | Promise<string> },
    public dialogRef: MatDialogRef<ModalInstructionsComponent>,
  ) {
    if (typeof this.data.markdownContent === 'string') {
      this.htmlContent = marked.parse(this.data.markdownContent);
    } else {
      (this.data.markdownContent as Promise<string>).then((content: string) => {
        this.htmlContent = marked.parse(content);
      });
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
