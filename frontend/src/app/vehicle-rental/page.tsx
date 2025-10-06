"use client"
import { FilterVehicleRental, CardListVehicleRental } from "@/components"
import { useNavbarItemStore } from "@/hooks"
import React, { useEffect } from "react"

export default function VehicleRentalPage() {
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)
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
    const handleFilter = (station: string, start: string, end: string) => {
        setBookingFilter(station, start, end)
    }
    return (
        <div className="h-30 mt-30 min-h-[80vh]">
            <div className="mt-30">
                <FilterVehicleRental />
            </div>

            <div className="mt-10">
                <CardListVehicleRental />
            </div>
        </div>
    )
}
