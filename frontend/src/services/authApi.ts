import {
    UserChangePasswordReq,
    UserRegisterCompleteReq,
    UserSetPasswordReq
} from "@/models/auth/schema/request"
import { LoginGoogleRes, TokenRes } from "@/models/auth/schema/response"
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
    loginGoogle: (credential: string) =>
        requestWrapper<LoginGoogleRes>(async () => {
            const res = await axiosInstance.post("/users/login-google", { credential })
            return res.data
        }),
    setPassword: (req: UserSetPasswordReq) =>
        requestWrapper<TokenRes>(async () => {
            const res = await axiosInstance.post("/users/login-google/set-password", req)
            return res.data
        }),

    // Register
    register: ({ email }: { email: string }) =>
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

    // Password
    changePassword: (req: UserChangePasswordReq) =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/change-password", req)
        }),
    forgotPassword: ({ email }: { email: string }) =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/forgot-password", { email })
        }),
    forgotPasswordVerify: ({ otp, email }: { otp: string; email: string }) =>
        requestWrapper<void>(async () => {
            await axiosInstance.post("/users/forgot-password/verify-otp", { otp, email })
        }),
    resetPassword: ({
        newPassword,
        confirmNewPassword
    }: {
        newPassword: string
        confirmNewPassword: string
    }) =>
        requestWrapper<void>(async () => {
            await axiosInstance.put("/users/reset-password", {
                password: newPassword,
                confirmPassword: confirmNewPassword
            })
        })
}
