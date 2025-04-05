import { RootState } from '@/redux/store.ts';

export const selectAuth = (state: RootState) => state.auth;