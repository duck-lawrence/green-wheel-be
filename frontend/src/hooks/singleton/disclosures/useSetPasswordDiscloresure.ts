import { DisclosureContext } from "@/providers/DisclosureProvider/DisclosureContext"
import { useDisclosure } from "@heroui/react"
import { useContext } from "react"

export const useSetPasswordDiscloresureCore = () => {
    return useDisclosure()
}

export const useSetPasswordDiscloresureSingleton = () => {
    const { useSetPasswordDiscloresure } = useContext(DisclosureContext)!
    return useSetPasswordDiscloresure
}
