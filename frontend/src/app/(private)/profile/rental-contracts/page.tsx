"use client"
// import BrandPicker from "@/components/modules/UserItem/BrandPicker"
import React, { useEffect, useState } from "react"
import { FillterBarOrder } from "@/components/shared/User/FilterBarOrder"
import TableStyled from "@/components/styled/TableStyled"
import { orders } from "@/data/order"
import { useTranslation } from "react-i18next"

export default function Page() {
    const { t } = useTranslation()
    const [order] = useState(orders) // thôn tin đơn hàng
    const [loading] = useState(false)
    const [filters, setFilter] = useState({})

    useEffect(() => {
        if (filters) {
            console.log("data filter gửi BE:", filters)
            //fetch api
        }
    }, [filters])
    return (
        <div>
            <div className="text-3xl mb-3 px-4 font-bold">
                <p>{t("user.rental_contracts")}</p>
            </div>

            <div className="mb-3 px-4">
                <FillterBarOrder onFilterChange={() => setFilter(filters)} />
            </div>

            <div className="px-4">
                <TableStyled data={order} loading={loading} />
            </div>
        </div>
    )
}
