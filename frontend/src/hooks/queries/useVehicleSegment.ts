import { QUERY_KEYS } from "@/constants/queryKey"
import { vehicleSegmentApi } from "@/services/vehicleSegmentApi"
import { useQuery } from "@tanstack/react-query"

export const useGetAllVehicleSegments = ({ enabled = true }: { enabled?: boolean } = {}) => {
    const query = useQuery({
        queryKey: QUERY_KEYS.VEHICLE_SEGMENTS,
        queryFn: vehicleSegmentApi.getAll,
        enabled
    })
    return query
}
