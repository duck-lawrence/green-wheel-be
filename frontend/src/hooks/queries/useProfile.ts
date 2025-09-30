import { profileApi } from "@/services/profileApi"
import { useQuery } from "@tanstack/react-query"
import { QUERY_KEYS } from "@/constants/queryKey"

export const useGetMe = ({ enabled }: { enabled?: boolean }) => {
    const query = useQuery({
        queryKey: QUERY_KEYS.ME,
        queryFn: profileApi.getMe,
        enabled
    })

    return query
}
