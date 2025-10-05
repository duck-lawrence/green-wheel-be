import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import {
    useSetPasswordDiscloresureCore,
    useForgotPasswordDiscloresureCore,
    useLoginDiscloresureCore,
    useRegisterDiscloresureCore
} from "@/hooks"
import { useAvatarUploadDiscloresureCore } from "@/hooks/singleton/disclosures/useAvatarUploadDiscloresure"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisterDiscloresure = useRegisterDiscloresureCore()
    const useSetPasswordDiscloresure = useSetPasswordDiscloresureCore()
    const useForgotPasswordDiscloresure = useForgotPasswordDiscloresureCore()
    const useAvatarUploadDiscloresure = useAvatarUploadDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useLoginDiscloresure,
                useRegisterDiscloresure,
                useSetPasswordDiscloresure,
                useForgotPasswordDiscloresure,
                useAvatarUploadDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
