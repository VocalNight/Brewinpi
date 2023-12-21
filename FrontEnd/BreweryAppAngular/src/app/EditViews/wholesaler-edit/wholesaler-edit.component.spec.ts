import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WholesalerEditComponent } from './wholesaler-edit.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('WholesalerEditComponent', () => {
  let component: WholesalerEditComponent;
  let fixture: ComponentFixture<WholesalerEditComponent>;

  const mockComponent = {
    close: jasmine.createSpy('close')
  }

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WholesalerEditComponent, HttpClientModule, BrowserAnimationsModule],
      providers: [HttpClientModule, HttpClient, {
        provide: ActivatedRoute,
        useValue: mockComponent
      }]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WholesalerEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
