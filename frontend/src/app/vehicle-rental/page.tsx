"use client"
import { FilterVehicleRental, CardListVehicleRental } from "@/components"
import { vehicleData } from "@/data/vehicleData"
import { useBookingStore, useNavbarItemStore } from "@/hooks"
import React, { useEffect } from "react"
// import Vehicle from "@/models/user/type/vehicle"

export default function VehicleRental() {
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)
    const { setBookingInfo, setFilteredVehicles } = useBookingStore()
    // const [vehicles, setVehicles] = useState<Vehicle[]>(vehicleData)
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
