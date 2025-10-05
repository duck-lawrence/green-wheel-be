import { QUERY_KEYS } from "@/constants/queryKey"
import { stationApi } from "@/services/stationApi"
import { useQuery } from "@tanstack/react-query"

export const useGetAllStations = ({ enabled = true }: { enabled?: boolean } = {}) => {
    const query = useQuery({
        queryKey: QUERY_KEYS.STATIONS,
        queryFn: stationApi.getAll,
        enabled
    })
    return query
}
