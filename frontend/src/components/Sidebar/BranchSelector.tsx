import { useDispatch, useSelector } from 'react-redux';
import { RootState } from '@/redux/store';
import { setActualBranch } from '@/redux/session/sessionSlice';
import DropdownMenu from '../DropDownMenu';

export default function BranchSelector() {
  const dispatch = useDispatch();
  const user = useSelector((state: RootState) => state.session.user);
  const actualBranch = useSelector((state: RootState) => state.session.actualBranch);

  if (!user?.branches || user.branches.length === 0) return null;

  return (
    <DropdownMenu
      buttonIcon={
        <span className="text-sm font-medium text-gray-800">
          {actualBranch?.branchName ?? 'Seleccionar sucursal'}
        </span>
      }
      options={user.branches.map(branch => ({
        button: (
          <button
            key={branch.branchId}
            className="w-full px-4 py-2 text-left hover:bg-gray-100"
            onClick={() => dispatch(setActualBranch(branch))}
          >
            {branch.branchName}
          </button>
        )
      }))}
    />
  );
}