import { useEffect, useRef, useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { FormInstance } from 'antd';

import { login } from '@/redux/auth/thunks';
import { selectAuth } from '@/redux/auth/selectors';
import { AppDispatch } from '@/redux/store';
import { LoginRequestDto } from '@/dtos/AuthDtos';

export const useLoginHandler = () => {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const formRef = useRef<FormInstance | null>(null);

    const { isLoading, isError, errorMessage, isSuccess, errorField } = useSelector(selectAuth);

    const onFinish = useCallback((values: { UserUserName: string; UserPassword: string }, form: FormInstance) => {
        form.setFields([
            { name: 'UserUserName', errors: [] },
            { name: 'UserPassword', errors: [] },
        ]);

        formRef.current = form;

        const loginRequest: LoginRequestDto = {
            UserUserName: values.UserUserName,
            UserPassword: values.UserPassword,
            DeviceId: 'some-device-id',
        };

        dispatch(login(loginRequest));
    }, [dispatch]);

    useEffect(() => {
        if (isSuccess) {
            formRef.current?.resetFields();
            navigate('/app/dashboard');
        }
    }, [isSuccess, navigate]);

    useEffect(() => {
        if (!formRef.current || !isError || !errorMessage || !errorField) return;

        formRef.current.setFields([
            {
                name: errorField,
                errors: [errorMessage],
            },
        ]);
    }, [isError, errorMessage, errorField]);

    return {
        isLoading,
        isError,
        errorMessage,
        onFinish,
    };
};
