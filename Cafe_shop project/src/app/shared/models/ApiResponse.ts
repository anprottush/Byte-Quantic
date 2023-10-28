export interface ApiResponse<T> {
    success?: boolean;
    request_time?: string;
    response_time?: string;
    request_url?: string;
    message?: Array<string>;
    payload?: T;
    operation_type?: string;
}


export interface Payload<T> {
    data?: T,
    totalCount?: number
}