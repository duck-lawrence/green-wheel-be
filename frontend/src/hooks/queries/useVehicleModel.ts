import { QUERY_KEYS } from "@/constants/queryKey"
import { vehicleModelApi } from "@/services/vehicleModelApi"
import { useQuery } from "@tanstack/react-query"

export const useGetAllVehicleModels = ({
    query,
    enabled = true
}: {
    query: {
        stationId: string
        startDate: string
        endDate: string
        segmentId: string | null
    }
    enabled?: boolean
}) => {
    return useQuery({
        queryKey: [QUERY_KEYS.VEHICLE_MODELS, query],
        queryFn: async () => await vehicleModelApi.getAll(query),
        enabled
    })
}

export const useGetVehicleModelById = ({
    modelId,
    query,
    enabled = true
}: {
    modelId: string
    query: { stationId: string; startDate: string; endDate: string }
    enabled?: boolean
}) => {
    return useQuery({
        queryKey: QUERY_KEYS.VEHICLE_MODEL(modelId),
        queryFn: async () => await vehicleModelApi.getById({ modelId, query }),
        enabled
    })
}
