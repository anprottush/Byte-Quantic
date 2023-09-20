export interface ApiResponse {
    success: boolean;
    request_time: string;
    response_time: string;
    request_url: string;
    message: Array<string>;
    payload: any;
    operation_type: string;
}
