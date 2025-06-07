import { FormInstance } from "antd"

export const setFormErrorField = (form: FormInstance, error: { field: string, detail: string }): any => {
    if (error?.field && error?.detail) {
        form.setFields([
            {
                name: error.field,
                errors: [error.detail],
            },
        ]);
    }
};