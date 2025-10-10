import { InvoiceItemType } from "@/constants/enum"
import { InvoiceViewRes } from "@/models/invoice/schema/response"

export const getDamageNotes = (invoice: InvoiceViewRes): string => {
    const notes = invoice.items
        .filter((value) => value.type === InvoiceItemType.Damage && value.checkListItem?.notes)
        .map((value) => `• ${value.checkListItem!.notes}`)
        .join("\n")
    return notes || "Không có phí thiệt hại nào."
}

export const getOtherNotes = (invoice: InvoiceViewRes): string => {
    const notes = invoice.items
        .filter((value) => value.type === InvoiceItemType.Other && value.checkListItem?.notes)
        .map((value) => `• ${value.checkListItem!.notes}`)
        .join("\n")
    return notes || "Không có phí thiệt hại nào."
}
