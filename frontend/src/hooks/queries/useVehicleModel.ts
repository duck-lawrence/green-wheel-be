import { QUERY_KEYS } from "@/constants/queryKey"
import { vehicleModelApi } from "@/services/vehicleModelApi"
import { useQuery } from "@tanstack/react-query"

export const useGetAllVehicleModels = ({
    query,
    enabled = true
}: {
    query: {
        stationId: string | null
        segmentId: string | null
        startDate: string | null
        endDate: string | null
    }
    enabled?: boolean
}) => {
    return useQuery({
        queryKey: [QUERY_KEYS.VEHICLE_MODELS, query],
        queryFn: async () => await vehicleModelApi.getAll(query),
        enabled
    })
}
