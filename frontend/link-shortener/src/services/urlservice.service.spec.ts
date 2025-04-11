import { TestBed } from '@angular/core/testing';

import { URLServiceService } from './urlservice.service';

describe('URLServiceService', () => {
  let service: URLServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(URLServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
