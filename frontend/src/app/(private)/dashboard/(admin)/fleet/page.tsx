"use client"

import React, { useMemo, useState } from "react"
import { SelectItem, Selection } from "@heroui/react"
import VehicleHorizontalCard from "@/components/modules/VehicleHorizontalCard"
import { ButtonStyled, FilterTypeStyle, PaginationStyled, SearchStyle } from "@/components"
import { VehicleModelViewRes } from "@/models/vehicle-model/schema/response"

const DEFAULT_PAGE_SIZE = 10
const TOTAL_VEHICLES = 105

const BRAND_POOL = [
    {
        id: "brand-1",
        name: "VinFast",
        description: "Leading innovator in electric vehicles.",
        country: "Viet Nam",
        foundedYear: 2017,
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    },
    {
        id: "brand-2",
        name: "VinFast",
        description: "Pioneer in smart EV technology.",
        country: "Viet Nam",
        foundedYear: 2003,
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    },
    {
        id: "brand-3",
        name: "VinFast",
        description: "Global leader in sustainable mobility.",
        country: "Viet Nam",
        foundedYear: 1995,
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    }
]

const SEGMENT_POOL = [
    {
        id: "segment-1",
        name: "SUV",
        description: "Practical SUVs for every terrain.",
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    },
    {
        id: "segment-2",
        name: "Sedan",
        description: "Comfort-focused sedans for city rides.",
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    },
    {
        id: "segment-3",
        name: "Hatchback",
        description: "Compact hatchbacks with efficient designs.",
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    }
]

const IMAGE_POOL = [
    "https://vinfastauto.eu/themes/custom/vinfast_v2/images/v3/vf-8/exterior-color-white.webp",
    "https://vinfastauto.us/themes/custom/vinfast_v2/images/v3/homepage/header-vf-9.webp",
    "https://vinfast-mienbac.com/wp-content/uploads/2024/10/vf5-7.png",
    // "https://images.unsplash.com/photo-1525609004556-c46c7d6cf023?q=80&w=1200",
    // "https://images.unsplash.com/photo-1523987355523-c7b5b84b79a0?q=80&w=1200"
]

const ADMIN_FLEET: VehicleModelViewRes[] = Array.from({ length: TOTAL_VEHICLES }, (_, index) => {
    const brand = BRAND_POOL[index % BRAND_POOL.length]
    const segment = SEGMENT_POOL[index % SEGMENT_POOL.length]
    const imageUrl = IMAGE_POOL[index % IMAGE_POOL.length]

    return {
        id: `demo-fleet-${index + 1}`,
        name: `${brand.name} Fleet ${index + 1}`,
        description: `${brand.name} ${segment.name} model prepared for admin fleet preview.`,
        costPerDay: 1200000 + (index % 6) * 50000,
        depositFee: 5000000 + (index % 4) * 250000,
        seatingCapacity: 4 + (index % 4),
        numberOfAirbags: 6 + (index % 5) * 2,
        motorPower: 160 + (index % 6) * 25,
        batteryCapacity: 55 + (index % 5) * 8,
        ecoRangeKm: 360 + (index % 6) * 35,
        sportRangeKm: 290 + (index % 6) * 28,
        brand,
        segment,
        imageUrl,
        imageUrls: [imageUrl],
        availableVehicleCount: (index % 9) + 1
    }
})

const CAR_TYPE_OPTIONS = [
    { key: "suv", label: "SUV" },
    { key: "sedan", label: "Sedan" },
    { key: "hatchback", label: "Hatchback" },
    { key: "crossover", label: "Crossover" }
]

const STATUS_OPTIONS = [
    { key: "available", label: "Available" },
    { key: "reserved", label: "Reserved" },
    { key: "maintenance", label: "Maintenance" }
]

