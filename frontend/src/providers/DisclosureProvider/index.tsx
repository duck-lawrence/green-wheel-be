import React, { type PropsWithChildren } from "react"
import { useCreatePostDisclosureCore } from "../../hooks/singleton/disclosures/useCreatePostDisclosure"
import { DisclosureContext } from "./DisclosureContext"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useCreatePostDisclosure = useCreatePostDisclosureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useCreatePostDisclosure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
