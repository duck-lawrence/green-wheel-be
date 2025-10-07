"use client"
import {
    useLoginDiscloresureCore,
    useRegisterDiscloresureCore,
    useSetPasswordDiscloresureCore,
    useForgotPasswordDiscloresureCore
} from "@/hooks"
import { createContext } from "react"

export interface DisclosureContextType {
    useLoginDiscloresure: ReturnType<typeof useLoginDiscloresureCore>
    useRegisterDiscloresure: ReturnType<typeof useRegisterDiscloresureCore>
    useSetPasswordDiscloresure: ReturnType<typeof useSetPasswordDiscloresureCore>
    useForgotPasswordDiscloresure: ReturnType<typeof useForgotPasswordDiscloresureCore>
}

export const DisclosureContext = createContext<DisclosureContextType | null>(null)
