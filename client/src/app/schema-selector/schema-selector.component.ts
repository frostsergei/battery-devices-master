import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { ParametersSchema, SchemaClient, Schemas } from '~/client';

@Component({
  selector: 'app-schema-selector',
  templateUrl: './schema-selector.component.html',
  styleUrls: ['./schema-selector.component.scss'],
})
export class SchemaSelectorComponent implements OnInit {
  public schemas: ParametersSchema[] = [];
  public selectedSchema!: ParametersSchema;

  @Output() fileDataEmitter: EventEmitter<string> = new EventEmitter<string>();

  constructor(private readonly schemaClient: SchemaClient) {}

  ngOnInit(): void {
    this.schemaClient.getSchemas().subscribe({
      next: (schemas: Schemas) => {
        this.schemas = schemas.content;
        if (this.schemas.length > 0) {
          this.selectedSchema = this.schemas[0];
          this.emitFileData();
        }
      },
      error: (error: any) => {
        console.error('Failed to load schemas:', error);
      },
    });
  }

  onSelectionChange(): void {
    this.emitFileData();
  }

  private emitFileData(): void {
    this.fileDataEmitter.emit(this.selectedSchema.fileData);
  }
}
