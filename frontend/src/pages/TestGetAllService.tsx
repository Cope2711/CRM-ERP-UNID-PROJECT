import {useEffect} from "react";
import {GetAllDto} from "@/dtos/GenericDtos.ts";
import genericService from "@/services/genericService.ts";
import {FilterOperators} from "@/constants/filterOperators.ts";

const TestGetAllService = () => {
    useEffect(() => {
        const getAllDto: GetAllDto = {
            pageNumber: 1,
            pageSize: 10,
            orderBy: 'UserUserName',
            descending: false,
            filters: [
                {
                    column: 'IsActive',
                    operator: FilterOperators.Equal,
                    value: 'true'
                }
            ],
            selects: ['UserId', 'UserUserName', 'IsActive']
        };

        genericService.getAll('users', getAllDto).then((response) => {
            console.log(response);
        }).catch((error) => {
            console.log(error);
        });
    }, []);

    return (
        <div>
            <h1>Test GetAll Service</h1>
        </div>
    );
};

export default TestGetAllService;