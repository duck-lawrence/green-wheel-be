import { BACKEND_API_URL } from "@/constants/api"
import { useToken } from "@/hooks"
import i18n from "@/lib/i18n"
import axios from "axios"
import toast from "react-hot-toast"

const axiosInstance = axios.create({
    baseURL: BACKEND_API_URL,
    withCredentials: true,
    headers: { "Content-Type": "application/json" }
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

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true
            try {
                const res = await axiosInstance.post(
                    "/users/refresh-token",
                    {},
                    { withCredentials: true }
                )
                useToken.getState().setAccessToken(res.data.accessToken)

                originalRequest.headers = originalRequest.headers || {}
                originalRequest.headers["Authorization"] = `Bearer ${res.data.accessToken}`

                return axiosInstance(originalRequest)
            } catch (refreshError) {
                toast(i18n.t("login.please_login"))
                return Promise.reject(refreshError)
            }
        }

        return Promise.reject(error)
    }
)

export default axiosInstance
