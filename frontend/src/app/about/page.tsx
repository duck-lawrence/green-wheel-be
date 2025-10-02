"use client"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
import React, { useEffect } from "react"

export default function AboutPage() {
    const { setActiveMenuKey } = useNavbarItemStore()

    useEffect(() => {
        setActiveMenuKey("about")
    }, [setActiveMenuKey])

    return <div>page</div>
}
