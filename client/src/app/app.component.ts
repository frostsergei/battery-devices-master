import {Component} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private readonly http: HttpClient) {
  }

  sendPingRequest() {
    this.http.get('https://localhost:8080/ping').subscribe({
      next: value => console.log('Observable emitted the next value: ', value),
      error: err => console.error('Observable emitted an error: ', err),
      complete: () => console.log('Observable emitted the complete notification')
    });
  }
}
