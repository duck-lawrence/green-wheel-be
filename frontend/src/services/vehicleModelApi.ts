import { VehicleModelViewRes } from "@/models/vehicle/schema/response"
import axiosInstance from "@/utils/axios"
import { buildQueryParams, requestWrapper } from "@/utils/helpers/axiosHelper"

export const vehicleModelApi = {
    getAll: (query: {
        stationId: string
        startDate: string
        endDate: string
        segmentId: string | null
    }) =>
        requestWrapper<VehicleModelViewRes[]>(async () => {
            const params = buildQueryParams(query)

            const res = await axiosInstance.get("/vehicle-models", { params })
            return res.data
        }),

    getById: ({
        modelId,
        query
    }: {
        modelId: string
        query: { stationId: string; startDate: string; endDate: string }
    }) =>
        requestWrapper<VehicleModelViewRes>(async () => {
            const params = buildQueryParams(query)
            const res = await axiosInstance.get(`/vehicle-models/${modelId}`, { params })
            return res.data
        })
}
