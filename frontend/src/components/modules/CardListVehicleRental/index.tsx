"use client"
import React from "react"
import CardVehicalStyled from "./CardVehicalStyled"
import { useBookingFilterStore } from "@/hooks"
// import Vehicle from "@/models/user/type/vehicle"

export function CardListVehicleRental() {
    const vehicleModels = useBookingFilterStore((s) => s.filteredVehicleModels)

    return (
        <div className="gap-8 grid grid-cols-2 sm:grid-cols-3 ">
            {vehicleModels &&
                vehicleModels.map((vehicleModel) => (
                    <CardVehicalStyled vehicleModel={vehicleModel} key={vehicleModel.id} />
                ))}
        </div>
    )
}
