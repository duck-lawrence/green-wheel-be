import { VehicleSegmentViewRes } from "@/models/vehicle-segment/schema/response"
import axiosInstance from "@/utils/axios"
import { requestWrapper } from "@/utils/helpers/axiosHelper"

export const vehicleSegmentApi = {
    getAll: () =>
        requestWrapper<VehicleSegmentViewRes[]>(async () => {
            const res = await axiosInstance.get("/vehicle-segments")
            return res.data
        })
}
