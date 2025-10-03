"use client"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
import React, { useEffect } from "react"

export default function VehicleRental() {
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)

    useEffect(() => {
        setActiveMenuKey("vehicle-rental")
    }, [setActiveMenuKey])

    return <div>page</div>
}
