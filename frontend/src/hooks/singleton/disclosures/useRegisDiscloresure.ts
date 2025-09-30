import { DisclosureContext } from "@/providers/DisclosureProvider/DisclosureContext"
import { useDisclosure } from "@heroui/react"
import { useContext } from "react"

export const useRegisDiscloresureCore = () => {
    return useDisclosure()
}

export const useRegisDiscloresureSingleton = () => {
    const { useRegisDiscloresure } = useContext(DisclosureContext)!
    return useRegisDiscloresure
}
