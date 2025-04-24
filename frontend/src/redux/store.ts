import { configureStore } from '@reduxjs/toolkit';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage'; 
import authReducer from './auth/authSlice';
import sessionReducer from './session/sessionSlice';

const persistConfig = {
  key: 'session', 
  storage, 
};

const persistedSessionReducer = persistReducer(persistConfig, sessionReducer);

export const store = configureStore({
  reducer: {
    auth: authReducer,
    session: persistedSessionReducer, 
  },
});

export const persistor = persistStore(store);

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
