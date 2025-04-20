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
            const { data } = await axiosInstance.get(`${modelName}/schema?type=${schemaType}`);
            return data;
        } catch (error: any) {
            const response = error.response.data;
            throw response;
        }
    }

    async create(modelName: string, data: any): Promise<any> {
        try {
            const { data: responseData } = await axiosInstance.post(`${modelName}/create`, data);
            return responseData;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async getById(modelName: string, id: string): Promise<any> {
        try {
            const { data } = await axiosInstance.get(`${modelName}/get-by-id?id=${id}`);
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async update(modelName: string, data: any, id: string): Promise<any> {
        try {
            const { data: responseData } = await axiosInstance.patch(`${modelName}/update/${id}`, data);
            return responseData;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async revoke(modelName: string, ids: string[]): Promise<any> {
        try {
            const { data } = await axiosInstance.delete(`${modelName}/revoke`, {
                data: { ids: ids }
              });
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async assign(modelName: string, modelAssignIds: { modelId: string, assignIds: string[] }): Promise<any> {
        try {
            const payload = {
                ModelAssignIds: modelAssignIds.assignIds.map(assignId => ({
                    ModelId: modelAssignIds.modelId,
                    AssignId: assignId,
                })),
            };
    
            // El modelName se incluye aquí en la llamada al endpoint
            const { data: responseData } = await axiosInstance.post(`${modelName}/assign`, payload);
            return responseData;
        } catch (error: any) {
            throw error.response?.data;
        }
    }
    
}

export default new GenericService();
