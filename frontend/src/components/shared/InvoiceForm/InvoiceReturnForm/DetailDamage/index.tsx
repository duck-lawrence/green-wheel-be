import { InvoiceViewRes } from "@/models/invoice/schema/response"
import { InvoiceItemType } from "@/constants/enum"
import React from "react"
import { formatCurrency } from "@/utils/helpers/currency"

export default function DetailDamage({ invoice }: { invoice: InvoiceViewRes }) {
    // Lọc ra các item có type = Damage
    const damageItems = invoice.items?.filter((item) => item.type === InvoiceItemType.Damage) ?? [] // dùng nullish coalescing thay vì ||

    return (
        <div className="space-y-3">
            {damageItems.length > 0 ? (
                damageItems.map((value) => (
                    <div
                        key={value.id}
                        className="p-3 border border-gray-200 rounded-lg bg-gray-50 dark:bg-gray-800"
                    >
                        <p className="font-semibold text-gray-800 dark:text-gray-100">
                            {/* Nếu có checkListItem.component.name thì show */}
                            {value.checkListItem?.component?.name || "Hạng mục hư hỏng"}
                        </p>
                        <p className="text-sm text-gray-500 dark:text-gray-300">
                            Mô tả: {value.checkListItem?.notes || "Không có mô tả"}
                        </p>
                        <p className="text-sm text-primary font-medium">
                            Chi phí: {formatCurrency(value.subTotal ?? 0)}
                        </p>
                    </div>
                ))
            ) : (
                <p className="text-gray-500 text-center italic">Không có hạng mục hư hỏng nào</p>
            )}
        </div>
    )
}
