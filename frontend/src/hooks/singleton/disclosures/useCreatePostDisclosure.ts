"use client"
import { DisclosureContext } from "@/providers/DisclosureProvider/DisclosureContext"
import { useDisclosure } from "@heroui/react"
import { useContext } from "react"
export const useCreatePostDisclosureCore = () => {
    return useDisclosure()
}

export const useCreatePostDisclosureSingleton = () => {
    const { useCreatePostDisclosure } = useContext(DisclosureContext)!
    return useCreatePostDisclosure
}
