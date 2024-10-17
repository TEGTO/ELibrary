import { TestBed } from '@angular/core/testing';
import { Store } from '@ngrx/store';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { of } from 'rxjs';
import { changeChatVisibilityState } from '../..';
import { ChatControllerService } from './chat-controller.service';

describe('ChatControllerService', () => {
  let service: ChatControllerService;
  let store: MockStore;
  const initialState = { isVisible: false };

  beforeEach(() => {

    TestBed.configureTestingModule({
      providers: [
        ChatControllerService,
        provideMockStore({ initialState }),
      ]
    });

    service = TestBed.inject(ChatControllerService);
    store = TestBed.inject(Store) as MockStore;

    spyOn(store, 'dispatch');
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return chat visibility state from store', () => {
    const mockState = true;
    spyOn(store, 'select').and.returnValue(of(mockState));

    service.getChatVisibilityState().subscribe((state) => {
      expect(state).toBe(mockState);
    });

  });

  it('should dispatch action to change chat visibility state', () => {
    const newState = false;
    service.changeChatVisibilityState(newState);

    expect(store.dispatch).toHaveBeenCalledWith(changeChatVisibilityState({ state: newState }));
  });
});