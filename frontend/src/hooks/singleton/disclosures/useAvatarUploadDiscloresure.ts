import { DisclosureContext } from "@/providers/DisclosureProvider/DisclosureContext"
import { useDisclosure } from "@heroui/react"
import { useContext } from "react"

export const useAvatarUploadDiscloresureCore = () => {
    return useDisclosure()
}

export const useAvatarUploadDiscloresureSingleton = () => {
    const { useAvatarUploadDiscloresure } = useContext(DisclosureContext)!
    return useAvatarUploadDiscloresure
}
