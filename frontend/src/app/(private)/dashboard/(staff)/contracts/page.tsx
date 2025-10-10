"use client"

import React, { useEffect, useState } from "react"
import { useTranslation } from "react-i18next"
import { FillterBarOrder } from "@/components/shared/User/FilterBarOrder"
import TableStyled from "@/components/styled/TableStyled"
import { orders } from "@/data/order"

export default function StaffContractsPage() {
    const { t } = useTranslation()
    const [order] = useState(orders)
    const [loading] = useState(false)
    const [filters, setFilter] = useState({})

    useEffect(() => {
        if (filters) {
            console.log("[staff/contracts] filters sent to BE:", filters)
        }
    }, [filters])

    return (
        <div className="rounded-2xl bg-white shadow-sm px-6 py-6">
            <div className="text-3xl mb-3 px-4 font-bold">
                <p>{t("staff.sidebar_contracts")}</p>
            </div>

            <div className="mb-4">
                <FillterBarOrder onFilterChange={() => setFilter(filters)} />
            </div>

            <TableStyled data={order} loading={loading} />
        </div>
    )
}
