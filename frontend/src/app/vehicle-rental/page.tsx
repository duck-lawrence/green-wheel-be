"use client"
import { FilterVehicleRental, CardListVehicleRental } from "@/components"
import { useNavbarItemStore } from "@/hooks"
import React, { useEffect } from "react"
// import Vehicle from "@/models/user/type/vehicle"

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

    return (
        <div className="h-30">
            <div className="mt-30">
                <FilterVehicleRental />
            </div>

            <div className="mt-10">
                <CardListVehicleRental />
            </div>
        </div>
    )
}
