"use client"
// import BrandPicker from "@/components/modules/UserItem/BrandPicker"
import TableStyled from "@/components/styled/TableStyled"
import React, { useEffect, useState } from "react"
import { orders } from "@/data/order"
import FillterBarOrder from "@/components/modules/UserItem/FilterBarOrder"

export default function Page() {
    const [order, setOrder] = useState(orders)
    const [filters, setFilter] = useState()

    useEffect(() => {
        if (filters) {
            console.log("data filter gửi BE:", filters)
            //fetch api
        }
    }, [filters])
    return (
        <div>
            <div className="text-3xl mb-4 p-4 font-bold">
                <p>Lịch sử đơn hàng</p>
            </div>

            <div className="p-4">
                <FillterBarOrder onFilterChange={(fillters) => setFilter(fillters)} />
            </div>

            <div className="p-4">
                <TableStyled data={order} loading={false} />
            </div>
        </div>
    )
}
