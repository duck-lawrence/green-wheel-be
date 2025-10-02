"use client"
import {
    useLoginDiscloresureCore,
    useRegisterDiscloresureCore,
    useChangePasswordDiscloresureCore
} from "@/hooks"
import { createContext } from "react"

export interface DisclosureContextType {
    useLoginDiscloresure: ReturnType<typeof useLoginDiscloresureCore>
    useRegisterDiscloresure: ReturnType<typeof useRegisterDiscloresureCore>
    useChangePasswordDiscloresure: ReturnType<typeof useChangePasswordDiscloresureCore>
}

export const DisclosureContext = createContext<DisclosureContextType | null>(null)
