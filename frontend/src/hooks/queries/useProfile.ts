import { profileApi } from "@/services/profileApi"
import { useQuery } from "@tanstack/react-query"
import { useTranslation } from "react-i18next"
import { useProfileStore } from "../singleton"
import toast from "react-hot-toast"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { BackendError } from "@/models/common/response"
import { QUERY_KEYS } from "@/constants/queryKey"
import { useEffect } from "react"

export const useGetMe = ({ onSuccess, enabled }: { onSuccess?: () => void; enabled?: boolean }) => {
    const { t } = useTranslation()
    const setUser = useProfileStore((s) => s.setUser)

    const query = useQuery({
        queryKey: QUERY_KEYS.ME,
        queryFn: profileApi.getMe,
        enabled
    })

    // handle success
    useEffect(() => {
        if (query.isSuccess && query.data) {
            onSuccess?.()
            setUser(query.data)
        }
    }, [query.isSuccess, query.data, setUser])

    // handle error
    useEffect(() => {
        if (query.isError) {
            const error = query.error as BackendError
            console.log(`${error.title}: ${error.detail}`)
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    }, [query.isError, query.error])

    return query
}
