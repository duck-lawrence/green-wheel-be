import { UserRegisterCompleteReq } from "@/models/auth/schema/request"
import { TokenRes } from "@/models/auth/schema/response"
import axiosInstance from "@/utils/axios"
import { requestWrapper } from "@/utils/helpers/handleAxiosError"

export const authApi = {
    login: ({ email, password }: { email: string; password: string }) =>
        requestWrapper<TokenRes>(async () => {
            const res = await axiosInstance.post("/users/login", { email, password })
            return res.data
        }),
    logout: () =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/logout")
        }),
    regsiter: ({ email }: { email: string }) =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/register", { email })
        }),
    registerVerify: ({ otp, email }: { otp: string; email: string }) =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/register/verify-otp", { otp, email })
        }),
    regsiterComplete: (req: UserRegisterCompleteReq) =>
        requestWrapper<TokenRes>(async () => {
            const res = await axiosInstance.post("/users/register/complete", req)
            return res.data
        }),
    loginGoogle: (credential: string) =>
        requestWrapper<TokenRes>(async () => {
            const res = await axiosInstance.post("/users/login-google", { credential })
            return res.data
        })
}
