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

    return useMutation({
        mutationFn: async (req: UserUpdateReq) => {
            await profileApi.updateMe(req)
            return req
        },
        onSuccess: (data) => {
            onSuccess?.({
                ...data
            })
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
    const updateUser = useProfileStore((s) => s.updateUser)

    return useMutation({
        mutationFn: profileApi.uploadAvatar,
        onSuccess: (data) => {
            onSuccess?.()
            toast.success(t("success.upload"))
            updateUser({ avatarUrl: data.avatarUrl })
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
    const updateUser = useProfileStore((s) => s.updateUser)

    return useMutation({
        mutationFn: profileApi.deleteAvatar,
        onSuccess: (data) => {
            onSuccess?.()
            toast.success(translateWithFallback(t, data.message))
            updateUser({ avatarUrl: undefined })
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}
