import { RentalContractStatus } from "@/constants/enum"
import { QUERY_KEYS } from "@/constants/queryKey"
import { BackendError } from "@/models/common/response"
import { RentalContractViewRes } from "@/models/rental-contract/schema/response"
import { rentalContractApi } from "@/services/rentalContractApi"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export const useGetAllRentalContract = ({
    params,
    enabled = true
}: {
    params: { phone?: string; status?: RentalContractStatus }
    enabled?: boolean
}) => {
    const queryClient = useQueryClient()
    const query = useQuery({
        queryKey: [...QUERY_KEYS.VEHICLE_SEGMENTS, params],
        queryFn: () => rentalContractApi.getAll(params),
        initialData: () => {
            return queryClient.getQueryData<RentalContractViewRes[]>([
                ...QUERY_KEYS.VEHICLE_SEGMENTS,
                params
            ])
        },
        enabled
    })
    return query
}

export const useCreateRentalContract = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: rentalContractApi.create,
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("contral_form.wait_for_confirm"), {
                position: "top-center"
            })
        },
        onError: (error: BackendError) => {
            toast.error(translateWithFallback(t, error.detail))
        }
    })
}
