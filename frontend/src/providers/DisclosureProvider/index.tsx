import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import { useLoginDiscloresureCore, useRegisterDiscloresureCore } from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisterDiscloresure = useRegisterDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useLoginDiscloresure,
                useRegisterDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
