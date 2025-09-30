import { UserProfileViewRes } from "@/models/user/schema/response"
import axiosInstance from "@/utils/axios"
import { requestWrapper } from "@/utils/helpers/handleAxiosError"

export const profileApi = {
    getMe: () =>
        requestWrapper<UserProfileViewRes>(async () => {
            const res = await axiosInstance.get("/users/me")
            return res.data
        })
}
