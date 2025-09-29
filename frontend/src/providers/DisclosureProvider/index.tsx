import React, { type PropsWithChildren } from "react"
import { useCreatePostDisclosureCore } from "../../hooks/singleton/disclosures/useCreatePostDisclosure"
import { DisclosureContext } from "./DisclosureContext"
import { useLoginDiscloresureCore, useRegisDiscloresureCore } from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useCreatePostDisclosure = useCreatePostDisclosureCore()
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisDiscloresure = useRegisDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useCreatePostDisclosure,
                useLoginDiscloresure,
                useRegisDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
