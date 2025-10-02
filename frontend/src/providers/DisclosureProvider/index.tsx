import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import {
    useChangePasswordDiscloresureCore,
    useLoginDiscloresureCore,
    useRegisterDiscloresureCore
} from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisterDiscloresure = useRegisterDiscloresureCore()
    const useChangePasswordDiscloresure = useChangePasswordDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useLoginDiscloresure,
                useRegisterDiscloresure,
                useChangePasswordDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
