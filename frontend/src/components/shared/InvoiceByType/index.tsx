
import { InvoiceType } from "@/constants/enum"
import { InvoiceItemViewRes, InvoiceViewRes } from "@/models/invoice/schema/response"
import React from "react"

export default function InvoidByType(invoice: InvoiceItemViewRes) {
    switch (invoice.type){
        case InvoiceType.BookingPayment:
            return(
                <>
                    <p><strong>Tiền thuê xe:</strong> {currentcy({invoice.subtotal})}</p>
                </>
            )
    }
    return <div>index</div>
}
