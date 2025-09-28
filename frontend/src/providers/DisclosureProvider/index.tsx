import React, { type PropsWithChildren } from "react"
import { useCreatePostDisclosureCore } from "../../hooks/singleton/disclosures/useCreatePostDisclosure"
import { DisclosureContext } from "./DisclosureContext"
import { useLoginDiscloresureCore } from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useCreatePostDisclosure = useCreatePostDisclosureCore()
    const useLoginDiscloresure = useLoginDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useCreatePostDisclosure,
                useLoginDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
