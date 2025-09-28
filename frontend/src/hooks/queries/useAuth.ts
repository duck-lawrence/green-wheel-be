import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { authApi } from "@/services/authApi"
import { useMutation } from "@tanstack/react-query"
import useToken from "@/hooks/singleton/store/useToken"
import { TokenRes } from "@/models/Auth/schema/response"
import { BackendError } from "@/models/Common/response"
import { getKeyWithFallback } from "@/utils/helpers/getKeyWithFallback"

export const useLogin = ({ rememberMe }: { rememberMe?: boolean } = {}) => {
    const { t } = useTranslation()
    const setAccessToken = useToken((state) => state.setAccessToken)

    return useMutation({
        mutationFn: authApi.login,
        onSuccess: (data: TokenRes) => {
            setAccessToken(data.accessToken, rememberMe)
            toast.success(t("success.login"))
        },
        onError: (error: BackendError) => {
            console.log(`${error.title}: ${error.detail}`)
            if (error.detail !== undefined) {
                toast.error(getKeyWithFallback(error.detail))
            }
        }
    })
}
