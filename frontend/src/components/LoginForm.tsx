import { Form, Input, Checkbox } from 'antd';
import { useForm } from 'antd/es/form/Form';
import SubmitButton from '@/components/Button/button-submit';

interface LoginFormProps {
    onFinish: (values: { UserUserName: string; UserPassword: string }, form: ReturnType<typeof useForm>[0]) => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onFinish }) => {
    const [form] = Form.useForm();

    return (
        <section
            className="bg-cover bg-center bg-no-repeat min-h-screen flex items-center justify-center px-6 py-8"
            style={{ backgroundImage: "url('https://w.wallhaven.cc/full/k9/wallhaven-k9mpkm.jpg')" }}
        >
            <div className="w-full max-w-md bg-black bg-opacity-30 backdrop-blur-lg rounded-xl shadow-lg p-8">
                <div className="text-center mb-6">
                    <img src="https://cdn.worldvectorlogo.com/logos/minecraft-1.svg" alt="Logo" className="mx-auto h-16" />
                </div>
                <h2 className="text-2xl font-bold text-white mb-6 text-center">Sign In</h2>

                <Form
                    form={form}
                    layout="vertical"
                    name="normal_login"
                    initialValues={{ remember: true }}
                    onFinish={(values) => onFinish(values, form)}
                    className="space-y-4"
                >
                    <Form.Item
                        label={<span className="text-white">Username</span>}
                        name="UserUserName"
                        rules={[{ required: true, message: 'Por favor ingrese su nombre de usuario' }]}
                    >
                        <Input
                            placeholder="username123"
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all bg-transparent placeholder-white"
                        />
                    </Form.Item>

                    <Form.Item
                        label={<span className="text-white">Password</span>}
                        name="UserPassword"
                        rules={[{ required: true, message: 'Por favor ingrese su contraseña' }]}
                    >
                        <Input.Password
                            placeholder="••••••••"
                            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all bg-transparent placeholder-white"
                        />
                    </Form.Item>

                    <div className="flex items-center justify-between">
                        <Form.Item name="remember" valuePropName="checked" noStyle>
                            <Checkbox className="text-white">Remember me</Checkbox>
                        </Form.Item>
                        <a href="#" className="text-sm font-medium text-white hover:text-indigo-500">
                            Forgot password?
                        </a>
                    </div>

                    <div className="flex justify-center">
                        <SubmitButton
                            onClick={() => {}}
                            className="w-full bg-indigo-600 hover:bg-green-100 text-black font-medium py-2.5 rounded-lg transition-colors"
                        >
                            Sign In
                        </SubmitButton>
                    </div>
                </Form>
            </div>
        </section>
    );
};

export default LoginForm;
