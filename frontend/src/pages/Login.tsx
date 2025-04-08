import {useDispatch, useSelector} from 'react-redux';
import {login} from '@/redux/auth/thunks';
import {selectAuth} from '@/redux/auth/selectors';
import Loading from '@/components/Loading';
import LoginForm from '@/components/LoginForm';
import {AppDispatch} from '@/redux/store';
import {LoginRequestDto} from '@/dtos/AuthDtos.ts';
import {useEffect, useRef} from 'react';
import {FormInstance} from 'antd';
import AlertMessage from "@/components/AlertMessage.tsx";

const LoginPage: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const {
        isLoading,
        isError,
        errorMessage,
        isSuccess,
        errorField,
    } = useSelector(selectAuth);

    const formRef = useRef<FormInstance | null>(null);

    const onFinish = (
        values: { UserUserName: string; UserPassword: string },
        form: FormInstance
    ) => {
        form.setFields([
            { name: 'UserUserName', errors: [] },
            { name: 'UserPassword', errors: [] },
        ]);

        if (!formRef.current){
            formRef.current = form;
        }

        const loginRequest: LoginRequestDto = {
            UserUserName: values.UserUserName,
            UserPassword: values.UserPassword,
            DeviceId: 'some-device-id',
        };

        dispatch(login(loginRequest));
    };

    useEffect(() => {
        if (isSuccess) {
            formRef.current?.resetFields();
        }
    }, [isSuccess]);

    useEffect(() => {
        if (!formRef.current || !isError || !errorMessage || !errorField) return;

        formRef.current.setFields([
            {
                name: errorField,
                errors: [errorMessage],
            },
        ]);
    }, [isError, errorMessage, errorField]);

    return (
        <div>
            <Loading isLoading={isLoading}>
                <LoginForm onFinish={onFinish}/>
                {isError && errorMessage && !errorField && (
                    <AlertMessage message={errorMessage} duration={5000}/>
                )}
            </Loading>
        </div>
    );
};

export default LoginPage;
