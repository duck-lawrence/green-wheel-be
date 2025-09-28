"use client"
import { useCreatePostDisclosureCore, useLoginDiscloresureCore } from "@/hooks"
import { createContext } from "react"

export interface DisclosureContextType {
    useCreatePostDisclosure: ReturnType<typeof useCreatePostDisclosureCore>
    useLoginDiscloresure: ReturnType<typeof useLoginDiscloresureCore>
}

export const DisclosureContext = createContext<DisclosureContextType | null>(null)
