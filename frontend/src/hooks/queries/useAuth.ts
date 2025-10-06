import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"
import { authApi } from "@/services/authApi"
import { profileApi } from "@/services/profileApi"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { BackendError } from "@/models/common/response"
import { UserProfileViewRes } from "@/models/user/schema/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { useProfileStore, useTokenStore } from "@/hooks"
import { QUERY_KEY } from "@/constants/queryKey"

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
    const invalidateAuthQuery = useInvalidateAuthQuery()

    return useMutation({
        mutationFn: authApi.login,
        onSuccess: (data) => {
            setAccessToken(data.accessToken, rememberMe)
            invalidateAuthQuery()
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
    const removeAccessToken = useTokenStore((s) => s.removeAccessToken)
    const removeUser = useProfileStore((s) => s.removeUser)
    const invalidateAuthQuery = useInvalidateAuthQuery()

    return useMutation({
        mutationFn: authApi.logout,
        onSuccess: () => {
            onSuccess?.()
            removeAccessToken()
            removeUser()
            invalidateAuthQuery()
            toast.success(t("success.logout"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
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
    const setUser = useProfileStore((s) => s.setUser)
    const invalidateAuthQuery = useInvalidateAuthQuery()

    return useMutation({
        mutationFn: authApi.loginGoogle,
        onSuccess: (data) => {
            if (!data.needSetPassword) {
                setAccessToken(data.accessToken!, rememberMe)
                invalidateAuthQuery()
                toast.success(t("success.login"))
            } else {
                setUser({
                    email: "",
                    firstName: data.firstName || "",
                    lastName: data.lastName || ""
                })
                onNeedSetPassword()
            }
            onSuccess?.()
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useSetPassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const setAccessToken = useTokenStore((s) => s.setAccessToken)
    const invalidateAuthQuery = useInvalidateAuthQuery()

    return useMutation({
        mutationFn: authApi.setPassword,
        onSuccess: (data) => {
            setAccessToken(data.accessToken)
            invalidateAuthQuery()
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

// ===== Register =====
export const useRegister = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.register,
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
    const setAccessToken = useTokenStore((s) => s.setAccessToken)
    const invalidateAuthQuery = useInvalidateAuthQuery()

    return useMutation({
        mutationFn: authApi.regsiterComplete,
        onSuccess: (data) => {
            setAccessToken(data.accessToken)
            invalidateAuthQuery()
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

// ===== Password =====
export const useForgotPassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.forgotPassword,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useForgotPasswordVerify = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: authApi.forgotPasswordVerify,
        onSuccess: onSuccess,
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
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
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useChangePassword = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const removeAccessToken = useTokenStore((s) => s.removeAccessToken)
    const removeUser = useProfileStore((s) => s.removeUser)

    return useMutation({
        mutationFn: authApi.changePassword,
        onSuccess: () => {
            removeAccessToken()
            removeUser()
            onSuccess?.()
            toast.success(t("success.change_password"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}


function useInvalidateAuthQuery() {
    const queryClient = useQueryClient()

    return () =>
        queryClient.invalidateQueries({
            predicate: (query) =>
                Array.isArray(query.queryKey) && query.queryKey[0] === QUERY_KEY.AUTH
        })
}

export const useAuth = () => {
    return useQuery({
        queryKey: [QUERY_KEY.AUTH],
        queryFn: async () => {
            try {
                const profile = await profileApi.getMe()
                const roleFromObject =
                    typeof (profile as { role?: unknown }).role === "object"
                        ? (profile as { role?: { name?: string } }).role?.name
                        : undefined

                let normalizedRole =
                    profile.role ?? roleFromObject ?? profile.roleDetail?.name ?? profile.roleId

                return {
                    ...profile,
                    role: typeof normalizedRole === "string" ? normalizedRole : undefined,
                    roleId:
                        typeof normalizedRole === "string"
                            ? normalizedRole
                            : typeof profile.roleId === "string"
                              ? profile.roleId
                              : undefined,
                    roleDetail:
                        profile.roleDetail ??
                        (typeof profile.role === "object" && profile.role !== null
                            ? (profile.role as { id?: string; name?: string; description?: string })
                            : undefined)
                }
            } catch (error) {
                const backendError = error as BackendError
                if (backendError?.status === 401) {
                    return undefined
                }
                throw error
            }
        },
        retry: false,
        staleTime: Infinity
    })
}

