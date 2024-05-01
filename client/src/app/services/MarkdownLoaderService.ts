import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MarkdownLoaderService {
  constructor(private http: HttpClient) {}

  loadMarkdown(): Observable<string> {
    return this.http.get('assets/instructions.md', { responseType: 'text' });
  }
}
