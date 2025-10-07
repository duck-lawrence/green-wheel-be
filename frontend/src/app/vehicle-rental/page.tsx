"use client"
import { CardListVehicleRental, FilterVehicleRental } from "@/components"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
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

    // const handleFilter = (station: string, start: string, end: string) => {
    //     setBookingFilter(station, start, end)
    // }
    return (
        <div className="min-h-[80vh] h-fit p-4">
            <div className="">
                <FilterVehicleRental />
            </div>

            <div className="mt-10">
                <CardListVehicleRental />
            </div>
        </div>
    )
}
