export interface ServiceResponse<T>{
    data?: T
    message?: string
    isSuccess?: boolean
    time?: Date
}