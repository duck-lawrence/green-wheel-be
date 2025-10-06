import { useMutation, useQuery } from "@tanstack/react-query"
import { QUERY_KEYS } from "@/constants/queryKey"
import { useTranslation } from "react-i18next"
import toast from "react-hot-toast"
import { BackendError } from "@/models/common/response"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { UserUpdateReq } from "@/models/user/schema/request"
import { UserProfileViewRes } from "@/models/user/schema/response"
import { useProfileStore } from "@/hooks"
import { profileApi } from "@/services/profileApi"
import { getQueryClient } from "@/utils/helpers/getQueryClient"

export function useInvalidateMeQuery() {
    const queryClient = getQueryClient()

    return () => queryClient.invalidateQueries({ queryKey: QUERY_KEYS.ME })
}

export function useMeFromCache(): UserProfileViewRes | undefined {
    const queryClient = getQueryClient()
    return queryClient.getQueryData<UserProfileViewRes>(QUERY_KEYS.ME)
}

export const useGetMe = ({ enabled = true }: { enabled?: boolean } = {}) => {
    const query = useQuery({
        queryKey: QUERY_KEYS.ME,
        queryFn: profileApi.getMe,
        enabled
    })
    return query
}

export const useUpdateMe = ({
    onSuccess
}: {
    onSuccess?: (data: Partial<UserProfileViewRes>) => void
}) => {
    const { t } = useTranslation()
    const queryClient = getQueryClient()

    return useMutation({
        mutationFn: async (req: UserUpdateReq) => {
            await profileApi.updateMe(req)
            return req
        },
        onSuccess: (data) => {
            // update cache
            queryClient.setQueryData<UserProfileViewRes>(QUERY_KEYS.ME, (prev) => {
                if (!prev) return prev
                return {
                    ...prev,
                    ...data
                }
            })

            onSuccess?.(data)
            toast.success(t("success.update"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useUploadAvatar = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const queryClient = getQueryClient()
    const updateUser = useProfileStore((s) => s.updateUser)

    return useMutation({
        mutationFn: profileApi.uploadAvatar,
        onSuccess: (data) => {
            // update cache & Zustand store
            queryClient.setQueryData<UserProfileViewRes>(QUERY_KEYS.ME, (prev) => {
                if (!prev) return prev
                return {
                    ...prev,
                    ...data
                }
            })
            updateUser({ avatarUrl: data.avatarUrl })

            onSuccess?.()
            toast.success(t("success.upload"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}

export const useDeleteAvatar = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()
    const queryClient = getQueryClient()
    const updateUser = useProfileStore((s) => s.updateUser)

    return useMutation({
        mutationFn: profileApi.deleteAvatar,
        onSuccess: (data) => {
            // update cache & Zustand store
            queryClient.setQueryData<UserProfileViewRes>(QUERY_KEYS.ME, (prev) => {
                if (!prev) return prev
                return {
                    ...prev,
                    avatarUrl: undefined
                }
            })
            updateUser({ avatarUrl: undefined })

            onSuccess?.()
            toast.success(translateWithFallback(t, data.message))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}
