import axios from "axios"
import { BackendError } from "@/models/common/response"

export const requestWrapper = async <T>(fn: () => Promise<T>): Promise<T> => {
    try {
        return await fn()
    } catch (error: unknown) {
        if (axios.isAxiosError(error)) {
            const data = error.response?.data
            const backendError: BackendError = {
                title: data?.title ?? "Error",
                status: data?.status ?? error.response?.status,
                detail: data?.detail ?? error.message
            }
            throw backendError
        }
        throw {
            title: "Internal Server Error",
            status: 500,
            detail: "common.unexpected_error"
        }
    }
}
