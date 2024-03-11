import { Component } from '@angular/core';

import { EchoBody, EchoClient } from '~/client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  public requestMessage: string = '';
  public responseMessage: string = '';

  public yamlText: string = '';

  constructor(private readonly echoClient: EchoClient) {}

  onSubmit() {
    this.echo();
  }

  echo() {
    console.log(this.requestMessage);
    this.echoClient
      .echo(new EchoBody({ message: this.requestMessage }))
      .subscribe({
        next: (value) => {
          console.log(`response: ${value.message}`);
          this.responseMessage = value.message;
        },
        error: (err) => {
          console.log(`error: ${err.message}`);
          this.responseMessage = `Error: ${err.message}`;
        },
      });
  }
  onRecieveValidYAMLText(newText: string): void {
    this.yamlText = newText;
    console.log(this.yamlText);
  }
}
