import { LoginReq } from "@/models/Auth/schema/request"
import axiosInstance from "@/utils/axios"
import { requestWrapper } from "@/utils/helpers/handleAxiosError"

export const authApi = {
    login: (req: LoginReq) =>
        requestWrapper(async () => {
            const res = await axiosInstance.post("/users/login", req)
            return res.data
        })
}
