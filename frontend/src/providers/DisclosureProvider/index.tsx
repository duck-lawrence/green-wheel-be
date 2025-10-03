import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import {
    useChangePasswordDiscloresureCore,
    useForgotPasswordDiscloresureCore,
    useLoginDiscloresureCore,
    useRegisterDiscloresureCore
} from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisterDiscloresure = useRegisterDiscloresureCore()
    const useChangePasswordDiscloresure = useChangePasswordDiscloresureCore()
    const useForgotPasswordDiscloresure = useForgotPasswordDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useLoginDiscloresure,
                useRegisterDiscloresure,
                useChangePasswordDiscloresure,
                useForgotPasswordDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
