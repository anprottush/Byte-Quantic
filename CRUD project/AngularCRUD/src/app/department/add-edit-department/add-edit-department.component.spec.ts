import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditDepartmentComponent } from './add-edit-department.component';

describe('AddEditDepartmentComponent', () => {
  let component: AddEditDepartmentComponent;
  let fixture: ComponentFixture<AddEditDepartmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddEditDepartmentComponent]
    });
    fixture = TestBed.createComponent(AddEditDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
