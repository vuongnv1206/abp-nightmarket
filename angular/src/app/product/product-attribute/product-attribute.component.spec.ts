import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAttributeComponent } from './product-attribute.component';

describe('ProductAttributeComponent', () => {
  let component: ProductAttributeComponent;
  let fixture: ComponentFixture<ProductAttributeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductAttributeComponent]
    });
    fixture = TestBed.createComponent(ProductAttributeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
