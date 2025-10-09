"use client"

import React, { useMemo, useState } from "react"
import { PaginationStyled } from "@/components"
import VehicleHorizontalCard from "@/components/modules/VehicleHorizontalCard"
import { VehicleModelViewRes } from "@/models/vehicle/schema/response"
/** ====== Giả lập kiểu dữ liệu backend ====== */
type StaffItem = { id: number; name: string; email: string }

type PagedResult<T> = {
    items: T[] // Items
    pageNumber: number // PageNumber (1-based)
    pageSize: number // PageSize
    totalCount: number // TotalCount
    totalPage: number // TotalPage = ceil(totalCount / pageSize)
}

/** ====== HARD-CODE DATA ====== */
const TOTAL_COUNT = 32 // TotalCount (cứng)
const DEFAULT_PAGE_SIZE = 10 // PageSize (cứng 10 như yêu cầu)
const ALL_ITEMS: StaffItem[] = Array.from({ length: TOTAL_COUNT }, (_, i) => ({
    id: i + 1,
    name: `User ${i + 1}`,
    email: `user${i + 1}@example.com`
}))

const mockVehicleModel: VehicleModelViewRes = {
    id: "demo-model-1",
    name: "VinFast VF7",
    description: "Premium electric sedan with outstanding range and technology.",
    costPerDay: 1500000,
    depositFee: 5000000,
    seatingCapacity: 5,
    numberOfAirbags: 10,
    motorPower: 670,
    batteryCapacity: 100,
    ecoRangeKm: 650,
    sportRangeKm: 520,
    brand: {
        id: "brand-1",
        name: "VinFast",
        description: "Leading innovator in electric vehicles.",
        country: "Viet Nam",
        foundedYear: 2003,
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    },
    segment: {
        id: "segment-1",
        name: "SUV",
        description: "High-end sedans with premium comfort and performance.",
        createdAt: "2024-01-01T00:00:00.000Z",
        updatedAt: "2024-01-01T00:00:00.000Z"
    },
    imageUrl: "https://res.cloudinary.com/dryp7ecsf/image/upload/v1759983843/shopping_p30zvg.webp",
    availableVehicleCount: 3
}

/** Hàm giả lập backend trả về PagedResult<T> */
function getPagedStaff(pageNumber: number, pageSize: number): PagedResult<StaffItem> {
    const totalCount = TOTAL_COUNT
    const totalPage = Math.max(1, Math.ceil(totalCount / Math.max(1, pageSize)))
    const safePage = Math.min(Math.max(1, pageNumber), totalPage)
    const start = (safePage - 1) * pageSize
    const end = start + pageSize

    return {
        items: ALL_ITEMS.slice(start, end),
        pageNumber: safePage,
        pageSize,
        totalCount,
        totalPage
    }
}

/** ====== PAGE DEMO (hardcode) ====== */
export default function StaffTestPage() {
    const [page, setPage] = useState(1)
    const pageSize = DEFAULT_PAGE_SIZE

    // “Gọi backend” nhưng ở đây là tính cứng trong memory
    const data = useMemo(() => getPagedStaff(page, pageSize), [page, pageSize])

    return (
        <div className="rounded-2xl bg-white px-6 py-8 shadow-sm space-y-6">
            <header>
                <h1 className="text-2xl font-semibold text-gray-900">Test Page (Hardcoded)</h1>
            </header>

            {/* Bảng xem trước Items hiện tại */}
            <section className="space-y-3">
                <div className="rounded-xl border border-dashed border-slate-200 p-4 text-sm text-slate-700">
                    <div>
                        Page: <b>{data.pageNumber}</b> / <b>{data.totalPage}</b>
                    </div>
                    <div>
                        PageSize: <b>{data.pageSize}</b>
                    </div>
                    <div>
                        TotalCount: <b>{data.totalCount}</b>
                    </div>
                </div>

                <div className="rounded-xl border border-slate-200 overflow-hidden">
                    <table className="w-full text-left text-sm">
                        <thead className="bg-slate-50 text-slate-600">
                            <tr>
                                <th className="px-4 py-2">ID</th>
                                <th className="px-4 py-2">Name</th>
                                <th className="px-4 py-2">Email</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.items.map((it) => (
                                <tr key={it.id} className="border-t">
                                    <td className="px-4 py-2">{it.id}</td>
                                    <td className="px-4 py-2">{it.name}</td>
                                    <td className="px-4 py-2">{it.email}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

                {/* Phân trang dùng PaginationStyled */}
                <div className="pt-2">
                    <PaginationStyled
                        pageNumber={data.pageNumber} // PageNumber từ “backend”
                        pageSize={data.pageSize} // PageSize từ “backend”
                        totalItems={data.totalCount} // TotalCount từ “backend”
                        onPageChange={setPage} // đổi trang -> tính lại data
                        showControls
                        // // Dùng màu hệ thống (có thể đổi):
                        // activeBgClass="bg-primary"
                        // activeTextClass="text-white"
                        // controlHoverClassName="hover:bg-primary hover:text-white"
                        // // Không set prevContent/nextContent -> mặc định hiển thị "<" / ">"
                    />
                </div>
            </section>

            <section className="space-y-4">
                <h2 className="text-xl font-semibold text-gray-900">Vehicle Card Demo</h2>
                <div className="max-w-7xl">
                    <VehicleHorizontalCard vehicleModel={mockVehicleModel} />
                </div>
            </section>
        </div>
    )
}
