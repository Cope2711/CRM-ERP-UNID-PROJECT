import { ErrorDetail } from '@/dtos/ErrorDetailDtos.ts';
import { axiosInstance } from '@/services/axiosConfig';
import type { Schema } from "@/types/Schema";
import {GetAllDto} from "@/dtos/GenericDtos.ts";

class GenericService {
    async getAll(modelName: string, getAllDto: GetAllDto): Promise<any> {
        try {
            const { data } = await axiosInstance.post(`${modelName}/get-all`, getAllDto);
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async getSchemas(modelName: string, schemaType: string): Promise<Schema> {
        try {
            const { data } = await axiosInstance.get(`${modelName}/get-create-schema?type=${schemaType}`);
            return data;
        } catch (error: any) {
            const response = error.response;
            const errorDetail: ErrorDetail = {
                title: response?.data?.title || 'Error',
                status: response?.status || 500,
                detail: response?.data?.detail || 'Error al obtener el esquema',
                field: response?.data?.field,
            };
            throw errorDetail;
        }
    }

    async create(modelName: string, data: any): Promise<any> {
        try {
            const { data: responseData } = await axiosInstance.post(`${modelName}/create`, data);
            return responseData;
        } catch (error: any) {
            const response = error.response;
            const errorDetail: ErrorDetail = {
                title: response?.data?.title || 'Error',
                status: response?.status || 500,
                detail: response?.data?.detail || 'Error al crear el registro',
                field: response?.data?.field,
            };
            throw errorDetail;
        }
    }
}

export default new GenericService();
