"use client"
import { ButtonStyled } from "@/components"
import { useRegisDiscloresureSingleton } from "@/hooks"
import React from "react"

export default function Home() {
    const { onOpen: onOpenRegis } = useRegisDiscloresureSingleton()
    return (
        <>
            <h1>Home Page</h1>
            <ButtonStyled className="mt-10" onPress={onOpenRegis}>
                Regis
            </ButtonStyled>
        </>
    )
}
