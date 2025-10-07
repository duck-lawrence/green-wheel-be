"use client"
import { FilterVehicleRental } from "@/components"
import CardVehicalStyled from "@/components/modules/VehicleCard"
import { useBookingFilterStore, useNavbarItemStore } from "@/hooks"
import Link from "next/link"
import React, { useEffect } from "react"

export default function VehicleModelsPage() {
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)
    const vehicleModels = useBookingFilterStore((s) => s.filteredVehicleModels)
    // const [vehicles, setVehicles] = useState<Vehicle[]>(vehicleData)
    useEffect(() => {
        setActiveMenuKey("vehicle-rental")
    }, [setActiveMenuKey])

    // useEffect(() => {
    //     if (!station || !start || !end) return
    //     const filtered = allVehicles.filter(
    //         (v) => v.station === station
    //     )
    //     setFilteredVehicles(filtered)
    // }, [station, start, end, allVehicles, setFilteredVehicles])

    // const handleFilter = (station: string, start: string, end: string) => {
    //     setBookingFilter(station, start, end)
    // }
    return (
        <div className="min-h-[80vh] h-fit p-4">
            <FilterVehicleRental />

            <div className="mt-10 gap-8 grid grid-cols-2 sm:grid-cols-3 ">
                {vehicleModels &&
                    vehicleModels.map((vehicleModel) => (
                        <Link key={vehicleModel.id} href={`/vehicle-rental/${vehicleModel.id}`}>
                            <CardVehicalStyled vehicleModel={vehicleModel} />
                        </Link>
                    ))}
            </div>
        </div>
    )
}
