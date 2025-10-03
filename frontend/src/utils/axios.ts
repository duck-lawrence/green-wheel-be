import { BACKEND_API_URL } from "@/constants/api"
import { useToken } from "@/hooks"
import axios from "axios"

const axiosInstance = axios.create({
    baseURL: BACKEND_API_URL,
    withCredentials: true,
    headers: { "Content-Type": "application/json" }
})

const refreshInstance = axios.create({
    baseURL: BACKEND_API_URL,
    withCredentials: true
})

// request interceptors
axiosInstance.interceptors.request.use(
    (config) => {
        const token = useToken.getState().accessToken
        if (token) {
            config.headers = config.headers || {}
            config.headers["Authorization"] = `Bearer ${token}`
        }
        return config
    },
    (error) => Promise.reject(error)
)

// response interceptors
axiosInstance.interceptors.response.use(
    (res) => res,
    async (error) => {
        const originalRequest = error.config
        const hasToken = !!useToken.getState().accessToken

        if (
            error.response?.status === 401 &&
            // (error.response?.data.detail === "user.invalid_token" || hasToken) &&
            !originalRequest.sent
        ) {
            originalRequest.sent = true
            try {
                useToken.getState().removeAccessToken()
                const res = await refreshInstance.post("/users/refresh-token")
                useToken.getState().setAccessToken(res.data.accessToken)

                originalRequest.headers = originalRequest.headers || {}
                originalRequest.headers["Authorization"] = `Bearer ${res.data.accessToken}`

                return axiosInstance(originalRequest)
            } catch (refreshError) {
                // toast.error(i18n.t("login.please_login"))
                return Promise.reject(refreshError)
            }
        }
        return Promise.reject(error)
    }
)

export default axiosInstance
