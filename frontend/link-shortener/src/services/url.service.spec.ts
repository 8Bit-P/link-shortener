import { TestBed } from '@angular/core/testing';
import { URLService } from './url.service';


describe('URLServiceService', () => {
  let service: URLService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(URLService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
