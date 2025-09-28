import axios from "axios"
import { BackendError } from "@/models/Common/response"

// export const handleAxiosError = (error: unknown): never => {
//     if (axios.isAxiosError(error) && error.response?.data) {
//         throw error.response.data as BackendError
//     }
//     throw {
//         title: "Internal Server Error",
//         status: 500,
//         detail: "common.unexpected_error"
//     }
// }

export const requestWrapper = async <T>(fn: () => Promise<T>): Promise<T> => {
    try {
        return await fn()
    } catch (error: unknown) {
        if (axios.isAxiosError(error) && error.response?.data) {
            throw error.response.data as BackendError
        }
        throw {
            title: "Internal Server Error",
            status: 500,
            detail: "common.unexpected_error"
        }
    }
}
