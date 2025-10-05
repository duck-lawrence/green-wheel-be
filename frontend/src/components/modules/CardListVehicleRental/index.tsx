"use client"
import React from "react"
import { useBookingFilterStore } from "@/hooks"
import CardVehicalStyled from "@/components/styled/CardVehicalStyled"
// import Vehicle from "@/models/user/type/vehicle"

export function CardListVehicleRental() {
    const vehicles = useBookingFilterStore((s) => s.filteredVehicles)

    return (
        <div className="gap-8 grid grid-cols-2 sm:grid-cols-3 ">
            {vehicles && vehicles.map((car) => <CardVehicalStyled car={car} key={car.id} />)}
        </div>
    )
}
