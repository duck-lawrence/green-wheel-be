import { InvoiceType } from "@/constants/enum"
import InvoiceHandOvertForm from "./InvoiceHandOverForm"
import InvoiceReturnForm from "./InvoiceReturnForm"
import InvoiceRefundForm from "./InvoiceRefundForm"
import InvoiceOtherForm from "./InvoiceOtherForm"
import React from "react"
import { InvoiceViewRes } from "@/models/invoice/schema/response"
export const renderInvoiceForm = (invoice: InvoiceViewRes) => {
    switch (invoice.type) {
        case InvoiceType.HandoverPayment:
            return <InvoiceHandOvertForm invoice={invoice} />
        case InvoiceType.ReturnPayment:
            return <InvoiceReturnForm invoice={invoice} />
        case InvoiceType.RefundPayment:
            return <InvoiceRefundForm invoice={invoice} />
        case InvoiceType.OtherPayment:
            return <InvoiceOtherForm invoice={invoice} />
        default:
            return null
    }
}
