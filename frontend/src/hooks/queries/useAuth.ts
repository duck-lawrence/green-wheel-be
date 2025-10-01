import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { authApi } from "@/services/authApi"
import { useMutation } from "@tanstack/react-query"
import { BackendError } from "@/models/common/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { useToken } from "@/hooks"
import { UserRegisterCompleteReq } from "@/models/auth/schema/request"

export const useLogin = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useToken((state) => state.setAccessToken)

    return useMutation({
        mutationFn: async ({
            email,
            password,
            rememberMe
        }: {
            email: string
            password: string
            rememberMe?: boolean
        }) => {
            const data = await authApi.login({ email, password })
            setAccessToken(data.accessToken, rememberMe)
        },
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.login"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useLogout = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const removeAccessToken = useToken((state) => state.removeAccessToken)

    return useMutation({
        mutationFn: async () => {
            await authApi.logout()
            removeAccessToken()
        },
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.logout"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useRegister = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.regsiter,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useRegisterVerify = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.registerVerify,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useRegisterComplete = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useToken((state) => state.setAccessToken)

    return useMutation({
        mutationFn: async (req: UserRegisterCompleteReq) => {
            const data = await authApi.regsiterComplete(req)
            setAccessToken(data.accessToken)
        },
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.register"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useLoginGoogle = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useToken((state) => state.setAccessToken)

    return useMutation({
        mutationFn: async (credential: string) => {
            const data = await authApi.loginGoogle(credential)
            setAccessToken(data.accessToken)
        },
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.login"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}
