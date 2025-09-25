"use client"
import React from "react"
import { HeroUIProvider } from "@heroui/react"
import { getQueryClient } from "@/utils/helpers/getQueryClient"
import { QueryClientProvider } from "@tanstack/react-query"
import { DisclosureProvider } from "./DisclosureProvider"
import { ClientI18nProvider } from "./ClientI18nProvider"

export function AppProviders({ children, locale }: { children: React.ReactNode; locale: string }) {
    const queryClient = getQueryClient()

    return (
        <QueryClientProvider client={queryClient}>
            <HeroUIProvider>
                <ClientI18nProvider locale={locale}>
                    <DisclosureProvider>{children}</DisclosureProvider>
                </ClientI18nProvider>
            </HeroUIProvider>
        </QueryClientProvider>
    )
}
