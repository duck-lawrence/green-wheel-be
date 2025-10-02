import { DisclosureContext } from "@/providers/DisclosureProvider/DisclosureContext"
import { useDisclosure } from "@heroui/react"
import { useContext } from "react"

export const useChangePasswordDiscloresureCore = () => {
    return useDisclosure()
}

export const useChangePasswordDiscloresureSingleton = () => {
    const { useChangePasswordDiscloresure } = useContext(DisclosureContext)!
    return useChangePasswordDiscloresure
}
