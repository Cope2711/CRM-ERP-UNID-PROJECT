import { axiosInstance } from '@/services/axiosConfig';
import {GetAllDto} from "@/dtos/GenericDtos.ts";
import { IdResponseStatusSchema } from '@/types/Status';
import { PropertiesSchema } from '@/types/Schema';

class GenericService {
    async getAll(modelName: string, getAllDto: GetAllDto): Promise<any> {
        try {
            const { data } = await axiosInstance.post(`${modelName}/get-all`, getAllDto);
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async getSchemas(modelName: string, ignoreRequired = false): Promise<PropertiesSchema> {
        try {
            const { data } = await axiosInstance.get(`${modelName}/schema?ignoreRequired=${ignoreRequired}`);
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

    async revoke(modelName: string, ids: string[]): Promise<IdResponseStatusSchema> {
        try {
            const { data } = await axiosInstance.delete(`${modelName}/revoke`, {
                data: { ids: ids }
              });
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async assign(controller: string, modelAssignIds: { modelId: string, assignIds: string[] }, senderResource: string): Promise<any> {
        try {
            const payload = {
                ModelAssignIds: modelAssignIds.assignIds.map(assignId => ({
                    ModelId: modelAssignIds.modelId,
                    AssignId: assignId,
                })),
            };
    
            const { data: responseData } = await axiosInstance.post(`${controller}/assign`, payload, {
                params: {
                    modelName: senderResource, 
                }
            });
    
            return responseData;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async activate(ids: string[], modelName: string): Promise<any> {
        try {
            const { data } = await axiosInstance.patch(`${modelName}/activate`, {
                ids: ids
            });
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }

    async deactivate(ids: string[], modelName: string): Promise<any> {
        try {
            const { data } = await axiosInstance.patch(`${modelName}/deactivate`, {
                ids: ids
            });
            return data;
        } catch (error: any) {
            throw error.response?.data;
        }
    }
}

export default new GenericService();
