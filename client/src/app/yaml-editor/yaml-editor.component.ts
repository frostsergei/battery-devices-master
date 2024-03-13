import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Output,
  ViewChild,
} from '@angular/core';
import { Ace, edit } from 'ace-builds';
import Mode from 'ace-builds/src-noconflict/mode-yaml';
import Theme from 'ace-builds/src-noconflict/theme-dracula';
import * as yamlParser from 'js-yaml';

@Component({
  selector: 'app-yaml-editor',
  templateUrl: './yaml-editor.component.html',
  styleUrl: './yaml-editor.component.scss',
})
export class YamlEditorComponent implements AfterViewInit {
  @ViewChild('editor') public editorRef!: ElementRef;
  @Output() public textEmitter: EventEmitter<string> =
    new EventEmitter<string>();
  public isValid: boolean = false;
  public text: string = '';
  public mode: string = 'yaml';
  public editor!: Ace.Editor;
  public options = {
    showPrintMargin: false,
    highlightActiveLine: true,
    tabSize: 2,
    wrap: true,
    fontSize: 14,
    fontFamily: "'Roboto Mono Regular', monospace",
  };

  ngAfterViewInit(): void {
    this.initEditor();
  }

  private initEditor(): void {
    this.editor = edit(this.editorRef.nativeElement);
    this.editor.setOptions(this.options);
    this.editor.setValue(this.text, -1);
    this.editor.setTheme(Theme);
    this.setEditorMode_();
    this.editor.session.setUseWorker(false);
    this.editor.on('change', () => this.onEditorTextChange());
  }

  private onEditorTextChange(): void {
    this.text = this.editor.getValue();
    try {
      yamlParser.load(this.text);
      this.editor.session.setAnnotations([]);
      this.isValid = true;
    } catch (e: unknown) {
      this.isValid = false;
      const yamlException = e as yamlParser.YAMLException;
      const errorLine = yamlException.mark.line;
      const errorColumn = yamlException.mark.column;
      const errorMessage = yamlException.message;
      this.editor.session.setAnnotations([
        {
          row: errorLine,
          column: errorColumn,
          text: errorMessage,
          type: 'error',
        },
      ]);
    }
  }

  private setEditorMode_(): void {
    this.editor.getSession().setMode(new Mode.Mode());
  }

  public emitYamlText() {
    this.textEmitter.emit(this.text);
  }
}
