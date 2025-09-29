"use client"
import {
    useCreatePostDisclosureCore,
    useLoginDiscloresureCore,
    useRegisDiscloresureCore
} from "@/hooks"
import { createContext } from "react"

export interface DisclosureContextType {
    useCreatePostDisclosure: ReturnType<typeof useCreatePostDisclosureCore>
    useLoginDiscloresure: ReturnType<typeof useLoginDiscloresureCore>
    useRegisDiscloresure: ReturnType<typeof useRegisDiscloresureCore>
}

export const DisclosureContext = createContext<DisclosureContextType | null>(null)
