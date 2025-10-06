import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"
import axiosInstance from "@/utils/axios"
import { buildQueryParams, requestWrapper } from "@/utils/helpers/axiosHelper"

export const vehicleModelApi = {
    getAll: (query: {
        stationId: string | null
        segmentId: string | null
        startDate: string | null
        endDate: string | null
    }) =>
        requestWrapper<VehicleModelViewRes[]>(async () => {
            const params = buildQueryParams(query)

            const res = await axiosInstance.get("/vehicle-models", { params })
            return res.data
        })
}
