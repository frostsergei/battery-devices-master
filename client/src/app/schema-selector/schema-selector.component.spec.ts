import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SchemaSelectorComponent } from './schema-selector.component';

describe('SchemaSelectorComponent', () => {
  let component: SchemaSelectorComponent;
  let fixture: ComponentFixture<SchemaSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SchemaSelectorComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SchemaSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
