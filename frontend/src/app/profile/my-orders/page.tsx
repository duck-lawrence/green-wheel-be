"use client"
// import BrandPicker from "@/components/modules/UserItem/BrandPicker"
import React, { useEffect, useState } from "react"
import { FillterBarOrder } from "@/components/shared/User/FilterBarOrder"
import TableStyled from "@/components/styled/TableStyled"
import { orders } from "@/data/order"
import { useTranslation } from "react-i18next"

export default function Page() {
    const { t } = useTranslation()
    const [order, setOrder] = useState(orders) // thôn tin đơn hàng
    const [loading, setLoading] = useState(false)
    const [filters, setFilter] = useState({})

    useEffect(() => {
        if (filters) {
            console.log("data filter gửi BE:", filters)
            //fetch api
        }
    }, [filters])
    return (
        <div>
            <div className="text-3xl mb-4 p-4 font-bold">
                <p>{t("user.booking_history")}</p>
            </div>

            <div className="p-4">
                <FillterBarOrder onFilterChange={() => setFilter(filters)} />
            </div>

            <div className="p-4">
                <TableStyled data={order} loading={loading} />
            </div>
        </div>
    )
}
