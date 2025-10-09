import { InvoiceViewRes } from "@/models/invoice/schema/response"
import React from "react"

export default function DetailDamage({ data }: { data: InvoiceViewRes }) {
    // Lấy danh sách items trong hóa đơn
    // Nếu items có thể undefined thì thêm optional chaining
    const damageItems = data.items?.filter((item) => item.type === 1) || []

    return (
        <div className="space-y-3">
            {damageItems.length > 0 ? (
                damageItems.map((v, i) => (
                    <div
                        key={v.id || i}
                        className="p-3 border border-gray-200 rounded-lg bg-gray-50 dark:bg-gray-800"
                    >
                        <p className="font-semibold text-gray-800 dark:text-gray-100">
                            {v.quantity || "Hạng mục hư hỏng"}
                        </p>
                        <p className="text-sm text-gray-500 dark:text-gray-300">
                            Mô tả: {v.unitPrice || "Không có mô tả"}
                        </p>
                        <p className="text-sm text-primary font-medium">
                            Chi phí: {v.subTotal?.toLocaleString("vi-VN")} VND
                        </p>
                    </div>
                ))
            ) : (
                <p className="text-gray-500 text-center italic">Không có hạng mục hư hỏng nào</p>
            )}
        </div>
    )
}
