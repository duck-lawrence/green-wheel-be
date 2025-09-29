import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import { useCreatePostDisclosureCore, useLoginDiscloresureCore } from "@/hooks/"

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
