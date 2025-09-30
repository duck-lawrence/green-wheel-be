"use client"
import { OrderStatus } from "@/constants/enum"
import { OrderStatusLabels } from "@/constants/orderStatusLabels"
import { Autocomplete, AutocompleteItem } from "@heroui/react"
import React from "react"

type StatusOrderProps = {
    value: OrderStatus | null
    onChange: (value: OrderStatus) => void
}

const statusItems = Object.values(OrderStatus)
    .filter((v) => typeof v === "number")
    .map((v) => ({
        key: String(v), // change key => string
        label: OrderStatusLabels[v as OrderStatus]
    }))

export function StatusOrderPicker({ value, onChange }: StatusOrderProps) {
    return (
        <Autocomplete
            items={statusItems}
            variant="bordered"
            isClearable
            className="max-w-55 h-14"
            label="Status"
            // placeholder=""
            selectedKey={value !== null ? String(value) : undefined} //é sang pstring mới hiện value trong ô
            onSelectionChange={(key) => {
                if (key !== null) {
                    onChange(Number(key) as OrderStatus) // convert ngược lại về number để lưu vào formik
                    console.log("Selected:", key)
                }
            }}
        >
            {(item) => <AutocompleteItem key={item.key}>{item.label}</AutocompleteItem>}
        </Autocomplete>
    )
}
