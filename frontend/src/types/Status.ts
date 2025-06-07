type IdResponseType = {
    id: string;
    status: string;
    message: string;
    field: string;
}

export type IdResponseStatusSchema = {
    failed: IdResponseType[];
    success: IdResponseType[];
}