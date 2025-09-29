import axios from "axios"
import { BackendError } from "@/models/Common/response"

export const requestWrapper = async <T>(fn: () => Promise<T>): Promise<T> => {
    try {
        return await fn()
    } catch (error: unknown) {
        console.log("RAW ERROR in wrapper:", error)
        if (axios.isAxiosError(error)) {
            console.log("axios.isAxiosError = true")
            const data = error.response?.data
            const backendError: BackendError = {
                title: data?.title ?? "Error",
                status: data?.status ?? error.response?.status,
                detail: data?.detail ?? error.message
            }
            throw backendError
        }
        console.log("axios.isAxiosError = false")
        throw {
            title: "Internal Server Error",
            status: 500,
            detail: "common.unexpected_error"
        }
    }
}
