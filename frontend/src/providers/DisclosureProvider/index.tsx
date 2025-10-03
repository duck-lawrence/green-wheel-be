import React, { type PropsWithChildren } from "react"
import { DisclosureContext } from "./DisclosureContext"
import {
    useSetPasswordDiscloresureCore,
    useForgotPasswordDiscloresureCore,
    useLoginDiscloresureCore,
    useRegisterDiscloresureCore
} from "@/hooks"

export const DisclosureProvider = ({ children }: PropsWithChildren) => {
    const useLoginDiscloresure = useLoginDiscloresureCore()
    const useRegisterDiscloresure = useRegisterDiscloresureCore()
    const useSetPasswordDiscloresure = useSetPasswordDiscloresureCore()
    const useForgotPasswordDiscloresure = useForgotPasswordDiscloresureCore()

    return (
        <DisclosureContext.Provider
            value={{
                useLoginDiscloresure,
                useRegisterDiscloresure,
                useSetPasswordDiscloresure,
                useForgotPasswordDiscloresure
            }}
        >
            {children}
        </DisclosureContext.Provider>
    )
}
