import { LoginUserReq } from "@/models/auth/schema/request"
import { TokenRes } from "@/models/auth/schema/response"
import axiosInstance from "@/utils/axios"
import { requestWrapper } from "@/utils/helpers/handleAxiosError"

export const authApi = {
    login: (req: LoginUserReq) =>
        requestWrapper<TokenRes>(async () => {
            const res = await axiosInstance.post("/users/login", req)
            return res.data
        }),

    logout: () =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/logout")
        })
}
