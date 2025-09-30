"use client"
import { useLoginDiscloresureCore, useRegisterDiscloresureCore } from "@/hooks"
import { createContext } from "react"

export interface DisclosureContextType {
    useLoginDiscloresure: ReturnType<typeof useLoginDiscloresureCore>
    useRegisterDiscloresure: ReturnType<typeof useRegisterDiscloresureCore>
}

export const DisclosureContext = createContext<DisclosureContextType | null>(null)
