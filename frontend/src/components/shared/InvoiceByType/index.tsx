import { InvoiceType } from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
import React from "react"

export default function InvoidByType(invoice: InvoiceViewRes) {
    switch (invoice.InvoiceType){
        case InvoiceType.BookingPayment:
            return(
                <>
                    <p><strong>Tiền thuê xe:</strong> {currentcy({invoice.subtotal})}</p>
                </>
            )
    }
    return <div>index</div>
}
