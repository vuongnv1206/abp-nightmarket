import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionGrantComponent } from './permission-grant.component';

describe('PermissionGrantComponent', () => {
  let component: PermissionGrantComponent;
  let fixture: ComponentFixture<PermissionGrantComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PermissionGrantComponent]
    });
    fixture = TestBed.createComponent(PermissionGrantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
