import { RentalContractStatus } from "@/constants/enum"
import { CreateRentalContractReq } from "@/models/rental-contract/schema/request"
import { RentalContractViewRes } from "@/models/rental-contract/schema/response"
import axiosInstance from "@/utils/axios"
import { buildQueryParams, requestWrapper } from "@/utils/helpers/axiosHelper"

export const rentalContractApi = {
    create: (req: CreateRentalContractReq) =>
        requestWrapper<RentalContractViewRes>(async () => {
            const res = await axiosInstance.post("/rental-contracts", req)
            return res.data
        }),
    getAll: (query: { phone?: string; status?: RentalContractStatus }) =>
        requestWrapper<RentalContractViewRes[]>(async () => {
            const params = buildQueryParams(query)

            const res = await axiosInstance.get("/rental-contracts", { params })
            return res.data
        })
}
