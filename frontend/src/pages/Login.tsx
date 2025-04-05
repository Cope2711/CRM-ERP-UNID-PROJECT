import { useDispatch, useSelector } from 'react-redux';
import { message } from 'antd';
import { login } from '@/redux/auth/thunks';
import { selectAuth } from '@/redux/auth/selectors';
import Loading from '@/components/Loading';
import LoginForm from '@/components/LoginForm';
import ErrorMessage from '@/components/ErrorMessage';
import { AppDispatch } from '@/redux/store';
import { LoginRequestDto } from '@/dtos/AuthDtos.ts';
import { useEffect } from 'react';

const LoginPage: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { isLoading, isError, errorMessage, isSuccess } = useSelector(selectAuth);

  const onFinish = (values: { UserUserName: string; UserPassword: string }) => {
    const loginRequest: LoginRequestDto = {
      UserUserName: values.UserUserName,
      UserPassword: values.UserPassword,
      DeviceId: 'some-device-id',
    };

    dispatch(login(loginRequest));
  };

  useEffect(() => {
    if (isSuccess) {
      message.success('Sesión iniciada');
    }
  }, [isSuccess]);

  useEffect(() => {
    if (isError && errorMessage) {
      message.error(errorMessage);
    }
  }, [isError, errorMessage]);

  return (
    <div>
      <h2>Login</h2>

      <Loading isLoading={isLoading}>
        <LoginForm onFinish={onFinish} />
      </Loading>

      {isError && errorMessage && <ErrorMessage message={errorMessage} />}
    </div>
  );
};

export default LoginPage;
