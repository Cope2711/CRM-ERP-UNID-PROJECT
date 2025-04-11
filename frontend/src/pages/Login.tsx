import { useLoginHandler } from '@/hooks/useLoginHandler';
import { useRedirectIfAuthenticated } from '@/hooks/useRedirectIfAuthenticated';

import Loading from '@/components/Loading';
import LoginForm from '@/components/LoginForm';
import AlertMessage from '@/components/AlertMessage';

const LoginPage: React.FC = () => {
    useRedirectIfAuthenticated();
    const { isLoading, isError, errorMessage, onFinish } = useLoginHandler();

    return (
        <div>
            <Loading isLoading={isLoading}>
                <LoginForm onFinish={onFinish} />
                {isError && errorMessage && (
                    <AlertMessage message={errorMessage} duration={5000} />
                )}
            </Loading>
        </div>
    );
};

export default LoginPage;