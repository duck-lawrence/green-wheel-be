"use client"
import { CardListVehicleRental, FilterVehicleRental } from "@/components"
// import CardListVehicleRental from "@/components/modules/CardListVehicleRental"
import { vehicleData } from "@/data/vehicleData"
import { useBookingStore } from "@/hooks"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
import Vehicle from "@/models/user/type/vehicle"
import React, { useEffect, useState } from "react"

export default function VehicleRental() {
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)
    const { setBookingInfo, setFilteredVehicles } = useBookingStore()
    const [vehicles, setVehicles] = useState<Vehicle[]>(vehicleData)
    useEffect(() => {
        setActiveMenuKey("vehicle-rental")
    }, [setActiveMenuKey])

    useEffect(() => {
        setFilteredVehicles(vehicleData)
    }, [setFilteredVehicles])
    // useEffect(() => {
    //     if (!station || !start || !end) return
    //     const filtered = allVehicles.filter(
    //         (v) => v.station === station
    //     )
    //     setFilteredVehicles(filtered)
    // }, [station, start, end, allVehicles, setFilteredVehicles])

    const handleFilter = (station: string, start: string, end: string) => {
        setBookingInfo(station, start, end)
    }
    return (
        <div className="h-30">
            <div className="mt-30">
                <FilterVehicleRental onFilterChange={handleFilter} />
            </div>

            <div className="mt-10">
                <CardListVehicleRental />
            </div>
        </div>
    )
}