export default function AdminFleetPage() {
    const [page, setPage] = useState(1)
    const [searchTerm, setSearchTerm] = useState("")
    const [carType, setCarType] = useState<string | undefined>()
    const [status, setStatus] = useState<string | undefined>()

    const totalVehicles = ADMIN_FLEET.length
    const totalPages = Math.max(1, Math.ceil(totalVehicles / DEFAULT_PAGE_SIZE))
    const currentPage = Math.min(Math.max(page, 1), totalPages)

    const currentVehicles = useMemo(() => {
        const start = (currentPage - 1) * DEFAULT_PAGE_SIZE
        const end = start + DEFAULT_PAGE_SIZE
        return ADMIN_FLEET.slice(start, end)
    }, [currentPage])

    const handlePageChange = (nextPage: number) => {
        setPage(Math.min(Math.max(nextPage, 1), totalPages))
    }

    const handleCarTypeChange = (keys: Selection) => {
        if (keys === "all") {
            setCarType(undefined)
            return
        }
        const value = Array.from(keys)[0]
        setCarType(typeof value === "string" ? value : value?.toString())
    }

    const handleStatusChange = (keys: Selection) => {
        if (keys === "all") {
            setStatus(undefined)
            return
        }
        const value = Array.from(keys)[0]
        setStatus(typeof value === "string" ? value : value?.toString())
    }

    const itemStart = (currentPage - 1) * DEFAULT_PAGE_SIZE + 1
    const itemEnd = Math.min(itemStart + DEFAULT_PAGE_SIZE - 1, totalVehicles)

    return (
        <div className="space-y-6">
            <header className="rounded-2xl bg-white px-6 py-5 shadow-sm">
                <div className="flex flex-col gap-4 lg:flex-row lg:items-center lg:gap-6">
                    <div>
                        {/* <h1 className="text-2xl font-semibold text-slate-900">Fleet Overview (Demo)</h1> */}
                        {/* <p className="text-sm text-slate-600">
                             {itemStart}&ndash;
                            {itemEnd} of {totalVehicles}.
                        </p> */}
                    </div>
                    <div className="flex w-full flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
                        <div className="flex w-full flex-col gap-3 sm:flex-1 sm:flex-row sm:items-center sm:gap-3">
                            <SearchStyle
                                placeholder="Search client name, car, etc"
                                value={searchTerm}
                                onValueChange={setSearchTerm}
                                className="sm:w-60 lg:w-72"
                            />
                            <FilterTypeStyle
                                placeholder="Car Type"
                                selectedKeys={carType ? new Set([carType]) : new Set([])}
                                onSelectionChange={handleCarTypeChange}
                                className="sm:w-44"
                            >
                                {CAR_TYPE_OPTIONS.map((option) => (
                                    <SelectItem key={option.key}>{option.label}</SelectItem>
                                ))}
                            </FilterTypeStyle>
                            <FilterTypeStyle
                                placeholder="Status"
                                selectedKeys={status ? new Set([status]) : new Set([])}
                                onSelectionChange={handleStatusChange}
                                className="sm:w-40"
                            >
                                {STATUS_OPTIONS.map((option) => (
                                    <SelectItem key={option.key}>{option.label}</SelectItem>
                                ))}
                            </FilterTypeStyle>
                        </div>
                        <ButtonStyled
                            color="danger"
                            radius="lg"
                            className="h-12 px-5 font-semibold sm:ml-3 sm:shrink-0"
                        >
                            Add Unit
                        </ButtonStyled>
                    </div>
                </div>
            </header>

            <section className="space-y-4">
                <div className="flex flex-col gap-4">
                    {currentVehicles.map((vehicle) => (
                        <VehicleHorizontalCard key={vehicle.id} vehicleModel={vehicle} readOnly={false} />
                    ))}
                </div>

                <div className="flex justify-center pt-2">
                    <PaginationStyled
                        pageNumber={currentPage}
                        pageSize={DEFAULT_PAGE_SIZE}
                        totalItems={totalVehicles}
                        onPageChange={handlePageChange}
                        showControls
                    />
                </div>
            </section>
        </div>
    )
}
