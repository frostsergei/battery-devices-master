import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private http: HttpClient) {}

  sendPingRequest() {
    this.http.get('https://localhost:8080/ping').subscribe(response => {
      console.log('Ping response:', response);
    }, error => {
      console.error('Error sending ping request:', error);
    });
  }
}
