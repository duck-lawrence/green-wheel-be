import { BackendError } from "@/models/common/response"
import { rentalContractApi } from "@/services/rentalContractApi"
import { translateWithFallback } from "@/utils/helpers/translateWithFallback"
import { useMutation } from "@tanstack/react-query"
import toast from "react-hot-toast"
import { useTranslation } from "react-i18next"

export const useCreateRentalContract = ({ onSuccess }: { onSuccess?: () => void }) => {
    const { t } = useTranslation()

    return useMutation({
        mutationFn: rentalContractApi.create,
        onSuccess: () => {
            onSuccess?.()
            toast.success(t("success.create"))
        },
        onError: (error: BackendError) => {
            if (error.detail !== undefined) {
                toast.error(translateWithFallback(t, error.detail))
            }
        }
    })
}
