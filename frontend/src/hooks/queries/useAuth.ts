import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { authApi } from "@/services/authApi"
import { useMutation } from "@tanstack/react-query"
import { BackendError } from "@/models/Common/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { LoginUserReq } from "@/models/Auth/schema/request"
import { useToken } from "@/hooks"

export const useLogin = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useToken((state) => state.setAccessToken)

    return useMutation({
        mutationFn: async ({ req, rememberMe }: { req: LoginUserReq; rememberMe?: boolean }) => {
            const data = await authApi.login(req)
            setAccessToken(data.accessToken, rememberMe)
            return data
        },
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.login"))
        },
        onError: (error: BackendError) => {
            console.log(`${error.title}: ${error.detail}`)
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}
