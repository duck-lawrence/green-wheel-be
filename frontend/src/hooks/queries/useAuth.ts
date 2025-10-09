import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { authApi } from "@/services/authApi"
import { useMutation } from "@tanstack/react-query"
import { BackendError } from "@/models/common/response"
// import { UserProfileViewRes } from "@/models/user/schema/response"
import { useInvalidateMeQuery, useTokenStore } from "@/hooks"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"

// ===== Login and logout =====
export const useLogin = ({
    rememberMe,
    onSuccess
}: {
    rememberMe?: boolean
    onSuccess?: () => void
}) => {
    const { t } = useTranslation()
    const setAccessToken = useTokenStore((s) => s.setAccessToken)
    const invalidateAuthQuery = useInvalidateMeQuery()

    return useMutation({
        mutationFn: authApi.login,
        onSuccess: (data) => {
            setAccessToken(data.accessToken, rememberMe)
            invalidateAuthQuery()
            onSuccess?.()
            toast.success(t("success.login"))
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useLogout = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const removeAccessToken = useTokenStore((s) => s.removeAccessToken)
    const invalidateAuthQuery = useInvalidateMeQuery()

    return useMutation({
        mutationFn: authApi.logout,
        onSuccess: () => {
            onSuccess?.()
            removeAccessToken()
            invalidateAuthQuery()
            toast.success(t("success.logout"))
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useLoginGoogle = ({
    rememberMe,
    onNeedSetPassword,
    onSuccess
}: {
    rememberMe?: boolean
    onNeedSetPassword: () => void
    onSuccess?: () => void
}) => {
    const { t } = useTranslation()
    const setAccessToken = useTokenStore((s) => s.setAccessToken)
    const invalidateAuthQuery = useInvalidateMeQuery()

    return useMutation({
        mutationFn: authApi.loginGoogle,
        onSuccess: (data) => {
            if (!data.needSetPassword) {
                setAccessToken(data.accessToken!, rememberMe)
                invalidateAuthQuery()
                toast.success(t("success.login"))
            } else {
                onNeedSetPassword()
            }
            onSuccess?.()
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useSetPassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useTokenStore((s) => s.setAccessToken)
    const invalidateAuthQuery = useInvalidateMeQuery()

    return useMutation({
        mutationFn: authApi.setPassword,
        onSuccess: (data) => {
            setAccessToken(data.accessToken)
            invalidateAuthQuery()
            onSuccess?.()
            toast.success(t("success.login"))
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

// ===== Register =====
export const useRegister = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.register,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useRegisterVerify = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.registerVerify,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useRegisterComplete = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useTokenStore((s) => s.setAccessToken)
    const invalidateAuthQuery = useInvalidateMeQuery()

    return useMutation({
        mutationFn: authApi.regsiterComplete,
        onSuccess: (data) => {
            setAccessToken(data.accessToken)
            invalidateAuthQuery()
            onSuccess?.()
            toast.success(t("success.register"))
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

// ===== Password =====
export const useForgotPassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.forgotPassword,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useForgotPasswordVerify = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.forgotPasswordVerify,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useResetPassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.resetPassword,
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.reset_password"))
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}

export const useChangePassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const removeAccessToken = useTokenStore((s) => s.removeAccessToken)
    const invalidateAuthQuery = useInvalidateMeQuery()

    return useMutation({
        mutationFn: authApi.changePassword,
        onSuccess: () => {
            removeAccessToken()
            invalidateAuthQuery()
            onSuccess?.()
            toast.success(t("success.change_password"))
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}
