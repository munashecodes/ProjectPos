import { TestBed } from '@angular/core/testing';

import { AccountCategoryService } from './account-category.service';

describe('AccountCategoryService', () => {
  let service: AccountCategoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccountCategoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
