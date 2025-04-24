import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { UserDto } from '@/dtos/UserDtos';
import { BranchDto } from '@/dtos/BranchDtos';

interface SessionState {
  actualBranch: BranchDto | null;
  user: UserDto | null;
}

const initialState: SessionState = {
  actualBranch: null,
  user: null,
};

const sessionSlice = createSlice({
  name: 'session',
  initialState,
  reducers: {
    setActualBranch: (state, action: PayloadAction<BranchDto>) => {
      state.actualBranch = action.payload;
    },
    setUser: (state, action: PayloadAction<UserDto>) => {
      state.user = action.payload;
    },
    clearSession: (state) => {
      state.actualBranch = null;
      state.user = null;
    },
  },
});

export const { setActualBranch, setUser, clearSession } = sessionSlice.actions;
export default sessionSlice.reducer;
