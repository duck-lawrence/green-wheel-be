import { QUERY_KEYS } from "@/constants/queryKey"
import { VehicleModelViewRes } from "@/models/vehicle/schema/response"
import { vehicleModelApi } from "@/services/vehicleModelApi"
import { useQuery, useQueryClient } from "@tanstack/react-query"

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
    const queryClient = useQueryClient()
    return useQuery({
        queryKey: [...QUERY_KEYS.VEHICLE_MODELS, modelId, query],
        queryFn: () => vehicleModelApi.getById({ modelId, query }),
        initialData: () => {
            const cachedList = queryClient.getQueryData<VehicleModelViewRes[]>([
                ...QUERY_KEYS.VEHICLE_MODELS,
                modelId,
                query
            ])
            return cachedList?.find((v) => v.id === modelId)
        },
        enabled
    })
}
