import { StationViewRes } from "@/models/station/schema/response"
import axiosInstance from "@/utils/axios"
import { requestWrapper } from "@/utils/helpers/axiosHelper"

export const stationApi = {
    getAll: () =>
        requestWrapper<StationViewRes[]>(async () => {
            const res = await axiosInstance.get("/stations")
            return res.data
        })
}
