import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import { useLoginDiscloresureCore, useRegisDiscloresureCore } from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisDiscloresure = useRegisDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useLoginDiscloresure,
                useRegisDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
